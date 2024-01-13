using Microsoft.EntityFrameworkCore;
using MovieApp.Areas.Cinema.Models;
using MovieApp.Areas.Cinema.ViewModel;
using MovieApp.Data;

namespace MovieApp.Areas.Cinema.Repository.EpisodeRepo;

public class EpisodeRepo : IEpisodeRepo
{
    private readonly MyDbContext _ctx;

    public EpisodeRepo(MyDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<List<Episode>> GetAllAsync()
    {
        var episodes = await _ctx.Episodes.ToListAsync();
        return episodes;
    }

    public async Task<Episode?> GetByIdAsync(int id)
    {
        var episode = await _ctx.Episodes
            .Include(e => e.Movie)!
            .FirstOrDefaultAsync(a => a.Id == id);
        return episode;
    }

    public async Task CreateAsync(EpisodeViewModel episodeViewModel)
    {
        var episode = new Episode()
        {
            MovieId = episodeViewModel.MovieId,
            EpNumber = episodeViewModel.EpNumber,
            EpString = episodeViewModel.EpString,
            LinkEmbed = episodeViewModel.LinkEmbed
        };
        await _ctx.Episodes.AddAsync(episode);
    }

    
    public async Task UpdateAsync(EpisodeUpdateViewModel vm)
    {
        var episode = await GetByIdAsync(vm.Id);
        if (episode != null)
        {
            episode.EpNumber = vm.EpNumber;
            episode.EpString = vm.EpString;
            episode.LinkEmbed = vm.LinkEmbed;
        }
        
    }

    public async Task DeleteAsync(int id)
    {
        var episode = await GetByIdAsync(id);
        if (episode != null) _ctx.Episodes.Remove(episode);
    }

    public async Task<bool> SaveAsync()
    {
        return await _ctx.SaveChangesAsync() > 0;
    }
}