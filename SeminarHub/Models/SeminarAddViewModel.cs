using Microsoft.AspNetCore.Identity;
using SeminarHub.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static SeminarHub.Common.DataValidationConstants;
using System.Configuration;

namespace SeminarHub.Models
{
    public class SeminarAddViewModel
    {
        [Required]
        [StringLength(SeminarTitleMaxLength, MinimumLength = SeminarTitleMinLength)]
        public string Topic { get; set; } = null!;

        [Required]
        [StringLength(SeminarLecturerMaxLength, MinimumLength = SeminarLecturerMinLength)]
        public string Lecturer { get; set; } = null!;

        [Required]
        [StringLength(SeminarDetailsMaxLength, MinimumLength = SeminarDetailsMinLength)]
        public string Details { get; set; } = null!;

        [Required]
        [RegularExpression(@"^\d{2}/\d{2}/\d{4} \d{2}:\d{2}$", ErrorMessage = "The date and time is not in a correct format.")]
        public string DateAndTime { get; set; } = null!;

        [Range(SeminarMinDuration, SeminarMaxDuration)]
        public int Duration { get; set; }

        public int CategoryId { get; set; }

        public IEnumerable<CategoryViewModel>? Categories { get; set; }

        public string? OrganizerId { get; set; }
                
    }
}
