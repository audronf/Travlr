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

namespace Travlr.Controllers
{
    [Route("/Grupos/[controller]")]
    [Authorize]

    public class FechasDisponibilidadController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UsuarioRepository _userRepository;
        public IUnitOfWork UnitOfWork { get { return this._unitOfWork; } }
        public UsuarioRepository UsuarioRepository { get { return this._userRepository; } }
        public FechasDisponibilidadController(DbContextOptions<DataBaseContext> options, UserManager<Usuario> userManager, IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
            this._userRepository = new UsuarioRepository(options, userManager);
        }

        [HttpGet("FechasDisponibilidad")]
        public IActionResult FechasDisponibilidad(int id)
        {
            var grupo = UnitOfWork.GrupoRepository.GetPeroCompleto(id);
            if (grupo == null)
            {
                return NotFound();
            }
            var grupoVM = new GrupoViewModel { GrupoID = grupo.GrupoID };
            return View(grupoVM);
        }

        [HttpPost("FechasDisponibilidad")]
        public IActionResult FechasDisponibilidad(GrupoViewModel gvm)
        {
            var logged = UsuarioRepository.UserManager.FindByNameAsync(User.Identity.Name).Result;
            var fechaDisp = new FechaDisponibilidad { UsuarioId = logged.Id, FechaInicio = DateTime.Now.AddDays(4), FechaFin = DateTime.Now.AddDays(10) };
            var grupo = UnitOfWork.GrupoRepository.GetPeroCompleto(gvm.GrupoID);
            if (grupo.FechasDisponibilidad == null)
            {
                grupo.FechasDisponibilidad = new List<FechaDisponibilidad>();
            }
            grupo.FechasDisponibilidad.Add(fechaDisp);
            UnitOfWork.GrupoRepository.Update(grupo);
            UnitOfWork.Complete();
            return Json("esto funciona");
        }
    }
}