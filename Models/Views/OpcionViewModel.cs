using System;
using System.Collections.Generic;

namespace Travlr.Models
{
    public class OpcionViewModel
    {
        public int ID { get; set; }
        public int GrupoID { get; set; }
        public int EncuestaID { get; set; }
        public string Opcion { get; set; }
    }
}