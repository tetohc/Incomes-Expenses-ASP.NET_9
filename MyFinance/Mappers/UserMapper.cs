using MyFinance.Helpers;
using MyFinance.Models.Entities;
using MyFinance.Models.ViewModels;

namespace MyFinance.Mappers
{
    public static class UserMapper
    {
        /// <summary>
        /// Recibe un ViewModel de usuario y lo convierte a una entidad de usuario.
        /// </summary>
        /// <param name="userVM">ViewModel de usuario.</param>
        /// <returns>Entidad `User`</returns>
        public static User ToEntity(this UserVM userVM)
        {
            return new User
            {
                Id = userVM.Id,
                Name = userVM.Name.Trim(),
                Surname = userVM.Surname.Trim(),
                Email = userVM.Email.Trim(),
                Password = EncryptPasswordSha256.Encrypt(userVM.Password.Trim()),
                IsActive = userVM.IsActive
            };
        }

        /// <summary>
        /// Recibe una entidad de usuario y la convierte a un ViewModel de usuario.
        /// </summary>
        /// <param name="user">Entidad de usuario.</param>
        /// <returns>ViewModel `UserVM`</returns>
        public static UserVM ToViewModel(this User user)
        {
            return new UserVM
            {
                Id = user.Id,
                FullName = $"{user.Name.Trim()} {user.Surname.Trim()}",
                Email = user.Email.Trim(),
            };
        }
    }
}