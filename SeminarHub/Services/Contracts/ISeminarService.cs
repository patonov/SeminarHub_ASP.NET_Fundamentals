using SeminarHub.Data.Models;
using SeminarHub.Models;

namespace SeminarHub.Services.Contracts
{
    public interface ISeminarService
    {
        Task<IEnumerable<SeminarAllViewModel>> GetSeminarAllsAsync();

        Task<SeminarAddViewModel> GetSeminarAddViewModelAsync();
        Task AddSeminarAsync(SeminarAddViewModel model, string userId);

        Task<SeminarAddViewModel> GetSeminarViewModelToEditAsync(int id);
        Task EditSeminarAsync(SeminarAddViewModel model, Seminar target);
        Task<Seminar> GetSeminarAsync(int id);

        Task<SeminarDetailsViewModel> GetSeminarDetailsAsync(int id); 

        Task<IEnumerable<SeminarJoinedViewModel>> GetAllSeminarsJoinedOfUserAsync(string userId);

        Task AddSeminarToJoinedAsync(string userId, SeminarJoinedViewModel seminar);
        Task<SeminarJoinedViewModel?> GetForJoiningSeminarByIdAsync(int id);

        Task LeaveSeminar(string userId, SeminarJoinedViewModel seminar);

        Task DeleteSeminarAsync(Seminar seminar);
        Task<Seminar> FindSeminarToDeleteById(int id);
    }
}
