using Microsoft.EntityFrameworkCore;
using PhimmoiClone.Areas.Cinema.Models;
using PhimmoiClone.Areas.Cinema.Repository.MovieRepo;
using PhimmoiClone.Data;

namespace PhimmoiClone.Services;

public class MovieServices
{
    private readonly MyDbContext _ctx;
    private readonly IMovieRepo _repo;


    public MovieServices(MyDbContext ctx, IMovieRepo repo)
    {
        _ctx = ctx;
        _repo = repo;
    }

    public async Task<List<Movie>> GetAllMovie(string searchName = "")
    {
        var movies = from m in _ctx.Movies
                    select m;
        if (!string.IsNullOrEmpty(searchName))
        {
            movies = movies.Where(m => m.Name.Contains(searchName));
        }

        return await movies.ToListAsync();
    }

    public async Task<Movie> GetMovieById(int id)
    {
        var movie = await _repo.GetByIdAsync(id);
        return movie;
    }
}