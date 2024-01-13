using System.ComponentModel.DataAnnotations;

namespace MovieApp.Areas.Cinema.Models;

public class Actor
{
    [Key]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Cần phải nhập {0}")]
    [StringLength(200, ErrorMessage = "{0} phải có độ dài từ {2} đến {1} ký tự", MinimumLength = 2)]
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    public DateTime DoB { get; set; }
    public List<MovieActor>? MovieActors { get; set; }
}