using Funtrip.Models;
using Funtrip.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;

namespace Funtrip.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public IUnitOfWork UnitOfWork { get { return this._unitOfWork; } }
        public UsuariosController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
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
        public IActionResult Create(UsuarioViewModel uvm)
        {
            if (ModelState.IsValid)
            {
                var user = new Usuario { Email = uvm.Email, Nombre = uvm.Nombre, Pass = uvm.Password};
                UnitOfWork.UsuarioRepository.Add(user);
                UnitOfWork.Complete();
            }
            return RedirectToAction("Index", "Usuarios");
        }
    }
}