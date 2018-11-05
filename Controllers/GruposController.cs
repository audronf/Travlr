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

        [HttpGet("ManejoFondos")]
        public IActionResult ManejoFondos(int id)
        {
            var grupo = UnitOfWork.GrupoRepository.GetPeroCompleto(id);
            if (grupo == null)
            {
                return NotFound();
            }
            var usuariosGrupo = new GrupoViewModel { GrupoID = grupo.GrupoID, FondoComun = grupo.FondoComun };
            return View(usuariosGrupo);
        }

        [HttpPost("ManejoFondos")]
        public IActionResult ManejoFondos(GrupoViewModel gvm)
        {
            try
            {
                var grupo = UnitOfWork.GrupoRepository.GetPeroCompleto(gvm.GrupoID);
                if (gvm.monto == 0)
                {
                    return Json(new { mensaje = "Formato invalido o monto = 0" });
                }
                else if (gvm.monto > 0)
                {
                    grupo.FondoComun.Monto += gvm.monto;
                    UnitOfWork.FondoComunRepository.Update(grupo.FondoComun);
                    UnitOfWork.Complete();
                    return Json(new { mensaje = "Se agrego $" + gvm.monto + " al fondo comun. El nuevo saldo es de : $" + grupo.FondoComun.Monto });
                }
                else
                {
                    if ((grupo.FondoComun.Monto + gvm.monto) > 0)
                    {
                        grupo.FondoComun.Monto += gvm.monto;
                        UnitOfWork.FondoComunRepository.Update(grupo.FondoComun);
                        UnitOfWork.Complete();
                        return Json(new { mensaje = "Se saco $" + gvm.monto * -1 + " del fondo comun. El nuevo saldo es de : $" + grupo.FondoComun.Monto });
                    }
                    else
                    {
                        return Json(new { mensaje = "El monto de un grupo no puede ser menor a $0" });
                    }
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet("ListaEncuestas")]
        //wip
        public IActionResult ListaEncuestas()
        {
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
            var text = new List<string>();
            text.Add("cuba");
            text.Add("Paris");

            var grupo = UnitOfWork.GrupoRepository.GetPeroCompleto(gvm.GrupoID);
            var encuesta = new Encuesta { Pregunta = gvm.Encuesta.Pregunta};
            var votacion = new List<Votaron>();
            foreach(var usuario in grupo.UsuarioGrupos.Where(u => u.GrupoID == grupo.GrupoID))
            {
                var vt = new Votaron { UsuarioId = usuario.UsuarioId, Voto = false};
                votacion.Add(vt);
            }
            var OpcionesList = new List<Opcion>();
            foreach(var opcion in gvm.Opciones)
            {
                var op = new Opcion { Texto = opcion, Cantidad = 0};
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
            return Json(new { mensaje = "exito!!!!!!!!!!!!" });
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
            var fechaDisp = new FechaDisponibilidad{UsuarioId = logged.Id, FechaInicio = DateTime.Now.AddDays(4),FechaFin = DateTime.Now.AddDays(10)};
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
            // if(encuesta.Votaron == null)
            // {
            //     encuesta.Votaron = new List<Usuario>();
            // }
            // encuesta.Votaron.Add(logged);
            UnitOfWork.EncuestaRepository.Update(encuesta);
            UnitOfWork.Complete();
            return Json("votaste!!!!");
        }
    }
}