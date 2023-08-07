using PhimmoiClone.Areas.Cinema.Models;
using PhimmoiClone.Areas.Cinema.ViewModel;

namespace PhimmoiClone.Areas.Cinema.Repository.ActorRepo;

public class ActorRepo : IActorRepo
{
    public ActorRepo()
    {
        
    }

    public Task<List<Actor>> GetAllActorAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Actor> GetActorById(int id)
    {
        throw new NotImplementedException();
    }

    public Task CreateActor(ActorViewModel actorViewModel)
    {
        throw new NotImplementedException();
    }

    public Task UpdateActor(ActorViewModel actorViewModel)
    {
        throw new NotImplementedException();
    }

    public Task DeleteActor(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Save()
    {
        throw new NotImplementedException();
    }
}