using System.ComponentModel.DataAnnotations;

namespace MyFinance.Models.Enums
{
    public enum ServiceType
    {
        [Display(Name = "Ingreso")]
        Income = 1,

        [Display(Name = "Gasto")]
        Expense = 2,
    }
}