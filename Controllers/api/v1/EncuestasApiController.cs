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
        public IActionResult ListaEncuestas(int id)
        {
            var grupo = UnitOfWork.GrupoRepository.GetPeroCompleto(id);
            var avm = new List<EncuestasViewModel>();
            var opciones = new List<Opcion>();
            foreach (var encuesta in grupo.Encuestas)
            {
                var ec = UnitOfWork.EncuestaRepository.GetPeroCompleto(encuesta.ID);
                var e = new EncuestasViewModel { EncuestaID = encuesta.ID, Pregunta = encuesta.Pregunta, Opciones = ec.Opciones, Votaste = ec.Votaron.Where(g => g.ID == encuesta.ID).FirstOrDefault().Voto };
                avm.Add(e);
            }
            return Json(avm);
        }

        [HttpPost("CrearEncuesta")]
        public IActionResult CrearEncuesta([FromBody]EncuestasViewModel evm)
        {
            var grupo = UnitOfWork.GrupoRepository.GetPeroCompleto(evm.GrupoID);
            var encuesta = new Encuesta { Pregunta = evm.Pregunta };
            var votacion = new List<Votaron>();
            foreach (var usuario in UnitOfWork.UsuarioGrupoRepository.GetAll().Where(g => g.GrupoID == grupo.GrupoID))
            {
                var vt = new Votaron { UsuarioId = usuario.UsuarioId, Voto = false };
                votacion.Add(vt);
            }
            var OpcionesList = new List<Opcion>();
            foreach (var opcion in evm.Opciones)
            {
                var op = new Opcion { Texto = opcion.Texto, Cantidad = 0 };
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
        public IActionResult VotarEncuesta([FromBody]EncuestasViewModel evm)
        {
            var logged = UsuarioRepository.UserManager.FindByNameAsync(User.Identity.Name).Result;
            var encuesta = UnitOfWork.EncuestaRepository.GetPeroCompleto(evm.EncuestaID);
            if (encuesta.Votaron.Where(u => u.UsuarioId == logged.Id).FirstOrDefault().Voto == false)
            {
                encuesta.Votaron.Where(u => u.UsuarioId == logged.Id).FirstOrDefault().Voto = true;
                encuesta.Opciones.Where(o => o.ID == evm.OptionSelected).FirstOrDefault().Cantidad++;
                UnitOfWork.EncuestaRepository.Update(encuesta);
                UnitOfWork.Complete();
            }
            return Json("votaste");
        }

        [HttpPost("AgregarOpcionEncuesta")]
        public IActionResult AgregarOpcion([FromBody]OpcionViewModel ovm)
        {
            var logged = UsuarioRepository.UserManager.FindByNameAsync(User.Identity.Name).Result;
            var encuesta = UnitOfWork.EncuestaRepository.GetPeroCompleto(ovm.EncuestaID);
            if (!encuesta.Opciones.Where(u => u.Texto.ToLower() == ovm.Opcion.ToLower()).Any())
            {
                var opcion = new Opcion { Texto = ovm.Opcion, Cantidad = 0 };
                encuesta.Opciones.Add(opcion);
                UnitOfWork.EncuestaRepository.Update(encuesta);
                UnitOfWork.Complete();
            }
            return Json("opcion agregada");
        }

    }
}