using System.ComponentModel.DataAnnotations;
using PhimmoiClone.Areas.Cinema.Models;

namespace PhimmoiClone.Areas.Cinema.ViewModel;

public class EpisodeViewModel
{
    
    public int Id { get; set; }
    [Required]
    public float? EpNumber { get; set; }
    public string? EpString { get; set; }
    [Required]
    public string? LinkEmbed { get; set; }
    [Required]
    public int MovieId { get; set; }
}