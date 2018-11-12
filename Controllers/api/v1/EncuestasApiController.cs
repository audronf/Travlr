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
    public class EncuestasApiController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UsuarioRepository _userRepository;
        public IUnitOfWork UnitOfWork { get { return this._unitOfWork; } }
        public UsuarioRepository UsuarioRepository { get { return this._userRepository; } }
        public EncuestasApiController(DbContextOptions<DataBaseContext> options, UserManager<Usuario> userManager, IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
            this._userRepository = new UsuarioRepository(options, userManager);
        }

        [HttpGet("ListaEncuestas")]
        public IActionResult ListaEncuestas()
        {
            //copy paste, no lo hice
            var encuestas = UnitOfWork.EncuestaRepository.GetAll().ToList();
            var logged = UsuarioRepository.UserManager.FindByNameAsync(User.Identity.Name).Result;
            var grupos = UnitOfWork.UsuarioGrupoRepository.GetAll().Where(u => u.UsuarioId == logged.Id);
            List<Grupo> gruposUsuario = new List<Grupo>();
            foreach (var grupo in grupos)
            {
                gruposUsuario.Add(UnitOfWork.GrupoRepository.Get(grupo.GrupoID));
            }
            return Json(gruposUsuario.Select(g => new GrupoViewModel { GrupoID = g.GrupoID, Nombre = g.Nombre }));

        }

        [HttpPost("CrearEncuesta")]
        public IActionResult CrearEncuesta([FromBody]GrupoViewModel gvm)
        {
            var grupo = UnitOfWork.GrupoRepository.GetPeroCompleto(gvm.GrupoID);
            var encuesta = new Encuesta { Pregunta = gvm.Encuesta.Pregunta };
            var votacion = new List<Votaron>();
            foreach (var usuario in UnitOfWork.UsuarioGrupoRepository.GetAll().Where(g => g.GrupoID == grupo.GrupoID))
            {
                var vt = new Votaron { UsuarioId = usuario.UsuarioId, Voto = false };
                votacion.Add(vt);
            }
            var OpcionesList = new List<Opcion>();
            foreach (var opcion in gvm.Opciones)
            {
                var op = new Opcion { Texto = opcion, Cantidad = 0 };
                OpcionesList.Add(op);
            }
            encuesta.Opciones = OpcionesList;
            encuesta.Votaron = votacion;
            if (grupo.Encuestas == null)
            {
                grupo.Encuestas = new List<Encuesta>();
            }
            grupo.Encuestas.Add(encuesta);
            UnitOfWork.GrupoRepository.Update(grupo);
            UnitOfWork.Complete();
            return Json(new { mensaje = "encuesta creada" });
        }

        [HttpPost("VotarEncuesta")]
        public IActionResult VotarEncuesta([FromBody]GrupoViewModel gvm)
        {
            var logged = UsuarioRepository.UserManager.FindByNameAsync(User.Identity.Name).Result;
            var encuesta = UnitOfWork.EncuestaRepository.GetPeroCompleto(gvm.Encuesta.ID);
            if (encuesta.Votaron.Where(u => u.UsuarioId == logged.Id).FirstOrDefault().Voto == false)
            {
                encuesta.Votaron.Where(u => u.UsuarioId == logged.Id).FirstOrDefault().Voto = true;
                encuesta.Opciones.Where(o => o.ID == gvm.OpcionSelect /* int del numero de opcion */).FirstOrDefault().Cantidad++;
                UnitOfWork.EncuestaRepository.Update(encuesta);
                UnitOfWork.Complete();
            }
            return Json("votaste");
        }
    }
}