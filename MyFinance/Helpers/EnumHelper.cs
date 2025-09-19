using MyFinance.Models.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MyFinance.Helpers
{
    /// <summary>
    /// Clase para ayudar a mostrar los nombres de los enums en la UI.
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// Este método obtiene el nombre de visualización de un enum utilizando el atributo Display.
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType().GetMember(enumValue.ToString()).First().GetCustomAttribute<DisplayAttribute>()?.GetName()!;
        }

        /// <summary>
        /// Este método obtiene una lista de elementos de un enum con sus respectivos nombres de visualización.
        /// </summary>
        /// <typeparam name="TEnum">Tipo de enum.</typeparam>
        /// <returns></returns>
        public static List<EnumItemVM> GetEnumDisplayList<TEnum>() where TEnum : Enum
        {
            return Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Select(e => new EnumItemVM
                {
                    Id = Convert.ToInt32(e),
                    Name = e.GetDisplayName()
                })
                .ToList();
        }
    }
}