namespace MyFinance.Models.Entities
{
    public class Service
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; } = null!;
        public int Type { get; set; }

        public bool IsActive { get; set; }

        public User User { get; set; } = null!;
    }
}