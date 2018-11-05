using Travlr.Models;
using Travlr.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System;

namespace Travlr.Controllers
{
    [Route("/Grupos/[controller]")]
    [Authorize]

    public class ActividadesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public IUnitOfWork UnitOfWork { get { return this._unitOfWork; } }
        public ActividadesController(IUnitOfWork unitOfWork)
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
            var actividad = new Actividad { Descripcion = gvm.Actividad.Descripcion, FechaHora = DateTime.Today };
            var grupo = UnitOfWork.GrupoRepository.GetPeroCompleto(gvm.GrupoID);
            if (grupo.Actividades == null)
            {
                grupo.Actividades = new List<Actividad>();
            }
            grupo.Actividades.Add(actividad);
            UnitOfWork.GrupoRepository.Update(grupo);
            UnitOfWork.Complete();
            return Json("esto funciona");
        }

    }
}