using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieApp.Areas.Cinema.Models;
using MovieApp.Areas.Identity.Models;

namespace MovieApp.Areas.Cinema.EntityConfig;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasKey(c => c.Id);

        builder
            .HasOne<Movie>(c => c.Movie)
            .WithMany(m => m.Comments)
            .HasForeignKey(c => c.MovieId);

        builder
            .HasOne<AppUser>(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId);

        builder.Property(c => c.Message).IsRequired();
    }
}