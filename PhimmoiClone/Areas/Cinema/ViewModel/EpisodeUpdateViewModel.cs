using System.ComponentModel.DataAnnotations;

namespace PhimmoiClone.Areas.Cinema.ViewModel;

public class EpisodeUpdateViewModel
{
    [Required]
    public float? EpNumber { get; set; }
    public string? EpString { get; set; }
    [Required]
    public string? LinkEmbed { get; set; }
    
}