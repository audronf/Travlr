using System;
using System.Linq;
using System.Collections.Generic;

namespace Funtrip.Models
{
    public class Grupo
    {
        public int GrupoID { get; set; }
        public string AdministradorId { get; set; }
        public virtual FondoComun FondoComun { get; set; }
        public virtual ICollection<Actividad> Actividades { get; set; }
        public virtual ICollection<Encuesta> Encuestas { get; set; }
        public virtual ICollection<FechaDisponibilidad> FechasDisponibilidad { get; set; }
        public virtual ICollection<UsuarioGrupo> UsuarioGrupos { get; set; }
        public virtual Usuario Administrador { get; set; }

    }
}