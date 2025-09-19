using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyFinance.Models.Entities
{
    public class Transaction
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid ServiceId { get; set; }

        [Required]
        [StringLength(100)]
        public string Comment { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        public DateOnly Date { get; set; }

        [Required]
        public DateTime RegisterDate { get; set; }

        public bool IsActive { get; set; }

        public User User { get; set; } = null!;

        public Service Service { get; set; } = null!;
    }
}