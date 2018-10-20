using System;
using System.Collections.Generic;

namespace Funtrip.Models
{
    public class Actividad
    {
        public int ID { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaHora { get; set; }
        public virtual ICollection<Usuario> Confirmados { get; set; }
    }
}