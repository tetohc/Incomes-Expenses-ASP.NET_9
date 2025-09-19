using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyFinance.Helpers;
using MyFinance.Managers;
using MyFinance.Models.Enums;
using MyFinance.Models.ViewModels;
using System.Security.Claims;

namespace MyFinance.Controllers
{
    [Authorize]
    public class ServicesController(ServiceManager _serviceManager) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var services = await _serviceManager.GetAll(userId);
            return View(services);
        }

        [HttpGet]
        public async Task<IActionResult> GetServices()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var services = await _serviceManager.GetAll(userId);
            return Json(new { data = services });
        }

        [HttpGet]
        public async Task<IActionResult> GetServicesByType(int type)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var services = await _serviceManager.GetAllByType(userId, type);
            return Ok(services);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var serviceTypes = EnumHelper.GetEnumDisplayList<ServiceType>();
            ViewBag.Type = new SelectList(serviceTypes?.OrderBy(x => x.Name), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<bool> Create(ServiceVM serviceVM)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            serviceVM.UserId = userId;
            var result = await _serviceManager.Create(serviceVM);
            return result;
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var service = _serviceManager.GetById(id);
            if (service == null)
            {
                return NotFound();
            }

            var serviceTypes = EnumHelper.GetEnumDisplayList<ServiceType>();
            ViewBag.Type = new SelectList(serviceTypes?.OrderBy(x => x.Name), "Id", "Name", service.Type);
            return View(service);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<bool> Edit(ServiceVM serviceVM)
        {
            var result = await _serviceManager.Update(serviceVM);
            return result;
        }

        [HttpDelete]
        public async Task<bool> Delete(Guid id)
        {
            var result = await _serviceManager.Delete(id);
            return result;
        }
    }
}