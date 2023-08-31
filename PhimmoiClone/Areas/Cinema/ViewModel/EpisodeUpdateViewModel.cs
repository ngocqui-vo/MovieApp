using System.ComponentModel.DataAnnotations;

namespace PhimmoiClone.Areas.Cinema.ViewModel;

public class EpisodeUpdateViewModel
{
    public int Id { get; set; }
    [Required]
    public float? EpNumber { get; set; }
    public string? EpString { get; set; }
    [Required]
    public string? LinkEmbed { get; set; }
    
}