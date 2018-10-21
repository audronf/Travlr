using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Funtrip.Models.Views
{
    public class GrupoViewModel
    {
        public int GrupoID { get; set; }
        public virtual FondoComun FondoComun { get; set; }
        public virtual ICollection<Actividad> Actividades { get; set; }
        public virtual ICollection<Encuesta> Encuestas { get; set; }
        public virtual ICollection<FechaDisponibilidad> FechasDisponibilidad { get; set; }
        public virtual ICollection<UsuarioGrupo> UsuarioGrupos { get; set; }
        public Usuario Administrador { get; set; }

    }
}
