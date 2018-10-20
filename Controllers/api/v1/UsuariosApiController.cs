using Funtrip.Models;
using Funtrip.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;

namespace Funtrip.Controllers
{
    [Route("api/v1/[controller]")]
    public class UsuariosApiController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public IUnitOfWork UnitOfWork { get { return this._unitOfWork; } }
        public UsuariosApiController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
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