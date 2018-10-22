using System;
using System.Collections.Generic;
using System.Linq;

namespace Funtrip.Models
{
    public class UsuarioGrupo
    {
        public string Id{ get; set; }
        public Usuario Usuario{ get; set; }
        public int GrupoID { get; set; }
        public Grupo Grupo { get; set; }
    }
}