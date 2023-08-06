using System.ComponentModel.DataAnnotations;

namespace PhimmoiClone.Areas.Cinema.Models;

public class MovieGenre
{
    [Required]
    public int MovieId { get; set; }
    public Cinema.Models.Movie? Movie { get; set; }
    [Required]
    public int GenreId { get; set; }
    public Genre? Genre { get; set; }
}