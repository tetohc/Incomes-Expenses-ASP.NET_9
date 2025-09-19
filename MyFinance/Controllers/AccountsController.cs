using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Helpers;
using MyFinance.Managers;
using MyFinance.Models.ViewModels;
using System.Security.Claims;

namespace MyFinance.Controllers
{
    [Authorize]
    public class AccountsController(UserManager _userManager) : Controller
    {
        #region Session

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn(LoginVM loginVM)
        {
            try
            {
                var isEmailExists = await _userManager.EmailExists(loginVM.Email);
                if (!isEmailExists)
                    return Json(new { result = false, message = "El correo electrónico no está registrado." });

                loginVM.Password = Helpers.EncryptPasswordSha256.Encrypt(loginVM.Password);
                var isValidUser = await _userManager.IsValidUser(loginVM);
                if (!isValidUser)
                    return Json(new { result = false, message = "El correo electrónico o la contraseña es incorrecta." });

                var user = await _userManager.SignIn(loginVM);
                List<Claim> claims = new()
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim(ClaimTypes.Email, user.Email),
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties() { AllowRefresh = true });

                return Json(new { result = true, message = "Inicio de sesión exitoso." });
            }
            catch (Exception)
            {
                return Json(new { result = false, message = "Ocurrió un error inesperado. Inténtelo de nuevo." });
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Accounts");
        }

        #endregion Session

        #region Users

        [HttpGet]
        public IActionResult Users()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var users = await _userManager.GetAll(userId);
            return Json(new { data = users });
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserVM userVM)
        {
            try
            {
                var isEmailExists = await _userManager.EmailExists(userVM.Email);
                if (isEmailExists)
                    return Json(new { result = false, message = "El correo electrónico ya está registrado." });

                var isCreated = await _userManager.Create(userVM);
                if (!isCreated)
                    return Json(new { result = false, message = "No se pudo registrar el usuario. Inténtelo de nuevo." });

                return Json(new { result = true, message = "Usuario registrado exitosamente." });
            }
            catch (Exception)
            {
                return Json(new { result = false, message = "Ocurrió un error inesperado. Inténtelo de nuevo." });
            }
        }

        [HttpDelete]
        public async Task<bool> Delete(Guid id)
        {
            var result = await _userManager.Delete(id);
            return result;
        }

        [HttpGet]
        public IActionResult Profile()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM changePasswordVM)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
                var currentPasswordEncrypted = EncryptPasswordSha256.Encrypt(changePasswordVM.CurrentPassword);

                bool isValidPassword = await _userManager.IsValidPassword(userId, currentPasswordEncrypted);
                if (!isValidPassword)
                    return Json(new { result = false, message = "La contraseña actual es incorrecta. Inténtelo de nuevo." });

                var isChanged = await _userManager.ChangePassword(userId, currentPasswordEncrypted);
                if (!isChanged)
                    return Json(new { result = false, message = "No se pudo cambiar la contraseña. Inténtelo de nuevo." });

                return Json(new { result = true, message = "Contraseña cambiada exitosamente." });
            }
            catch (Exception)
            {
                return Json(new { result = false, message = "Ocurrió un error inesperado. Inténtelo de nuevo." });
            }
        }

        #endregion Users
    }
}