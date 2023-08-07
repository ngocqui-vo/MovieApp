using System.ComponentModel.DataAnnotations;

namespace PhimmoiClone.Areas.Cinema.Models;

public class MovieActor
{
    [Required]
    public int MovieId { get; set; }
    public Cinema.Models.Movie? Movie { get; set; }
    [Required]
    public int ActorId { get; set; }
    public Actor? Actor { get; set; }
}