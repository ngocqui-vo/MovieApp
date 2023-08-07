using PhimmoiClone.Areas.Cinema.Models;
using PhimmoiClone.Areas.Cinema.ViewModel;

namespace PhimmoiClone.Areas.Cinema.Repository.ActorRepo;

public interface IActorRepo
{
    Task<List<Actor>> GetAllActorAsync();
    Task<Actor> GetActorById(int id);
    Task CreateActor(ActorViewModel actorViewModel);
    Task UpdateActor(ActorViewModel actorViewModel);
    Task DeleteActor(int id);
    Task<bool> Save();
}