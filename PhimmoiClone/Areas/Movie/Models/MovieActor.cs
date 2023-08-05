using System.ComponentModel.DataAnnotations;

namespace PhimmoiClone.Areas.Movie.Models;

public class MovieActor
{
    [Required]
    public int MovieId { get; set; }
    public Movie? Movie { get; set; }
    [Required]
    public int ActorId { get; set; }
    public Actor? Actor { get; set; }
}