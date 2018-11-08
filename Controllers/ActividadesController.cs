using Travlr.Models;
using Travlr.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Travlr.Repositories.Database;
using Microsoft.AspNetCore.Identity;
using Repositories;

namespace Travlr.Controllers
{
    [Route("/Grupos/[controller]")]
    [Authorize]

    public class ActividadesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UsuarioRepository _userRepository;
        public IUnitOfWork UnitOfWork { get { return this._unitOfWork; } }
        public UsuarioRepository UsuarioRepository { get { return this._userRepository; } }
        public ActividadesController(DbContextOptions<DataBaseContext> options, UserManager<Usuario> userManager, IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        [HttpGet("ListaActividades")]
        public IActionResult ListaActividades()
        {
            return View();
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
        public IActionResult CrearActividad(GrupoViewModel gvm)
        {
            var actividad = new Actividad { Descripcion = gvm.Actividad.Descripcion, FechaHora = DateTime.Today /* aca llega una fecha */ };
            var disponibles = new List<ActividadConfirmado>();
            var grupo = UnitOfWork.GrupoRepository.GetPeroCompleto(gvm.GrupoID);
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

        [HttpGet("ConfirmarAsistencia")]
        public IActionResult ConfirmarAsistencia(int id)
        {
            return View();
        }

        [HttpPost("ConfirmarAsistencia")]
        public IActionResult ConfirmarAsistencia(GrupoViewModel gvm)
        {
            var logged = UsuarioRepository.UserManager.FindByNameAsync(User.Identity.Name).Result;
            var actividad = UnitOfWork.ActividadRepository.GetPeroCompleto(gvm.Actividad.ID);
            if(actividad.Confirmados.Where(u => u.UsuarioId == logged.Id).FirstOrDefault().Asiste == false)
            {
                actividad.Confirmados.Where(u => u.UsuarioId == logged.Id).FirstOrDefault().Asiste = true;
                UnitOfWork.ActividadRepository.Update(actividad);
                UnitOfWork.Complete();
            }
            return Json("asistis");
        }
    }
}