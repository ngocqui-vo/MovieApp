using System.ComponentModel.DataAnnotations;

namespace MovieApp.Areas.Cinema.Models;

public class MovieImage
{
    [Key]
    public int Id { get; set; }
    public string? Name { get; set; }
    [Required]
    public string? Path { get; set; }

    public int MovieId { get; set; }
    public Movie? Movie { get; set; }
}