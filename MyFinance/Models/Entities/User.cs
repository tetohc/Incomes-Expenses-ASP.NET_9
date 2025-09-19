using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace MyFinance.Models.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string Surname { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(255)]
        public string Password { get; set; } = null!;   

        public bool IsActive { get; set; }

        public Collection<Service> Services { get; set; } = null!;
        public Collection<Transaction> Transactions { get; set; } = null!;
    }
}