using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace Funtrip.Models.Views
{
    public class ActividadViewModel
    {
        [Required]
        public string Descripcion { get; set; }

        [Required]
        public DateTime FechaHora { get; set; }      
        
    }
}