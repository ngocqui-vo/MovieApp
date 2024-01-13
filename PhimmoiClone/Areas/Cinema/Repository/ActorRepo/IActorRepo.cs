using MovieApp.Areas.Cinema.Models;
using MovieApp.Areas.Cinema.ViewModel;

namespace MovieApp.Areas.Cinema.Repository.ActorRepo;

public interface IActorRepo
{
    Task<List<Actor>> GetAllAsync();
    Task<Actor?> GetByIdAsync(int id);
    Task CreateAsync(ActorViewModel actorViewModel);
    Task UpdateAsync(int id, ActorEditViewModel actorViewModel);
   
    Task DeleteAsync(int id);
    Task<bool> SaveAsync();
}