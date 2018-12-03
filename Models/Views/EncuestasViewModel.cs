using System;
using System.Collections.Generic;

namespace Travlr.Models
{
    public class EncuestasViewModel
    {
        public int EncuestaID { get; set; }
        public int GrupoID { get; set; }
        public string Pregunta { get; set; }
        public int OptionSelected { get; set; }
        public bool Votaste { get; set; }
        public ICollection<Opcion> Opciones { get; set; }
    }
}