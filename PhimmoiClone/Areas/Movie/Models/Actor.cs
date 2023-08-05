using System.ComponentModel.DataAnnotations;

namespace PhimmoiClone.Areas.Movie.Models;

public class Actor
{
    [Key]
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime DoB { get; set; }
    public List<Movie>? Movies { get; set; }
}