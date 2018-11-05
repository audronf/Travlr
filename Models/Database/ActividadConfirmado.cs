using System;
using System.Collections.Generic;

namespace Travlr.Models
{
    public class ActividadConfirmado
    {
        public int ID { get; set; }
        public string UsuarioId { get; set; }
        public bool Asiste { get; set; }
    }
}