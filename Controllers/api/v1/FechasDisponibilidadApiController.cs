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
    public class FechasDisponibilidadApiController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UsuarioRepository _userRepository;
        public IUnitOfWork UnitOfWork { get { return this._unitOfWork; } }
        public UsuarioRepository UsuarioRepository { get { return this._userRepository; } }
        public FechasDisponibilidadApiController(DbContextOptions<DataBaseContext> options, UserManager<Usuario> userManager, IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
            this._userRepository = new UsuarioRepository(options, userManager);
        }

        [HttpPost("FechasDisponibilidad")]
        public IActionResult FechasDisponibilidad(GrupoViewModel gvm)
        {
            var logged = UsuarioRepository.UserManager.FindByNameAsync(User.Identity.Name).Result;
            var fechaDisp = new FechaDisponibilidad { UsuarioId = logged.Id, FechaInicio = gvm.FechaDisponibilidad.FechaInicio, FechaFin = gvm.FechaDisponibilidad.FechaFin };
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

        [HttpGet("ListadoFechas")]
        public IActionResult ListadoFechas(int id)
        {
            var grupo = UnitOfWork.GrupoRepository.GetPeroCompleto(id);
            var fdvm = grupo.FechasDisponibilidad.Select(fd => new FechaDisponibilidadViewModel{ID = fd.ID, FechaInicio = fd.FechaInicio, FechaFin = fd.FechaFin});
            return Json(fdvm);
        }
    }
}