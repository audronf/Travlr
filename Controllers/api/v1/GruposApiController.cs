using Travlr.Models;
using Travlr.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;

// Estos API controller no se usan por ahora pero los dejo ya creados, usamos los otros controller para poder testearlos con vistas simples.
namespace Travlr.Controllers
{
    [Route("api/v1/[controller]")]
    public class GruposApiController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public IUnitOfWork UnitOfWork { get { return this._unitOfWork; } }
        public GruposApiController(IUnitOfWork unitOfWork)
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
                var grupo = new Grupo { };
                UnitOfWork.GrupoRepository.Add(grupo);
                UnitOfWork.Complete();
            }
            return RedirectToAction("Index", "Grupo");
        }
    }
}