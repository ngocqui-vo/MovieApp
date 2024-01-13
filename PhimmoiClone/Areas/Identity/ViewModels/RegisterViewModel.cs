using System.ComponentModel.DataAnnotations;

namespace MovieApp.Areas.Identity.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Phải nhập {0}")]
        [EmailAddress(ErrorMessage = "Sai định dạng email")]
        [Display(Name = "Email")]
        public string Email { get; set; }


        [StringLength(30, ErrorMessage = "{0} phải dài từ {2} đến {1} ký tự", MinimumLength = 2)]
        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Phải nhập {0}")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Display(Name = "Nhập lại mật khẩu")]
        [Required(ErrorMessage = "Phải nhập {0}")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu lặp lại không khớp")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Phải nhập {0}")]
        [DataType(DataType.Text)]
        [StringLength(30, ErrorMessage = "{0} phải dài từ {2} đến {1} ký tự", MinimumLength = 2)]
        [Display(Name = "Tên tài khoản")]
        public string Username { get; set; }
    }
}
