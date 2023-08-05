using System.ComponentModel.DataAnnotations;

namespace PhimmoiClone.Areas.Movie.Models;

public class MovieGenre
{
    [Required]
    public int MovieId { get; set; }
    public Movie? Movie { get; set; }
    [Required]
    public int GenreId { get; set; }
    public Genre? Genre { get; set; }
}