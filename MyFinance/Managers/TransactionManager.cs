using Microsoft.EntityFrameworkCore;
using MyFinance.Context;
using MyFinance.Mappers;
using MyFinance.Models.Entities;
using MyFinance.Models.Enums;
using MyFinance.Models.ViewModels;

namespace MyFinance.Managers
{
    /// <summary>
    /// Clase encargada de comunicarse con la base de datos para gestionar las transacciones del usuario.
    /// </summary>
    /// <param name="_dbContext"></param>
    public class TransactionManager(MyFinanceDbContext _dbContext)
    {
        #region CRUD

        public async Task<bool> Create(TransactionVM transactionVM)
        {
            transactionVM.Id = Guid.NewGuid();
            transactionVM.IsActive = true;
            Transaction transaction = transactionVM.ToEntity();

            using var transactionDb = _dbContext.Database.BeginTransaction();
            try
            {
                _dbContext.Transaction.Add(transaction);
                await _dbContext.SaveChangesAsync();
                await transactionDb.CommitAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                transactionDb.Rollback();
                return false;
            }
        }

        public async Task<bool> Delete(Guid id)
        {
            var service = _dbContext.Transaction.FirstOrDefault(x => x.Id == id && x.IsActive);
            if (service == null)
                return false;

            service.IsActive = false;
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                _dbContext.Transaction.Update(service);
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                transaction.Rollback();
                return false;
            }
        }

        #endregion CRUD

        #region Listar

        public async Task<List<TransactionVM>> GetAll(Guid userId)
        {
            var transactions = await _dbContext.Transaction
                .Include(x => x.Service)
                .Where(x => x.UserId == userId && x.IsActive && x.Service.IsActive)
                .OrderByDescending(t => t.Date)
                .ToListAsync();
            return transactions.Select(t => t.ToViewModel())
                .OrderByDescending(x => x.Date)
                .ToList();
        }

        public async Task<List<TransactionVM>> GetAllByDate(Guid userId, DateOnly startDate, DateOnly? endDate = null)
        {
            endDate = endDate == DateOnly.FromDateTime(DateTime.Parse("01/01/0001")) ? DateOnly.FromDateTime(DateTime.Now) : endDate;
            var transactions = await _dbContext.Transaction
                .Include(x => x.Service)
                .Where(x =>
                    x.UserId == userId
                    && x.IsActive
                    && x.Date >= startDate
                    && x.Date <= endDate
                )
                .OrderByDescending(x => x.Date)
                .ToListAsync();
            return transactions.Select(x => x.ToViewModel())
                .OrderByDescending(x => x.Date)
                .ToList();
        }

        #endregion Listar

        #region Dashboard

        public async Task<DashboardViewModel> GetDataForDashboard(Guid userId)
        {
            var transactions = await _dbContext.Transaction
                .Include(x => x.Service)
                .Where(x => x.UserId == userId && x.IsActive && x.Service.IsActive)
                .ToListAsync();
            var totalIncome = transactions
                .Where(t => t.Service.Type == (int)ServiceType.Income) 
                .Sum(t => t.TotalAmount);
            var totalExpenses = transactions
                .Where(t => t.Service.Type == (int)ServiceType.Expense) 
                .Sum(t => t.TotalAmount);
            var incomeByCategory = transactions
                .Where(t => t.Service.Type == (int)ServiceType.Income)
                .GroupBy(t => t.Service.Name)
                .Select(g => new IncomeCategoryViewModel
                {
                    CategoryName = g.Key,
                    Percentage = totalIncome == 0 ? 0 : (g.Sum(t => t.TotalAmount) / totalIncome) * 100
                })
                .ToList();
            var recentTransactions = transactions
                .OrderByDescending(t => t.Date)
                .Take(5)
                .Select(t => new TransactionViewModel
                {
                    Description = t.Comment,
                    Amount = t.TotalAmount,
                    Date = t.Date.ToDateTime(new TimeOnly(0, 0)),
                    Type = t.Service.Type
                })
                .ToList();
            return new DashboardViewModel
            {
                TotalIncome = totalIncome,
                TotalExpenses = totalExpenses,
                IncomeByCategory = incomeByCategory,
                RecentTransactions = recentTransactions
            };
        }

        #endregion Dashboard
    }
}