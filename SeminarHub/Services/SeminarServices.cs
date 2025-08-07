using Microsoft.EntityFrameworkCore;
using SeminarHub.Data;
using SeminarHub.Models;
using SeminarHub.Services.Contracts;

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

    }
}
