using MovieApp.Areas.Cinema.Models;
using MovieApp.Areas.Cinema.ViewModel;

namespace MovieApp.Areas.Cinema.Repository.MovieRepo
{
    public interface IMovieRepo
    {
        Task<List<Movie>> GetAllAsync();
        Task<Movie?> GetByIdAsync(int id);
        Task CreateAsync(MovieViewModel movieViewModel);
        Task UpdateAsync(int id, MovieViewModel movieViewModel);
        Task DeleteAsync(int id);
        Task<bool> AddToActorsAsync(Movie movie, List<int> actorsId);
        Task<bool> AddToGenresAsync(Movie movie, List<int> genresId);
        List<int>? GetAllActorIds(Movie movie);
        List<int>? GetAllGenreIds(Movie movie);
        Task<bool> SaveAsync();
    }
}
