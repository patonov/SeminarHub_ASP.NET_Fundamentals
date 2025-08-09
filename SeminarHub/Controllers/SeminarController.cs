using Microsoft.AspNetCore.Mvc;
using SeminarHub.Data.Models;
using SeminarHub.Models;
using SeminarHub.Services;
using SeminarHub.Services.Contracts;
using System.Security.Claims;
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

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            SeminarAddViewModel? model = await _seminarServices.GetSeminarViewModelToEditAsync(id);

            if (model == null)
            {
                return RedirectToAction("All", "Seminar");
            }

            string? userId = GetUserId();

            if (model.OrganizerId != userId)
            {
                return RedirectToAction("All", "Seminar");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, SeminarAddViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Seminar seminar = await _seminarServices.GetSeminarAsync(id);

            if (seminar == null)
            {
                return RedirectToAction("All", "Seminar");
            }

            var userId = GetUserId();

            if (model.OrganizerId != userId)
            {
                return RedirectToAction("All", "Seminar");
            }

            await _seminarServices.EditSeminarAsync(model, seminar);
            return RedirectToAction("All", "Seminar");
        }

        public async Task<IActionResult> Details(int id)
        { 
            SeminarDetailsViewModel model = await _seminarServices.GetSeminarDetailsAsync(id);

            if (model == null)
            {
                return RedirectToAction("All", "Seminar");
            }
            return View(model);
        }

        public async Task<IActionResult> Joined()
        {
            string userId = GetUserId();

            var model = await _seminarServices.GetAllSeminarsJoinedOfUserAsync(userId);

            return View(model);
        }

        public async Task<IActionResult> Join(int id)
        {
            string userId = GetUserId();

            SeminarJoinedViewModel? seminar = await _seminarServices.GetForJoiningSeminarByIdAsync(id);

            if (seminar == null)
            {
                return RedirectToAction("All", "Seminar");
            }

            await _seminarServices.AddSeminarToJoinedAsync(userId, seminar);
            return RedirectToAction("Joined", "Seminar");
        }

        public async Task<IActionResult> Leave(int id)
        { 
            string userId = GetUserId();

            SeminarJoinedViewModel? seminar = await _seminarServices.GetForJoiningSeminarByIdAsync(id);

            if (seminar == null)
            {
                return RedirectToAction("All", "Seminar");
            }

            await _seminarServices.LeaveSeminar(userId, seminar);
            return RedirectToAction("Joined", "Seminar");
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            string userId = GetUserId();

            Seminar seminar = await _seminarServices.FindSeminarToDeleteById(id);

            if (seminar == null || seminar.OrganizerId != userId)
            {
                return RedirectToAction("All", "Seminar");
            }

            SeminarDeleteViewModel seminarViewModel = new SeminarDeleteViewModel()
            {
                Id = seminar.Id,
                Topic = seminar.Title,
                DateAndTime = seminar.DateAndTime,
            };

            return View(seminarViewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string userId = GetUserId();

            Seminar seminar = await _seminarServices.FindSeminarToDeleteById(id);

            if (seminar == null || seminar.OrganizerId != userId)
            {
                return RedirectToAction("All", "Seminar");
            }

            await _seminarServices.DeleteSeminarAsync(seminar);
            return RedirectToAction("All", "Seminar");
        }

    }
}
