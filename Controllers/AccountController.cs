using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Funtrip.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using Funtrip.Models.Views;

namespace MvcMovie.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;

        public AccountController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        public SignInManager<Usuario> SignInManager
        {
            get { return this._signInManager; }
        }

        public UserManager<Usuario> UserManager
        {
            get { return this._userManager; }
        }

        [AllowAnonymous]
        [HttpGet("Register")]
        public IActionResult Register() => View();

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UsuarioViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new Usuario { UserName = userViewModel.Nombre, Email = userViewModel.Email };
                var result = await UserManager.CreateAsync(user, userViewModel.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, true);
                    return RedirectToAction("Index", "Grupos");
                }
                else foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(userViewModel);
        }

        [AllowAnonymous]
        [HttpGet("Login")]
        public async Task<IActionResult> Login()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            var viewModel = new LoginViewModel();
            return View(viewModel);
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await SignInManager.PasswordSignInAsync(loginViewModel.Nombre, loginViewModel.Password, loginViewModel.RememberMe, false);
                if (result.Succeeded) return RedirectToAction("Index", "Grupos");
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(loginViewModel);
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

    }
}