using System;
using System.Collections.Generic;

namespace Travlr.Models
{
    public class EncuestasViewModel
    {
        public int ID { get; set; }
        public string Pregunta { get; set; }
        public ICollection<Opcion> Opciones { get; set; }
    }
}