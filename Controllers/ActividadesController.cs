using Travlr.Models;
using Travlr.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Travlr.Controllers
{
    [Route("[controller]")]
    [Authorize]

    public class ActividadesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public IUnitOfWork UnitOfWork { get { return this._unitOfWork; } }
        public ActividadesController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("actividad")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("actividad")]
        public IActionResult Create(ActividadViewModel avm)
        {
            if (ModelState.IsValid)
            {
                var actividad = new Actividad { Descripcion = avm.Descripcion, FechaHora = avm.FechaHora};
                UnitOfWork.ActividadRepository.Add(actividad);
                UnitOfWork.Complete();
            }
            return RedirectToAction("Index", "Actividades");
        }    

    }
}