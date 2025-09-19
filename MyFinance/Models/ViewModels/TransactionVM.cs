using MyFinance.Helpers;
using System.ComponentModel.DataAnnotations;

namespace MyFinance.Models.ViewModels
{
    public class TransactionVM
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        [Display(Name = "Servicio")]
        [Required(ErrorMessage = "Seleccione un servicio válido.")]
        public Guid ServiceId { get; set; }

        public string Service { get; set; } = null!;

        [Display(Name = "Comentario")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "El comentario es obligatorio.")]
        public string Comment { get; set; } = null!;

        [Display(Name = "Monto")]
        [Required(ErrorMessage = "El monto es obligatorio.")]
        [DataType(DataType.Currency)]
        [Range(0.01, double.MaxValue, ErrorMessage = "Ingrese un monto válido mayor que cero.")]
        public decimal TotalAmount { get; set; }

        [Display(Name = "Fecha")]
        [Required(ErrorMessage = "La fecha es obligatoria.")]
        public DateOnly Date { get; set; }

        public DateTime RegisterDate { get; set; }

        public bool IsActive { get; set; }

        [Display(Name = "Tipo de transacción")]
        public int Type { get; set; }
        public int TypeTransaction { get; set; }
    }
}