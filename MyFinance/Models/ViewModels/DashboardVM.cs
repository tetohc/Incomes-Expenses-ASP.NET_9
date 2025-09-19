using System.ComponentModel.DataAnnotations;

namespace MyFinance.Models.ViewModels
{
    public class DashboardViewModel
    {
        [Display(Name = "Total Income")]
        public decimal TotalIncome { get; set; }

        [Display(Name = "Total Expenses")]
        public decimal TotalExpenses { get; set; }

        [Display(Name = "Balance")]
        public decimal Balance => TotalIncome - TotalExpenses;

        public List<IncomeCategoryViewModel> IncomeByCategory { get; set; } = new();

        public List<TransactionViewModel> RecentTransactions { get; set; } = new();
    }

    public class IncomeCategoryViewModel
    {
        [Required]
        public string CategoryName { get; set; } = null!;

        [Range(0, 100)]
        public decimal Percentage { get; set; }
    }

    public class TransactionViewModel
    {
        [Required]
        public string Description { get; set; } = null!;

        [Display(Name = "Amount")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Display(Name = "Date")]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [Display(Name = "Type")]
        public int Type { get; set; }
    }
}
