using System.Collections.Generic;

namespace Travlr.Models.Views
{
    public class GrupoViewModel
    {
        public int GrupoID { get; set; }
        public string Nombre { get; set; }
        public virtual FondoComun FondoComun { get; set; }
        public float monto { get; set; }
        public virtual ICollection<Actividad> Actividades { get; set; }
        public virtual ICollection<Encuesta> Encuestas { get; set; }
        public virtual ICollection<FechaDisponibilidad> FechasDisponibilidad { get; set; }
        public virtual ICollection<UsuarioGrupo> UsuarioGrupos { get; set; }
        public Usuario Administrador { get; set; }

    }
}
