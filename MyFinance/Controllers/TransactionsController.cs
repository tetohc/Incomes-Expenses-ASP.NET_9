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
    public class TransactionsController(
        ServiceManager _serviceManager,
        TransactionManager _transactionManager) : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactions()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var transactions = await _transactionManager.GetAll(userId);
            return Json(new { data = transactions });
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactionsByDate(DateOnly startDate, DateOnly endDate)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var transactions = await _transactionManager.GetAllByDate(userId, startDate, endDate);
            return Json(new { data = transactions });
        }

        public async Task<IActionResult> LoadServices(int type)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var services = await _serviceManager.GetAllByType(userId, type);
            return Json(new { data = services });
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
        public async Task<bool> Create(TransactionVM transactionVM)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            transactionVM.UserId = userId;
            var result = await _transactionManager.Create(transactionVM);
            return result;
        }

        [HttpDelete]
        public async Task<bool> Delete(Guid id)
        {
            var result = await _transactionManager.Delete(id);
            return result;
        }
    }
}