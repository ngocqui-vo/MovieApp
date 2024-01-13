using MovieApp.Areas.Cinema.Models;
using System.ComponentModel.DataAnnotations;

namespace MovieApp.Areas.Cinema.ViewModel;

public class MovieViewModel
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Cần phải nhập {0}")]
    [StringLength(200, ErrorMessage = "{0} phải có độ dài từ {2} đến {1} ký tự", MinimumLength = 2)]
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime Publish { get; set; }
    public IFormFile[]? ListImages { get; set; }

}