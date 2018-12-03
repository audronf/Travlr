using Travlr.Models;
using Travlr.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Repositories;
using Microsoft.EntityFrameworkCore;
using Travlr.Repositories.Database;
using Microsoft.AspNetCore.Identity;
using System;

namespace Travlr.Controllers
{
    [Route("api/v1/[controller]")]
    public class ActividadesApiController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UsuarioRepository _userRepository;
        public IUnitOfWork UnitOfWork { get { return this._unitOfWork; } }
        public UsuarioRepository UsuarioRepository { get { return this._userRepository; } }
        public ActividadesApiController(DbContextOptions<DataBaseContext> options, UserManager<Usuario> userManager, IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
            this._userRepository = new UsuarioRepository(options, userManager);
        }

        [HttpGet("ListaActividades")]
        public IActionResult ListaActividades(int id)
        {
            var grupo = UnitOfWork.GrupoRepository.GetPeroCompleto(id);
            var avm = grupo.Actividades.Select(ac => new ActividadViewModel { ActividadID = ac.ID, Descripcion = ac.Descripcion, FechaHora = ac.FechaHora });
            return Json(avm);
        }

        [HttpGet("CrearActividad")]
        public IActionResult CrearActividad(int id)
        {
            var grupo = UnitOfWork.GrupoRepository.GetPeroCompleto(id);
            if (grupo == null)
            {
                return NotFound();
            }
            var grupoVM = new GrupoViewModel { GrupoID = grupo.GrupoID };
            return View(grupoVM);
        }

        [HttpPost("CrearActividad")]
        public IActionResult CrearActividad([FromBody]ActividadViewModel avm)
        {
            var actividad = new Actividad { Descripcion = avm.Descripcion, FechaHora = DateTime.Today };
            var disponibles = new List<ActividadConfirmado>();
            var grupo = UnitOfWork.GrupoRepository.GetPeroCompleto(avm.GrupoID);
            foreach (var usuario in UnitOfWork.UsuarioGrupoRepository.GetAll().Where(g => g.GrupoID == grupo.GrupoID))
            {
                var ac = new ActividadConfirmado { UsuarioId = usuario.UsuarioId, Asiste = false };
                disponibles.Add(ac);
            }
            actividad.Confirmados = disponibles;
            if (grupo.Actividades == null)
            {
                grupo.Actividades = new List<Actividad>();
            }
            grupo.Actividades.Add(actividad);
            UnitOfWork.GrupoRepository.Update(grupo);
            UnitOfWork.Complete();
            return Json("esto funciona");
        }

        [HttpPost("ConfirmarAsistencia")]
        public IActionResult ConfirmarAsistencia([FromBody]ActividadViewModel avm)
        {
            var logged = UsuarioRepository.UserManager.FindByNameAsync(User.Identity.Name).Result;
            var actividad = UnitOfWork.ActividadRepository.GetPeroCompleto(avm.ActividadID);
            if (actividad.Confirmados.Where(u => u.UsuarioId == logged.Id).FirstOrDefault().Asiste == false)
            {
                actividad.Confirmados.Where(u => u.UsuarioId == logged.Id).FirstOrDefault().Asiste = true;
                UnitOfWork.ActividadRepository.Update(actividad);
                UnitOfWork.Complete();
            }
            return Json("asiste");
        }
    }
}