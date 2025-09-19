using Microsoft.EntityFrameworkCore;
using MyFinance.Context;
using MyFinance.Mappers;
using MyFinance.Models.Entities;
using MyFinance.Models.ViewModels;

namespace MyFinance.Managers
{
    /// <summary>
    /// Clase encargada de comunicarse con la base de datos para gestionar los servicios del usuario.
    /// </summary>
    /// <param name="_dbContext"></param>
    public class ServiceManager(MyFinanceDbContext _dbContext)
    {
        #region CRUD

        public async Task<bool> Create(ServiceVM viewModel)
        {
            viewModel.Id = Guid.NewGuid();
            viewModel.IsActive = true;
            Service service = viewModel.ToEntity();

            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                _dbContext.Service.Add(service);
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

        public async Task<bool> Update(ServiceVM viewModel)
        {
            var existingService = _dbContext.Service.FirstOrDefault(x => x.Id == viewModel.Id && x.IsActive);
            if (existingService == null)
                return false;

            existingService.Name = viewModel.Name;
            existingService.Type = (int)viewModel.Type!;

            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                _dbContext.Service.Update(existingService);
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
            var service = _dbContext.Service.FirstOrDefault(x => x.Id == id && x.IsActive);
            if (service == null)
                return false;

            service.IsActive = false;
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                _dbContext.Service.Update(service);
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

        public ServiceVM? GetById(Guid id)
        {
            Service? service = _dbContext.Service.FirstOrDefault(x => x.Id == id && x.IsActive);
            return service?.ToViewModel();
        }

        #endregion CRUD

        #region Listar

        public async Task<List<ServiceVM>> GetAll(Guid userId)
        {
            var services = await _dbContext.Service
                .Where(x => x.UserId == userId && x.IsActive)
                .ToListAsync();
            return services.ToViewModelList();
        }

        public async Task<List<ServiceVM>> GetAllByType(Guid userId, int type)
        {
            var services = await _dbContext.Service
                .Where(x => x.UserId == userId && x.IsActive && x.Type == type)
                .ToListAsync();
            return services.ToViewModelList();
        }

        #endregion Listar
    }
}