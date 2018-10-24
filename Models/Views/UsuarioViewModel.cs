using System.ComponentModel.DataAnnotations;

namespace Travlr.Models.Views
{
    public class UsuarioViewModel
    {
        [Required]
        [EmailAddress]
        [MaxLength(256)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(40)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}