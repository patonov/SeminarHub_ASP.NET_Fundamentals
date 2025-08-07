using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeminarHub.Data.Models
{
    public class SeminarParticipant
    {
        [Required]
        public int SeminarId { get; set; }

        [Required]
        [ForeignKey(nameof(SeminarId))]
        public virtual Seminar Seminar { get; set; } = null!;

        [Required]
        public string ParticipantId { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(ParticipantId))]
        public IdentityUser Participant {  get; set; } = null!;

    }
}
