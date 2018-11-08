using Travlr.Models;
using Travlr.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System;
using Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Travlr.Repositories.Database;
using System.Linq;

namespace Travlr.Controllers
{
    [Route("/Grupos/[controller]")]
    [Authorize]

    public class EncuestasController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UsuarioRepository _userRepository;
        public IUnitOfWork UnitOfWork { get { return this._unitOfWork; } }
        public UsuarioRepository UsuarioRepository { get { return this._userRepository; } }
        public EncuestasController(DbContextOptions<DataBaseContext> options, UserManager<Usuario> userManager, IUnitOfWork unitOfWork)
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
            return View(gruposUsuario.Select(g => new GrupoViewModel { GrupoID = g.GrupoID, Nombre = g.Nombre }));

        }

        [HttpGet("CrearEncuesta")]
        public IActionResult CrearEncuesta(int id)
        {
            var grupo = UnitOfWork.GrupoRepository.GetPeroCompleto(id);
            if (grupo == null)
            {
                return NotFound();
            }
            var grupoVM = new GrupoViewModel { GrupoID = grupo.GrupoID };
            return View(grupoVM);
        }

        [HttpPost("CrearEncuesta")]
        public IActionResult CrearEncuesta(GrupoViewModel gvm)
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

        [HttpGet("VotarEncuesta")]
        public IActionResult VotarEncuesta(int idEncuesta, int idGrupo)
        {
            return View();
        }

        [HttpPost("VotarEncuesta")]
        public IActionResult VotarEncuesta(GrupoViewModel gvm)
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