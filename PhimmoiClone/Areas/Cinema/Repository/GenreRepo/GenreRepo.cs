using Microsoft.EntityFrameworkCore;
using PhimmoiClone.Areas.Cinema.Models;
using PhimmoiClone.Areas.Cinema.ViewModel;
using PhimmoiClone.Data;

namespace PhimmoiClone.Areas.Cinema.Repository.GenreRepo;

public class GenreRepo : IGenreRepo
{
    private readonly MyDbContext _ctx;

    public GenreRepo(MyDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<List<Genre>> GetAllAsync()
    {
        var genres = await _ctx.Genres.ToListAsync();
        return genres;
    }

    public async Task<Genre?> GetByIdAsync(int id)
    {
        var genre = await _ctx.Genres
            .Include(g => g.MovieGenres)!
            .ThenInclude(mg => mg.Movie)
            .FirstOrDefaultAsync(g => g.Id == id);
        return genre;
    }

    public async Task CreateAsync(GenreViewModel genreViewModel)
    {
        var genre = new Genre()
        {
            Title = genreViewModel.Title,
            Description = genreViewModel.Description
        };
        await _ctx.Genres.AddAsync(genre);
    }

    
    public async Task UpdateAsync(int id, GenreViewModel genreViewModel)
    {
        var genre = await GetByIdAsync(id);
        if (genre != null)
        {
            genre.Title = genreViewModel.Title;
            genre.Description = genreViewModel.Description;
            
        }
        
    }

   

    public async Task DeleteAsync(int id)
    {
        var genre = await GetByIdAsync(id);
        if (genre != null) _ctx.Genres.Remove(genre);
    }

    public async Task<bool> SaveAsync()
    {
        return await _ctx.SaveChangesAsync() > 0;
    }
}