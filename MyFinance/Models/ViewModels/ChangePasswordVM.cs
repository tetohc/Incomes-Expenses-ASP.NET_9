using System.ComponentModel.DataAnnotations;

namespace MyFinance.Models.ViewModels
{
    public class ChangePasswordVM
    {
        [Display(Name = "Contraseña actual")]
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(16, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 16 caracteres.")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; } = null!;

        [Display(Name = "Nueva contraseña")]
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(16, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 16 caracteres.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = null!;

        [Display(Name = "Confirmar nueva contraseña")]
        [Required(ErrorMessage = "Confirma tu contraseña.")]
        [Compare("NewPassword", ErrorMessage = "Las contraseñas no coinciden.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = null!;
    }
}
