using System;
using System.ComponentModel.DataAnnotations;

namespace Travlr.Models
{
    public class Opcion
    {
        public int ID { get; set; }
        public string Texto { get; set; }
        public int Cantidad { get; set; }
    }
}