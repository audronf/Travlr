using Funtrip.Models;
using Funtrip.Models.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;

namespace Funtrip.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class GruposController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public IUnitOfWork UnitOfWork { get { return this._unitOfWork; } }
        public GruposController(IUnitOfWork unitOfWork)
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
        public IActionResult Create(GrupoViewModel gvm)
        {
            if (ModelState.IsValid)
            {
                var fondoComun = new FondoComun { Monto = 0};
                var grupo = new Grupo { Administrador = gvm.Administrador, FondoComun = fondoComun};
                UnitOfWork.GrupoRepository.Add(grupo);
                UnitOfWork.Complete();
            }
            return RedirectToAction("Create", "Grupo");
        }
    }
}