using System.ComponentModel.DataAnnotations;

namespace PhimmoiClone.Areas.Cinema.Models;

public class Genre
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "Cần phải nhập {0}")]
    [StringLength(200, ErrorMessage = "{0} phải có độ dài từ {2} đến {1} ký tự", MinimumLength = 2)]
    
    public string? Title { get; set; }
    public string? Description { get; set; }
    public List<MovieGenre>? MovieGenres { get; set; }
}