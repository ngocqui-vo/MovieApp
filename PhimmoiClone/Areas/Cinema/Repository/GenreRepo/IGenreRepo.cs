using PhimmoiClone.Areas.Cinema.Models;
using PhimmoiClone.Areas.Cinema.ViewModel;

namespace PhimmoiClone.Areas.Cinema.Repository.GenreRepo;

public interface IGenreRepo
{
    Task<List<Genre>> GetAllAsync();
    Task<Genre?> GetByIdAsync(int id);
    Task CreateAsync(GenreViewModel genreViewModel);
    Task UpdateAsync(int id, GenreViewModel genreViewModel);
    Task DeleteAsync(int id);
    Task<bool> SaveAsync();
}