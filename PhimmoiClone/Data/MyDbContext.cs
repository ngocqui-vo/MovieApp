using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PhimmoiClone.Areas.Cinema.EntityConfig;
using PhimmoiClone.Areas.Cinema.Models;
using PhimmoiClone.Areas.Identity.Models;

namespace PhimmoiClone.Data
{
    public class MyDbContext : IdentityDbContext<AppUser>
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        public DbSet<Actor> Actors { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<MovieActor> MovieActor { get; set; }
        public DbSet<MovieGenre> MovieGenre { get; set; }
        public DbSet<MovieImage> MovieImages { get; set; }
        
        public DbSet<Comment> Comments { get; set; }
        

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            // Bỏ tiền tố AspNet của các bảng: mặc định
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName != null && tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }

            // Episode model config
            builder.ApplyConfiguration(new EpisodeConfiguration());

            // MovieActor model config
            builder.ApplyConfiguration(new MovieActorConfiguration());
            
            
            // MovieGenre model config
            builder.ApplyConfiguration(new MovieGenreConfiguration());
            
            // Comment model config
            builder.ApplyConfiguration(new CommentConfiguration());
        }
        
    }
}
