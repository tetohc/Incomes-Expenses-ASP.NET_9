using System.ComponentModel.DataAnnotations;

namespace MyFinance.Models.ViewModels
{
    public class UserVM
    {
        public Guid Id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres.")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Solo letras y espacios.")]
        public string Name { get; set; } = null!;

        [Display(Name = "Apellidos")]
        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(50, ErrorMessage = "El apellido no puede exceder los 50 caracteres.")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Solo letras y espacios.")]
        public string Surname { get; set; } = null!;

        [Display(Name = "Correo electrónico")]
        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido.")]
        public string Email { get; set; } = null!;

        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(16, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 16 caracteres.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Display(Name = "Confirmar contraseña")]
        [Required(ErrorMessage = "Confirma tu contraseña.")]
        [Compare("NewPassword", ErrorMessage = "Las contraseñas no coinciden.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = null!;

        [Display(Name = "Estado")]
        public bool IsActive { get; set; }

        [Display(Name = "Nombre")]
        public string FullName { get; set; } = null!;
    }
}