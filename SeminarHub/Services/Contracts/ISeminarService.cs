using SeminarHub.Models;

namespace SeminarHub.Services.Contracts
{
    public interface ISeminarService
    {
        Task<IEnumerable<SeminarAllViewModel>> GetSeminarAllsAsync();
    }
}
