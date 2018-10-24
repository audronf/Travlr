using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Travlr.Models
{
    public class Usuario : IdentityUser
    {
        public override string Id{ get; set; }
        public string Nombre{ get; set; }
        public string Password { get; set; }
        public override string Email { get; set; }
        public virtual ICollection<Grupo> GruposAdmin { get; set; }
        public virtual ICollection<UsuarioGrupo> UsuarioGrupos { get; set; }
    }
}