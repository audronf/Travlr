using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Travlr.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Travlr.Models.Views;
using Microsoft.Extensions.Configuration;
using Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AssignmentsNetcore.Helpers;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Travlr.Controllers
{
    [Route("api/v1/[controller]")]
    public class AccountApiController : Controller
    {
        private const string AuthSchemes =
        CookieAuthenticationDefaults.AuthenticationScheme + "," +
        JwtBearerDefaults.AuthenticationScheme;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly UserManager<Usuario> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        public AccountApiController(UserManager<Usuario> userManager,
                                    SignInManager<Usuario> signInManager,
                                    IUnitOfWork unitOfWork,
                                    IConfiguration configuration)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._configuration = configuration;
            this._unitOfWork = unitOfWork;
        }

        public UserManager<Usuario> UserManager { get => this._userManager; }
        public SignInManager<Usuario> SignInManager { get => this._signInManager; }
        public IConfiguration Configuration { get => this._configuration; }
        public IUnitOfWork UnitOfWork { get => this._unitOfWork; }

        [HttpPost("SignIn")]
        public async Task<object> SignIn([FromBody] LoginViewModel loginViewModel)
        {
            var userName = UserManager.FindByEmailAsync(loginViewModel.Email).Result;
            var result = await SignInManager.PasswordSignInAsync(userName.UserName, loginViewModel.Password, loginViewModel.RememberMe, false);
            object response;
            if (result.Succeeded)
            {
                var appUser = UserManager.Users.SingleOrDefault(r => r.Email == loginViewModel.Email);
                Response.StatusCode = StatusCodes.Status200OK;
                var configVariables = new Dictionary<string, string>
                {
                    { "key", Configuration["Jwt:Key"] },
                    { "expire", Configuration["Jwt:ExpireDays"] },
                    { "issuer", Configuration["Jwt:Issuer"] },
                };
                response = new { Token = AccountHelper.GenerateJwtToken(loginViewModel.Email, appUser, configVariables) };
            }
            else if (result.IsNotAllowed)
            {
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                response = new { Message = "Error, no autorizado" };
            }
            else
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                if (string.IsNullOrEmpty(loginViewModel.Email) || string.IsNullOrEmpty(loginViewModel.Password))
                    response = new { Message = "Faltan datos" };
                else
                    response = new { Message = "El registro fallo" };
            }
            return Json(response);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("Me")]
        public async Task<IActionResult> Me()
        {
            return Json(await UserManager.FindByEmailAsync(User.Identity.Name));
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]UsuarioViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new Usuario { UserName = userViewModel.Nombre, Email = userViewModel.Email, NickName = userViewModel.NickName };
                var result = await UserManager.CreateAsync(user, userViewModel.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, true);
                    var appUser = UserManager.Users.SingleOrDefault(r => r.Email == user.Email);
                    Response.StatusCode = StatusCodes.Status200OK;
                    var configVariables = new Dictionary<string, string>
                    {
                        { "key", Configuration["Jwt:Key"] },
                        { "expire", Configuration["Jwt:ExpireDays"] },
                        { "issuer", Configuration["Jwt:Issuer"] },
                    };
                    return Json(new { Token = AccountHelper.GenerateJwtToken(user.Email, appUser, configVariables) });
                }
                else foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
            }
            return Json(userViewModel);
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();
            return Json("Logged out");
        }
    }
}