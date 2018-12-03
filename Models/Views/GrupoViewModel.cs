using System.Collections.Generic;

namespace Travlr.Models.Views
{
    public class GrupoViewModel
    {
        public int GrupoID { get; set; }
        public string Nombre { get; set; }
        public FondoComun FondoComun { get; set; }
        public Usuario Administrador { get; set; }
        public float monto { get; set; }
        public Encuesta Encuesta { get; set; }
        public Actividad Actividad { get; set; }
        public FechaDisponibilidad FechaDisponibilidad { get; set; }
    }
}
