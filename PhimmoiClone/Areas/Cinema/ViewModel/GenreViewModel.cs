using System.ComponentModel.DataAnnotations;
using MovieApp.Areas.Cinema.Models;

namespace MovieApp.Areas.Cinema.ViewModel;

public class GenreViewModel
{
    
    [Required(ErrorMessage = "Cần phải nhập {0}")]
    [StringLength(200, ErrorMessage = "{0} phải có độ dài từ {2} đến {1} ký tự", MinimumLength = 2)]
    public string? Title { get; set; }
    public string? Description { get; set; }
    
}