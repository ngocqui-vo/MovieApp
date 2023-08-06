using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhimmoiClone.Areas.Cinema.Models;

namespace PhimmoiClone.Areas.Cinema.EntityConfig;

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