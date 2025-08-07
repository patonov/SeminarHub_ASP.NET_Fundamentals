using Microsoft.AspNetCore.Mvc;
using SeminarHub.Services;
using SeminarHub.Services.Contracts;
using static SeminarHub.Services.SeminarServices;

namespace SeminarHub.Controllers
{
    public class SeminarController : Controller
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
    }
}
