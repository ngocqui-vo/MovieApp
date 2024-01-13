using System.ComponentModel.DataAnnotations;
using MovieApp.Areas.Cinema.Models;

namespace MovieApp.Areas.Cinema.ViewModel;

public class EpisodeViewModel
{
    [Required]
    public float? EpNumber { get; set; }
    public string? EpString { get; set; }
    [Required]
    public string? LinkEmbed { get; set; }
    [Required]
    public int MovieId { get; set; }
}