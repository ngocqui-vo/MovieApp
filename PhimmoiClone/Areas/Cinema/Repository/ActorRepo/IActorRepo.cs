using PhimmoiClone.Areas.Cinema.Models;
using PhimmoiClone.Areas.Cinema.ViewModel;

namespace PhimmoiClone.Areas.Cinema.Repository.ActorRepo;

public interface IActorRepo
{
    Task<List<Actor>> GetAllActorAsync();
    Task<Actor?> GetActorByIdAsync(int id);
    Task CreateActorAsync(ActorViewModel actorViewModel);
    Task UpdateActorAsync(int id, ActorViewModel actorViewModel);
    Task DeleteActorAsync(int id);
    Task<bool> SaveAsync();
}