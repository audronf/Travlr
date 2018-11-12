using Travlr.Models;
using Travlr.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Repositories;
using Microsoft.EntityFrameworkCore;
using Travlr.Repositories.Database;
using Microsoft.AspNetCore.Identity;
using System;

namespace Travlr.Controllers
{
    [Route("api/v1/[controller]")]
    public class ManejoFondosApiController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UsuarioRepository _userRepository;
        public IUnitOfWork UnitOfWork { get { return this._unitOfWork; } }
        public UsuarioRepository UsuarioRepository { get { return this._userRepository; } }
        public ManejoFondosApiController(DbContextOptions<DataBaseContext> options, UserManager<Usuario> userManager, IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
            this._userRepository = new UsuarioRepository(options, userManager);
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
            return Json(usuariosGrupo);
        }

        [HttpPost("ManejoFondos")]
        public IActionResult ManejoFondos([FromBody]GrupoViewModel gvm)
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
    }
}