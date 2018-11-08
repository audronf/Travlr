using System.Linq;
using Travlr.Models;
using Travlr.Models.Views;
using Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Travlr.Repositories.Database;
using System.Collections.Generic;
using System;

namespace Travlr.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class GruposController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UsuarioRepository _userRepository;
        public IUnitOfWork UnitOfWork { get { return this._unitOfWork; } }
        public UsuarioRepository UsuarioRepository { get { return this._userRepository; } }
        public GruposController(DbContextOptions<DataBaseContext> options, UserManager<Usuario> userManager, IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
            this._userRepository = new UsuarioRepository(options, userManager);
        }
        public IActionResult Index()
        {
            var logged = UsuarioRepository.UserManager.FindByNameAsync(User.Identity.Name).Result;
            var grupos = UnitOfWork.UsuarioGrupoRepository.GetAll().Where(u => u.UsuarioId == logged.Id);
            List<Grupo> gruposUsuario = new List<Grupo>();
            foreach (var grupo in grupos)
            {
                gruposUsuario.Add(UnitOfWork.GrupoRepository.Get(grupo.GrupoID));
            }
            return View(gruposUsuario.Select(g => new GrupoViewModel { GrupoID = g.GrupoID, Nombre = g.Nombre }));
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        public IActionResult Create(GrupoViewModel gvm)
        {
            var admin = _userRepository.UserManager.FindByNameAsync(User.Identity.Name).Result;
            var grupoExiste = UnitOfWork.GrupoRepository.GetAll().Any(g => g.Nombre == gvm.Nombre && g.AdministradorId == admin.Id);
            if (!grupoExiste)
            {
                var fondoComun = new FondoComun { Monto = 0 };
                var grupo = new Grupo { AdministradorId = admin.Id, FondoComun = fondoComun, Nombre = gvm.Nombre };
                var usuarioGrupo = new UsuarioGrupo { UsuarioId = admin.Id, Grupo = grupo };
                UnitOfWork.GrupoRepository.Add(grupo);
                UnitOfWork.UsuarioGrupoRepository.Add(usuarioGrupo);
                UnitOfWork.Complete();
                var codGrupo = UnitOfWork.GrupoRepository.GetAll().Where(g => g.Nombre == gvm.Nombre && g.AdministradorId == admin.Id).FirstOrDefault().GrupoID;
                return Json(new { mensaje = "El grupo " + gvm.Nombre + " fue creado, su codigo es: " + codGrupo });
            }
            else
            {
                return Json(new { mensaje = "Un grupo con el nombre " + gvm.Nombre + " ya existe" });
            }
        }

        [HttpGet("Unirse")]
        public IActionResult Unirse()
        {
            return View();
        }

        [HttpPost("Unirse")]
        public IActionResult Unirse(GrupoViewModel gvm)
        {
            var usuario = UsuarioRepository.UserManager.FindByNameAsync(User.Identity.Name).Result;
            var grupo = UnitOfWork.GrupoRepository.Get(gvm.GrupoID);
            if (grupo == null)
            {
                return Json(new { mensaje = "No existe un grupo con ese código" });
            }
            var admin = UsuarioRepository.UserManager.FindByIdAsync(grupo.AdministradorId).Result;
            var yaEsMiembo = UnitOfWork.UsuarioGrupoRepository.GetAll().ToList().Any(ug => ug.GrupoID == gvm.GrupoID && ug.UsuarioId == usuario.Id);
            if (!yaEsMiembo)
            {
                var usuarioGrupo = new UsuarioGrupo { UsuarioId = usuario.Id, Grupo = grupo, GrupoID = grupo.GrupoID };
                UnitOfWork.UsuarioGrupoRepository.Add(usuarioGrupo);
                UnitOfWork.Complete();
                return Json(new { mensaje = "Te uniste al grupo " + grupo.Nombre + " de " + admin.UserName });
            }
            return Json(new { mensaje = "Ya formas parte de ese grupo" });
        }

        /*Eliminado fisico de grupo */
        [HttpGet("Eliminar")]
        public IActionResult Eliminar(GrupoViewModel gvm)
        {
            var grupo = UnitOfWork.GrupoRepository.GetPeroCompleto(gvm.GrupoID);
            if (grupo == null)
            {
                return Json(new { mensaje = "El id " + gvm.GrupoID + " no existe" });
            }
            UnitOfWork.GrupoRepository.Remove(grupo);
            return Json(new { mensaje = "Se ha eliminado el grupo correctamente" });
        }

        [HttpGet("Detalles")]
        public IActionResult Detalles(int id)
        {
            var grupo = UnitOfWork.GrupoRepository.GetPeroCompleto(id);
            if (grupo == null)
            {
                return NotFound();
            }
            var miembrosGrupo = UnitOfWork.UsuarioGrupoRepository.GetAll().Where(g => g.GrupoID == grupo.GrupoID);

            var miembros = new List<Usuario>();
            foreach (var miembro in miembrosGrupo)
            {
                miembros.Add(UsuarioRepository.UserManager.FindByIdAsync(miembro.UsuarioId).Result);
            }
            var usuariosGrupo = new UsuariosGrupoViewModel { Usuarios = miembros, NombreGrupo = grupo.Nombre };
            return View(usuariosGrupo);
        }

        [HttpGet("DejarGrupo")]
        public IActionResult DejarGrupo(int id)
        {
            var grupo = UnitOfWork.GrupoRepository.Get(id);
            if (grupo == null)
            {
                return NotFound();
            }
            var miembrosGrupo = UnitOfWork.UsuarioGrupoRepository.GetAll().Where(g => g.GrupoID == grupo.GrupoID);
            var miembros = new List<Usuario>();
            foreach (var miembro in miembrosGrupo)
            {
                miembros.Add(UsuarioRepository.UserManager.FindByIdAsync(miembro.UsuarioId).Result);
            }
            var usuariosGrupo = new UsuariosGrupoViewModel { GrupoID = grupo.GrupoID, Usuarios = miembros, NombreGrupo = grupo.Nombre };
            return View(usuariosGrupo);
        }

        [HttpPost("DejarGrupo")]
        public IActionResult DejarGrupoConfirm(UsuariosGrupoViewModel gvm)
        {
            try
            {
                var usuario = UsuarioRepository.UserManager.FindByNameAsync(User.Identity.Name).Result;
                var usuarioGrupo = UnitOfWork.UsuarioGrupoRepository.GetAll().Where(ug => ug.GrupoID == gvm.GrupoID && ug.UsuarioId == usuario.Id).FirstOrDefault();
                UnitOfWork.UsuarioGrupoRepository.RemoveUsuarioGrupo(usuarioGrupo);
                UnitOfWork.Complete();
                return RedirectToAction("Index", "Grupos");
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet("SemanaActividades")]
        public IActionResult SemanaActividades(int id)
        {
            var logged = UsuarioRepository.UserManager.FindByNameAsync(User.Identity.Name).Result;
            var grupo = UnitOfWork.GrupoRepository.GetPeroCompleto(id);
            var actconf = grupo.Actividades.Select(a => new Actividad{ ID = a.ID, FechaHora = a.FechaHora, Descripcion = a.Descripcion}).Where(act => act.Confirmados.All(a => a.Asiste==true && a.UsuarioId == logged.Id));
            var vm = actconf.Select(act => new ActividadViewModel { Descripcion = act.Descripcion, FechaHora = act.FechaHora, Id = act.ID});
            return View(vm);
        }
    }
}