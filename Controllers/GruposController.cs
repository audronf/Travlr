using System.Linq;
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
            
            var fondoComun = new FondoComun { Monto = 0 };
            var admin = _userRepository.UserManager.FindByNameAsync(User.Identity.Name).Result;
            var grupo = new Grupo {AdministradorId=admin.Id, FondoComun = fondoComun};
            var usuarioGrupo = new UsuarioGrupo { UsuarioId = admin.Id, Grupo = grupo };
            UnitOfWork.GrupoRepository.Add(grupo);
            UnitOfWork.UsuarioGrupoRepository.Add(usuarioGrupo); 
            UnitOfWork.Complete();
            return RedirectToAction("Create", "Grupos");
        }
        
        // [HttpGet("JoinGroup")]
        // public IActionResult JoinGroup()
        // {
        //     return View();
        // }
        
        // [HttpPost("JoinGroup")]
        // public IActionResult JoinGroup(int coda)
        // {
        //     //Harcodeado porque no puedo hacer que llegue el cod de la vista
        //     int cod = 1;
        //     var usuario = _userRepository.UserManager.FindByNameAsync(User.Identity.Name);
        //     var grupo = UnitOfWork.GrupoRepository.Get(cod);
        //     var yaEsMiembo = UnitOfWork.UsuarioGrupoRepository.GetAll().ToList().Any(ug => ug.GrupoID==cod && ug.Id==usuario.Result.Id);
        //     if(!yaEsMiembo)
        //     {
        //         var usuarioGrupo = new UsuarioGrupo { Usuario = usuario.Result, Grupo = grupo, GrupoID = grupo.GrupoID };
        //         UnitOfWork.UsuarioGrupoRepository.Add(usuarioGrupo);
        //         UnitOfWork.Complete();
        //     }
        //     return RedirectToAction("","");
        // }
    }
}