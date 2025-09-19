using System.ComponentModel.DataAnnotations;

namespace MyFinance.Models.ViewModels
{
    public class LoginVM
    {
        [Display(Name = "Correo electrónico")]
        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,16}$", ErrorMessage = "La contraseña debe tener letras y números.")]
        [StringLength(16, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}