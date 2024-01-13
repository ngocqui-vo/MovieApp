using MovieApp.Areas.Cinema.Models;
using MovieApp.Areas.Cinema.ViewModel;

namespace MovieApp.Areas.Cinema.Repository.GenreRepo;

public interface IGenreRepo
{
    Task<List<Genre>> GetAllAsync();
    Task<Genre?> GetByIdAsync(int id);
    Task CreateAsync(GenreViewModel genreViewModel);
    Task UpdateAsync(int id, GenreViewModel genreViewModel);
    Task DeleteAsync(int id);
    Task<bool> SaveAsync();
}