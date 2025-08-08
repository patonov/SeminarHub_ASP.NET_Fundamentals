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

    }
}
