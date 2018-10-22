using Funtrip.Models;
using Funtrip.Models.Views;
using Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Funtrip.Repositories.Database;

namespace Funtrip.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class GruposController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UsuarioRepository _userRepository;
        public IUnitOfWork UnitOfWork { get { return this._unitOfWork; } }
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
            if (ModelState.IsValid)
            {
                var fondoComun = new FondoComun { Monto = 0};
                var admin = _userRepository.UserManager.FindByNameAsync(User.Identity.Name);
                var grupo = new Grupo { Administrador = admin.Result, FondoComun = fondoComun};
                var usuarioGrupo = new UsuarioGrupo { Usuario = admin.Result, Grupo = grupo };
                UnitOfWork.UsuarioGrupoRepository.Add(usuarioGrupo);
                UnitOfWork.GrupoRepository.Add(grupo);
                UnitOfWork.Complete();
            }
            return RedirectToAction("Create", "Grupo");
        }
        
        [HttpGet("JoinGroup")]
        public IActionResult JoinGroup()
        {
            return View();
        }
        
        [HttpPost("JoinGroup")]
        public IActionResult JoinGroup(int cod)
        {
            if(ModelState.IsValid )
            {
                var grupo = UnitOfWork.GrupoRepository.Get(cod);
                var usuario = _userRepository.UserManager.FindByNameAsync(User.Identity.Name);
                var usuarioGrupo = new UsuarioGrupo { Usuario = usuario.Result, Grupo = grupo };
            }
            return RedirectToAction("JoinGroup","Grupo");
        }
    }
}