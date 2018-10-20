using System.ComponentModel.DataAnnotations;

namespace Funtrip.Models.Views
{
    public class UsuarioViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}