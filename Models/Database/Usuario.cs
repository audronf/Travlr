using System;
using System.Collections.Generic;

namespace Funtrip.Models
{
    public class Usuario
    {
        public int UsuarioID{ get; set; }
        public string Nombre{ get; set; }
        public string Pass { get; set; }
        public string Email { get; set; }
        public virtual ICollection<UsuarioGrupo> UsuarioGrupos { get; set; }
    }
}