using Microsoft.AspNetCore.Mvc;
using SeminarHub.Models;
using SeminarHub.Services;
using SeminarHub.Services.Contracts;
using static SeminarHub.Services.SeminarServices;

namespace SeminarHub.Controllers
{
    public class SeminarController : BaseController
    {
        private readonly ISeminarService _seminarServices;

        public SeminarController(ISeminarService seminarServices)
        {
            _seminarServices = seminarServices;
        }

        public async Task<IActionResult> All()
        {
            var model = await _seminarServices.GetSeminarAllsAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        { 
            var model = await _seminarServices.GetSeminarAddViewModelAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(SeminarAddViewModel model)
        {
            if (!ModelState.IsValid)
            {                
                return View(model);
            }

            var userId = GetUserId();

            await _seminarServices.AddSeminarAsync(model, userId);
            return RedirectToAction("All");
        }


    }
}
