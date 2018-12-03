using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace Travlr.Models.Views
{
    public class ActividadViewModel
    {
        public int GrupoID { get; set; }
        public int ActividadID { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaHora { get; set; }

    }
}