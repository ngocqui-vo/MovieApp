using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieApp.Areas.Cinema.Models;

namespace MovieApp.Areas.Cinema.EntityConfig;

public class EpisodeConfiguration : IEntityTypeConfiguration<Episode>
{
    public void Configure(EntityTypeBuilder<Episode> builder)
    {
        builder
            .HasOne<Movie>(m => m.Movie)
            .WithMany(m => m.Episodes)
            .HasForeignKey(e => e.MovieId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}