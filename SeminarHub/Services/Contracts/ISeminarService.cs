using SeminarHub.Models;

namespace SeminarHub.Services.Contracts
{
    public interface ISeminarService
    {
        Task<IEnumerable<SeminarAllViewModel>> GetSeminarAllsAsync();

        Task<SeminarAddViewModel> GetSeminarAddViewModelAsync();
        Task AddSeminarAsync(SeminarAddViewModel model, string userId);
    }
}
