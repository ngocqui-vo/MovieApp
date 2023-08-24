using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PhimmoiClone.Areas.Cinema.Models;
using PhimmoiClone.Areas.Cinema.ViewModel;
using PhimmoiClone.Data;

namespace PhimmoiClone.Areas.Cinema.Repository.ActorRepo;

public class ActorRepo : IActorRepo
{
    private readonly MyDbContext _ctx;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ActorRepo(
        MyDbContext ctx,
        IWebHostEnvironment webHostEnvironment)
    {
        _ctx = ctx;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<List<Actor>> GetAllAsync()
    {
        var actors = await _ctx.Actors.ToListAsync();
        return actors;
    }

    public async Task<Actor?> GetByIdAsync(int id)
    {
        var actor = await _ctx.Actors
            .Include(a => a.MovieActors)!
            .ThenInclude(ma => ma.Movie)
            .FirstOrDefaultAsync(a => a.Id == id);
        return actor;
    }

    public async Task CreateAsync(ActorViewModel actorViewModel)
    {
        var uniqueFileName = await UploadFile(actorViewModel);
        var actor = new Actor()
        {
            Name = actorViewModel.Name,
            Description = actorViewModel.Description,
            DoB = actorViewModel.DoB,
            Image = uniqueFileName
        };
        await _ctx.Actors.AddAsync(actor);
    }

    private async Task<string?> UploadFile(ActorViewModel actorViewModel)
    {
        string? uniqueFileName = null;
        if (actorViewModel.Image != null)
        {
            string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            uniqueFileName = Guid.NewGuid().ToString() + "_" + actorViewModel.Image.FileName;
            string filePath = Path.Combine(uploadFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await actorViewModel.Image.CopyToAsync(fileStream);
            }
        }
        return uniqueFileName;
    }

    
    public async Task UpdateAsync(int id, ActorViewModel actorViewModel)
    {
        string? uniqueFileName = await UploadFile(actorViewModel);
        var actor = await GetByIdAsync(id);
        if (actor != null)
        {
            actor.Name = actorViewModel.Name;
            actor.Description = actorViewModel.Description;
            actor.DoB = actorViewModel.DoB;
            actor.Image = uniqueFileName ?? actor.Image;
        }
        
    }

   

    public async Task DeleteAsync(int id)
    {
        var actor = await GetByIdAsync(id);
        if (actor != null) _ctx.Actors.Remove(actor);
    }

    public async Task<bool> SaveAsync()
    {
        return await _ctx.SaveChangesAsync() > 0;
    }
}