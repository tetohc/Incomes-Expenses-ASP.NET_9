using MyFinance.Helpers;
using MyFinance.Models.Entities;
using MyFinance.Models.Enums;
using MyFinance.Models.ViewModels;

namespace MyFinance.Mappers
{
    public static class ServiceMapper
    {
        /// <summary>
        /// Recibe un ViewModel de servicio y lo convierte a una entidad de servicio.
        /// </summary>
        /// <param name="serviceVM">ViewModel de servicio.</param>
        /// <returns>Entidad de servicio 'Service'.</returns>
        public static Service ToEntity(this ServiceVM serviceVM)
        {
            return new Service
            {
                Id = serviceVM.Id,
                UserId = serviceVM.UserId,
                Name = serviceVM.Name.Trim(),
                Type = (int)serviceVM.Type!,
                IsActive = serviceVM.IsActive
            };
        }

        /// <summary>
        /// Recibe una entidad de servicio y la convierte a un ViewModel de servicio.
        /// </summary>
        /// <param name="service">Entidad de servicio</param>
        /// <returns></returns>
        public static ServiceVM ToViewModel(this Service service)
        {
            return new ServiceVM
            {
                Id = service.Id,
                UserId = service.UserId,
                Name = service.Name.Trim(),
                Type = service.Type,
                TypeDisplay = EnumHelper.GetDisplayName((ServiceType)service.Type!),
                IsActive = service.IsActive
            };
        }

        /// <summary>
        /// Recibe una lista de servicios y los convierte a una lista de ViewModels.
        /// </summary>
        /// <param name="services">Lista de servicios.</param>
        /// <returns>Lista de ViewModel 'ServiceVM'.</returns>
        public static List<ServiceVM> ToViewModelList(this List<Service> services)
        {
            return services.Select(x => new ServiceVM
            {
                Id = x.Id,
                UserId = x.UserId,
                Name = x.Name.Trim(),
                Type = x.Type,
                TypeDisplay = EnumHelper.GetDisplayName((ServiceType)x.Type!),
                IsActive = x.IsActive
            }).ToList();
        }
    }
}