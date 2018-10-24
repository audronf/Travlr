using System;
using System.Collections.Generic;
using System.Linq;

namespace Travlr.Models
{
    public class UsuarioGrupo
    {
        public string UsuarioId{ get; set; }
        public virtual Usuario Usuario{ get; set; }
        public int GrupoID { get; set; }
        public virtual Grupo Grupo { get; set; }
    }
}