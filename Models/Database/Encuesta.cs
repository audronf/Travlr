using System;
using System.Collections.Generic;

namespace Travlr.Models
{
    public class Encuesta
    {
        public int ID { get; set; }
        public string Pregunta { get; set; }
        public virtual ICollection<Opcion> Opciones { get; set; }
        public virtual ICollection<Votaron> Votaron { get; set; }
    }
}