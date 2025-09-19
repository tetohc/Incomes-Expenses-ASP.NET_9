using System.ComponentModel.DataAnnotations;

namespace MyFinance.Models.ViewModels
{
    public class ServiceVM
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres.")]
        public string Name { get; set; } = null!;

        [Display(Name = "Tipo de servicio")]
        [Required(ErrorMessage = "Seleccione un tipo de servicio válido.")]
        public int? Type { get; set; }
        public string TypeDisplay { get; set; } = null!;

        public bool IsActive { get; set; }
    }
}
