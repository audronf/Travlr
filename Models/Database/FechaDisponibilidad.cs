using System;

namespace Funtrip.Models
{
    public class FechaDisponibilidad
    {
        public int ID { get; set; }
        public virtual Usuario Usuario { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        
    }
}