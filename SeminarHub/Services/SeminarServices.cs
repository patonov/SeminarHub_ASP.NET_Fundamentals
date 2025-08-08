using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeminarHub.Data;
using SeminarHub.Data.Models;
using SeminarHub.Models;
using SeminarHub.Services.Contracts;
using static SeminarHub.Common.DataValidationConstants;

namespace SeminarHub.Services
{
    public class SeminarServices : ISeminarService
    {
        private readonly SeminarHubDbContext _context;

        public SeminarServices(SeminarHubDbContext context)
        { 
            _context = context;
        }

        public async Task<IEnumerable<SeminarAllViewModel>> GetSeminarAllsAsync()
        {
            IEnumerable<SeminarAllViewModel> model = await _context.Seminars
                .Include(s => s.Organizer)
                .Include(s => s.Category)
                .Select(s => new SeminarAllViewModel 
                { 
                Id = s.Id,
                Topic = s.Title,
                Lecturer = s.Lecturer,
                Category = s.Category.Name,
                DateAndTime = s.DateAndTime.ToString("dd/MM/yyyy HH:mm"),
                Organizer = s.Organizer.UserName,
                }).ToListAsync();

            return model;
        }

        public async Task<SeminarAddViewModel> GetSeminarAddViewModelAsync()
        {
            var model = new SeminarAddViewModel();

            var categories = await _context.Categories
                .Select(c => new CategoryViewModel 
                { 
                    Id = c.Id,
                    Name = c.Name,
                })
                .ToListAsync();
            
            model.Categories = categories;
            return model;
        }

        public async Task AddSeminarAsync(SeminarAddViewModel model, string userId)
        {
            if (!DateTime.TryParseExact(model.DateAndTime, SeminarDateAndTimeFormat, null, System.Globalization.DateTimeStyles.None, out var seminarDateAndTime))
            {
                throw new InvalidOperationException("Invalid date format");
            }

            Seminar seminar = new Seminar()
            {
                Title = model.Topic,
                Lecturer = model.Lecturer,
                Details = model.Details,
                DateAndTime = seminarDateAndTime,
                Duration = model.Duration,
                CategoryId = model.CategoryId,
                OrganizerId = userId,
            };

            await _context.Seminars.AddAsync(seminar);
            await _context.SaveChangesAsync();
        }

        public async Task<SeminarAddViewModel> GetSeminarViewModelToEditAsync(int id)
        {
            var categories = await _context.Categories
                .Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                })
                .ToListAsync();

            var model = await _context.Seminars.Where(s => s.Id == id)
                .Select(s => new SeminarAddViewModel
                {
                    Topic = s.Title,
                    Lecturer = s.Lecturer,
                    Details = s.Details,
                    DateAndTime = s.DateAndTime.ToString("dd/MM/yyyy HH:mm"),
                    Duration = s.Duration,
                    CategoryId = s.CategoryId,
                    OrganizerId = s.OrganizerId,
                    Categories = categories,
                })
                .FirstOrDefaultAsync();

            return model;
        }

        public async Task<Seminar> GetSeminarAsync(int id)
        { 
            Seminar? seminar = await _context.Seminars.FirstOrDefaultAsync(s => s.Id == id);
            
            return seminar;
        }

        public async Task EditSeminarAsync(SeminarAddViewModel model, Seminar target)
        {
            if (!DateTime.TryParseExact(model.DateAndTime, "dd/MM/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None,
               out var timeOfSeminar))
            {
                throw new InvalidOperationException("Invalid date format");
            }

            target.Title = model.Topic;
            target.Lecturer = model.Lecturer;
            target.Details = model.Details;
            target.DateAndTime = timeOfSeminar;
            target.Duration = model.Duration;
            target.CategoryId = model.CategoryId;
            
            await _context.Seminars.AddAsync(target);
            await _context.SaveChangesAsync();
        }

        public async Task<SeminarDetailsViewModel> GetSeminarDetailsAsync(int id)
        {
            var model = await _context.Seminars.Where(s => s.Id == id)
                .Select(s => new SeminarDetailsViewModel
                {
                Id = s.Id,
                Topic = s.Title,
                DateAndTime = s.DateAndTime.ToString("dd/MM/yyyy HH:mm"),
                Duration = s.Duration,
                Lecturer = s.Lecturer,
                Details = s.Details,
                Category = s.Category.Name,
                Organizer = s.Organizer.UserName,
                })
                .FirstOrDefaultAsync();
            
            return model;
        }

    }
}
