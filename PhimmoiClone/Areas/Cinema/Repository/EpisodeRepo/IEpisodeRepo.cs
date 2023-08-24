using PhimmoiClone.Areas.Cinema.Models;
using PhimmoiClone.Areas.Cinema.ViewModel;

namespace PhimmoiClone.Areas.Cinema.Repository.EpisodeRepo;

public interface IEpisodeRepo
{
    Task<List<Episode>> GetAllAsync();
    Task<Episode?> GetByIdAsync(int id);
    Task CreateAsync(EpisodeViewModel vm);
    Task UpdateAsync(int id, EpisodeViewModel vm);
    Task DeleteAsync(int id);
    Task<bool> SaveAsync();
}