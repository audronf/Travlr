using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using Travlr.Models;
using Microsoft.IdentityModel.Tokens;

namespace AssignmentsNetcore.Helpers
{
    public static class AccountHelper
    {
        public static string GenerateJwtToken(string email, Usuario user, Dictionary<string, string> configVariables)
        {
            var claims = new ClaimsIdentity(new GenericIdentity(user.Email, "Token"), new[] { new Claim("ID", user.Id.ToString()) });
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configVariables["key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(configVariables["expire"]));
            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = configVariables["issuer"],
                Audience = configVariables["issuer"],
                SigningCredentials = creds,
                Subject = claims,
                Expires = expires,
            });
            return handler.WriteToken(securityToken);
        }
    }
}