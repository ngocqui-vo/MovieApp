using MovieApp.Areas.Cinema.Models;
using MovieApp.Areas.Cinema.ViewModel;

namespace MovieApp.Areas.Cinema.Repository.EpisodeRepo;

public interface IEpisodeRepo
{
    Task<List<Episode>> GetAllAsync();
    Task<Episode?> GetByIdAsync(int id);
    Task CreateAsync(EpisodeViewModel vm);
    Task UpdateAsync(EpisodeUpdateViewModel vm);
    Task DeleteAsync(int id);
    Task<bool> SaveAsync();
}