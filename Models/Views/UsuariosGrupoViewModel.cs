using System.Collections.Generic;

namespace Travlr.Models.Views
{
    public class UsuariosGrupoViewModel
    {
        public int GrupoID { get; set; }
        public string NombreGrupo { get; set; }
        public bool isAdmin { get; set; }
        public ICollection<Usuario> Usuarios { get; set; }

    }
}
