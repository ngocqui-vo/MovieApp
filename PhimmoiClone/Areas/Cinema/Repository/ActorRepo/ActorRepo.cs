﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PhimmoiClone.Areas.Cinema.Models;
using PhimmoiClone.Areas.Cinema.ViewModel;
using PhimmoiClone.Data;

namespace PhimmoiClone.Areas.Cinema.Repository.ActorRepo;

public class ActorRepo : IActorRepo
{
    private readonly MyDbContext _ctx;

    public ActorRepo(MyDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<List<Actor>> GetAllActorAsync()
    {
        var actors = await _ctx.Actors.ToListAsync();
        return actors;
    }

    public async Task<Actor?> GetActorByIdAsync(int id)
    {
        var actor = await _ctx.Actors
            .Include(a => a.MovieActors)!
            .ThenInclude(ma => ma.Movie)
            .FirstOrDefaultAsync(a => a.Id == id);
        return actor;
    }

    public async Task CreateActorAsync(ActorViewModel actorViewModel)
    {
        var actor = new Actor()
        {
            Name = actorViewModel.Name,
            Description = actorViewModel.Description,
            DoB = actorViewModel.DoB
        };
        await _ctx.Actors.AddAsync(actor);
    }

    
    public async Task UpdateActorAsync(int id, ActorViewModel actorViewModel)
    {
        var actor = await GetActorByIdAsync(id);
        if (actor != null)
        {
            actor.Name = actorViewModel.Name;
            actor.Description = actorViewModel.Description;
            actor.DoB = actorViewModel.DoB;
        }
        
    }

   

    public async Task DeleteActorAsync(int id)
    {
        var actor = await GetActorByIdAsync(id);
        if (actor != null) _ctx.Actors.Remove(actor);
    }

    public async Task<bool> SaveAsync()
    {
        return await _ctx.SaveChangesAsync() > 0;
    }
}