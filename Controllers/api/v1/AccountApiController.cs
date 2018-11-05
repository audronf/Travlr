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

namespace Travlr.Controllers
{
    [Route("api/v1/[controller]")]
    public class AccountApiController : Controller
    {
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
            var result = await SignInManager.PasswordSignInAsync(loginViewModel.Nombre, loginViewModel.Password, loginViewModel.RememberMe, false);
            object response;
            if (result.Succeeded)
            {
                var appUser = UserManager.Users.SingleOrDefault(r => r.Nombre == loginViewModel.Nombre);
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
            return Json(await UserManager.FindByNameAsync(User.Identity.Name));
        }
    }
}