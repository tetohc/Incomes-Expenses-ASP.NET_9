using Microsoft.EntityFrameworkCore;
using MyFinance.Context;
using MyFinance.Mappers;
using MyFinance.Models.ViewModels;

namespace MyFinance.Managers
{
    /// <summary>
    /// Clase encargada de comunicarse con la base de datos para gestionar los usuarios.
    /// </summary>
    /// <param name="_dbContext"></param>
    public class UserManager(MyFinanceDbContext _dbContext)
    {
        public async Task<bool> Create(UserVM userVM)
        {
            userVM.Id = Guid.NewGuid();
            userVM.IsActive = true;
            var user = userVM.ToEntity();
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                _dbContext.User.Add(user);
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

        public async Task<bool> Delete(Guid id)
        {
            var user = await _dbContext.User.FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
            if (user == null)
                return false;
            user.IsActive = false;
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                _dbContext.User.Update(user);
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

        public async Task<List<UserVM>> GetAll(Guid userId)
        {
            return await _dbContext.User
                .Where(x => x.IsActive && x.Id != userId)
                .Select(x => x.ToViewModel())
                .ToListAsync();
        }

        public async Task<bool> EmailExists(string email)
        {
            return await _dbContext.User.AnyAsync(x => x.Email.Trim() == email.Trim() && x.IsActive);
        }

        public async Task<bool> IsValidUser(LoginVM loginVM)
        {
            return await _dbContext.User.AnyAsync(x => x.Email.Trim() == loginVM.Email.Trim() && x.Password == loginVM.Password && x.IsActive);
        }

        public async Task<UserVM> SignIn(LoginVM loginVM)
        {
            var user = await _dbContext.User.FirstOrDefaultAsync(
                x => x.Email.Trim() == loginVM.Email.Trim()
                && x.Password == loginVM.Password
                && x.IsActive);

            if (user == null)
                return null!;

            return user.ToViewModel();
        }

        public async Task<bool> IsValidPassword(Guid userId, string currentPassword)
        {
            return await _dbContext.User.AnyAsync(x => x.Id == userId && x.Password == currentPassword && x.IsActive);
        }

        public async Task<bool> ChangePassword(Guid userId, string newPassword)
        {
            var user = await _dbContext.User.FirstOrDefaultAsync(x => x.Id == userId && x.IsActive);
            if (user == null)
                return false;

            user.Password = newPassword;
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                _dbContext.User.Update(user);
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
    }
}