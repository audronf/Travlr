using System;
using System.ComponentModel.DataAnnotations;

namespace Travlr.Models
{
    public class FechaDisponibilidadViewModel
    {
        public int ID { get; set; }
        public virtual Usuario Usuario { get; set; }

        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }

        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        public DateTime FechaFin { get; set; }
    }
}