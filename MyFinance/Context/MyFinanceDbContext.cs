using Microsoft.EntityFrameworkCore;
using MyFinance.Models.Entities;

namespace MyFinance.Context
{
    public class MyFinanceDbContext : DbContext
    {
        public MyFinanceDbContext(DbContextOptions<MyFinanceDbContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Service> Service { get; set; }
        public DbSet<Transaction> Transaction { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(e =>
            {
                e.HasKey(e => e.Id);
            });

            modelBuilder.Entity<Service>(e =>
            {
                e.HasKey(e => e.Id);
                e.HasOne(s => s.User)
                    .WithMany(u => u.Services)
                    .HasForeignKey(s => s.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Transaction>(e =>
            {
                e.HasKey(e => e.Id);
                e.Property(e => e.TotalAmount)
                    .HasColumnType("decimal(18,2)");

                e.HasOne(t => t.User)
                    .WithMany(u => u.Transactions)
                    .HasForeignKey(t => t.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(t => t.Service)
                    .WithMany()
                    .HasForeignKey(t => t.ServiceId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}