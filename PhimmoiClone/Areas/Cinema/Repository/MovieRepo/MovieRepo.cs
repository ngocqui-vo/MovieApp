using Microsoft.EntityFrameworkCore;
using PhimmoiClone.Areas.Cinema.Models;
using PhimmoiClone.Areas.Cinema.ViewModel;
using PhimmoiClone.Data;
using System.Linq;

namespace PhimmoiClone.Areas.Cinema.Repository.MovieRepo
{
    public class MovieRepo : IMovieRepo
    {
        private MyDbContext _ctx;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MovieRepo(MyDbContext ctx, IWebHostEnvironment webHostEnvironment)
        {
            _ctx = ctx;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<List<Movie>> GetAllAsync()
        {
            var movies = await _ctx.Movies.ToListAsync();
            return movies;
        }

        public async Task<Movie?> GetByIdAsync(int id)
        {
            var movie = await _ctx
                .Movies
                .Include(m => m.MovieActors)!
                .ThenInclude(ma => ma.Actor)
                .Include(m => m.MovieGenres)!
                .ThenInclude(mg => mg.Genre)
                .Include(m => m.Episodes)
                .Include(m => m.MovieImages)
                .FirstOrDefaultAsync(x => x.Id == id);
            return movie;
        }


        public async Task CreateAsync(MovieViewModel movieViewModel)
        {
            // create movie
            Movie movie = new Movie
            {
                Name = movieViewModel.Name,
                Description = movieViewModel.Description,
                Publish = movieViewModel.Publish,
            };
            await _ctx.Movies.AddAsync(movie);

            // create movieImages
            var imageNames = await UploadImagesAsync(movieViewModel);
            List<MovieImage> listImage = new List<MovieImage>();
            for (int i = 0; i < movieViewModel.ListImages?.Length; i++)
            {
                MovieImage image = new MovieImage()
                {
                    Name = "",
                    Path = imageNames[i],
                    MovieId = movie.Id,
                    Movie = movie
                };
                listImage.Add(image);
            }

            await _ctx.MovieImages.AddRangeAsync(listImage);
        }

        private async Task<string[]> UploadImagesAsync(MovieViewModel? movieViewModel)
        {
            var uniqueNames = new string[movieViewModel.ListImages.Length];
            if (movieViewModel.ListImages.Length <= 0)
                return uniqueNames;

            string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");

            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);

            for (int i = 0; i < uniqueNames.Length; i++)
            {
                uniqueNames[i] = Guid.NewGuid() + "_" + movieViewModel.ListImages[i].FileName;
                string filePath = Path.Combine(uploadFolder, uniqueNames[i]);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await movieViewModel.ListImages[i].CopyToAsync(fileStream);
                }
            }


            return uniqueNames;
        }


        public async Task DeleteAsync(int id)
        {
            var movie = await GetByIdAsync(id);
            if (movie != null)
            {
                string imagesFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");

                if (Directory.Exists(imagesFolder))
                {
                    foreach (var movieImage in movie.MovieImages!)
                    {
                        var path = Path.Combine(_webHostEnvironment.WebRootPath + "images" + movieImage.Path);
                        File.Delete(path);
                    }
                }
                _ctx.Movies.Remove(movie);
            }
        }


        public async Task UpdateAsync(int id, MovieViewModel movieViewModel)
        {
            var movie = await GetByIdAsync(id);

            if (movie != null)
            {
                movie.Name = movie.Name;
                movie.Description = movie.Description;
                movie.Publish = movie.Publish;
                var movieImages = movie.MovieImages;

                if (movieViewModel is { ListImages: not null })
                {
                    foreach (var movieImage in movieImages!)
                    {
                        var path = Path.Combine(_webHostEnvironment.WebRootPath + "images" + movieImage.Path);
                        File.Delete(path);
                    }

                    _ctx.MovieImages.RemoveRange(movieImages);

                    var imageNames = await UploadImagesAsync(movieViewModel);
                    List<MovieImage> listImage = new List<MovieImage>();
                    for (int i = 0; i < movieViewModel.ListImages?.Length; i++)
                    {
                        MovieImage image = new MovieImage()
                        {
                            Name = "",
                            Path = imageNames[i],
                            MovieId = movie.Id,
                            Movie = movie
                        };
                        listImage.Add(image);
                    }

                    await _ctx.MovieImages.AddRangeAsync(listImage);
                }
            }
        }


        public List<int>? GetAllActorIds(Movie movie)
        {
            var actorIds = movie?.MovieActors?.Select(m => m.ActorId).ToList();
            return actorIds;
        }

        public List<int>? GetAllGenreIds(Movie movie)
        {
            var genreIds = movie?.MovieGenres?.Select(m => m.GenreId).ToList();
            return genreIds;
        }

        public async Task<bool> SaveAsync()
        {
            if (await _ctx.SaveChangesAsync() > 0)
                return true;
            return false;
        }

        public async Task<bool> AddToActorsAsync(Movie movie, List<int> actorsId)
        {
            // xóa tất cả actor cũ
            movie.MovieActors?.Clear();
            bool removeA = await SaveAsync();

            var actors = await _ctx.Actors.Where(a => actorsId.Contains(a.Id)).ToListAsync();

            var movieActors = new List<MovieActor>();
            foreach (var actor in actors)
            {
                var movieActor = new MovieActor { Movie = movie, Actor = actor };
                movieActors.Add(movieActor);
            }

            _ctx.MovieActor?.AddRange(movieActors);
            bool removeB = await SaveAsync();
            return removeA || removeB;
        }

        public async Task<bool> AddToGenresAsync(Movie movie, List<int> genresId)
        {
            // xóa tất cả genre cũ
            movie.MovieGenres?.Clear();
            bool removeA = await SaveAsync();

            var genres = await _ctx.Genres.Where(g => genresId.Contains(g.Id)).ToListAsync();

            var movieGenres = new List<MovieGenre>();
            foreach (var genre in genres)
            {
                var movieGenre = new MovieGenre { Movie = movie, Genre = genre };
                movieGenres.Add(movieGenre);
            }

            _ctx.MovieGenre?.AddRange(movieGenres);
            bool removeB = await SaveAsync();
            return removeA || removeB;
        }
    }
}