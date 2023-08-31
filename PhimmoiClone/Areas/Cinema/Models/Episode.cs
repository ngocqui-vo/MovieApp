using System.ComponentModel.DataAnnotations;

namespace PhimmoiClone.Areas.Cinema.Models;

public class Episode
{
    [Key]
    public int Id { get; set; }
    public float? EpNumber { get; set; }
    public string? EpString { get; set; }
    public string? LinkEmbed { get; set; }
    public int MovieId { get; set; }
    public Movie? Movie { get; set; }
}