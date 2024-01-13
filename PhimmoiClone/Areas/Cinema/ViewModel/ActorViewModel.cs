using System.ComponentModel.DataAnnotations;
using MovieApp.Areas.Cinema.Models;

namespace MovieApp.Areas.Cinema.ViewModel;

public class ActorViewModel
{
    
    [Required(ErrorMessage = "Cần phải nhập {0}")]
    [StringLength(200, ErrorMessage = "{0} phải có độ dài từ {2} đến {1} ký tự", MinimumLength = 2)]
    public string? Name { get; set; }
    public string? Description { get; set; }
    
    public IFormFile? Image { get; set; }
    public DateTime DoB { get; set; }
    
}