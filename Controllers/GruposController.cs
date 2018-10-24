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
            return View();
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

        [HttpGet("Join")]
        public IActionResult Join()
        {
            return View();
        }

        [HttpPost("Join")]
        public IActionResult Join(GrupoViewModel gvm)
        {
            var usuario = _userRepository.UserManager.FindByNameAsync(User.Identity.Name).Result;
            var grupo = UnitOfWork.GrupoRepository.Get(gvm.GrupoID);
            var yaEsMiembo = UnitOfWork.UsuarioGrupoRepository.GetAll().ToList().Any(ug => ug.GrupoID == gvm.GrupoID && ug.UsuarioId == usuario.Id);
            if (!yaEsMiembo)
            {
                var usuarioGrupo = new UsuarioGrupo { UsuarioId = usuario.Id, Grupo = grupo, GrupoID = grupo.GrupoID };
                UnitOfWork.UsuarioGrupoRepository.Add(usuarioGrupo);
                UnitOfWork.Complete();
            }
            return RedirectToAction("Join", "Grupos");
        }
    }
}