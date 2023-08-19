using Microsoft.EntityFrameworkCore;
using PhimmoiClone.Areas.Cinema.Models;
using PhimmoiClone.Areas.Cinema.ViewModel;
using PhimmoiClone.Data;

namespace PhimmoiClone.Areas.Cinema.Repository.MovieRepo
{
    public class MovieRepo : IMovieRepo
    {
        private MyDbContext _ctx;

        public MovieRepo(MyDbContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<List<Movie>> GetAllAsync()
        {
            var movies = await _ctx.Movies.ToListAsync();
            return movies;
        }

        public async Task<Movie?> GetByIdAsync(int id)
        {
            var movie = await _ctx.Movies.FirstOrDefaultAsync(x => x.Id == id);
            return movie;
        }


        public async Task CreateAsync(MovieViewModel movieViewModel)
        {
            Movie movie = new Movie
            {
                Name = movieViewModel.Name,
                Description = movieViewModel.Description,
                Publish = movieViewModel.Publish,
            };
            await _ctx.Movies.AddAsync(movie);
        }

        public async Task DeleteAsync(int id)
        {
            var movie = await GetByIdAsync(id);
            if (movie != null)
                _ctx.Movies.Remove(movie);
        }

        
        public async Task UpdateAsync(int id, MovieViewModel movieViewModel)
        {
            var movie = await GetByIdAsync(id);
            if (movie != null)
            {
                movie.Name = movie.Name;
                movie.Description = movie.Description;
                movie.Publish = movie.Publish;
            }
        }
        public async Task<bool> SaveAsync()
        {
            if (await _ctx.SaveChangesAsync() > 0)
                return true;
            return false;
        }

        public async Task<bool> AddToActorAsync(Movie movie, int actorId)
        {
            var actor = await _ctx.Actors.FirstOrDefaultAsync(a => a.Id == actorId);
            movie?.MovieActors?.Add(new MovieActor(){Movie = movie, Actor = actor});
            if (await SaveAsync())
                return true;
            return false;
        }

        public async Task<bool> AddToActorsAsync(Movie movie, List<int> actorsId)
        {
            var actors = await _ctx.Actors.Where(a => actorsId.Contains(a.Id)).ToListAsync();
            var movieActors = new List<MovieActor>();
            foreach (var actor in actors)
            {
                var movieActor = new MovieActor { Movie = movie, Actor = actor };
                movieActors.Add(movieActor);
            }

            movie?.MovieActors?.AddRange(movieActors);
            return await SaveAsync();
        }

        public async Task<bool> RemoveFromActorAsync(Movie movie, int actorId)
        {
            var actor = await _ctx.Actors.FirstOrDefaultAsync(a => a.Id == actorId);
            if (actor == null) return false;

            var movieActorToRemove = movie?.MovieActors.SingleOrDefault(ma => ma.ActorId == actor.Id);
            if (movieActorToRemove != null)
            {
                movie?.MovieActors?.Remove(movieActorToRemove);
            }
            
            return await SaveAsync();
        }

        public async Task<bool> RemoveFromActorsAsync(Movie movie, List<int> actorsId)
        {
            //var actors = await _ctx.Actors.Where(a => actorsId.Contains(a.Id)).ToListAsync();
            //var movieActors = new List<MovieActor>();
            //foreach (var actor in actors)
            //{
            //    var movieActor = new MovieActor { Movie = movie, Actor = actor };
            //    movieActors.Add(movieActor);
            //}

            movie?.MovieActors?.RemoveAll(ma => actorsId.Contains(ma.MovieId));
            return await SaveAsync();
        }

    }
}
