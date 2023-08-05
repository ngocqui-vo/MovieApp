using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using PhimmoiClone.Areas.Identity.Models;
using PhimmoiClone.Ultilities;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Encodings.Web;

namespace PhimmoiClone.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Route("Account/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
       
        private readonly ILogger<AccountController> _logger;
        private readonly IEmailSender _emailSender;
        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            
            ILogger<AccountController> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            
            _logger = logger;
            _emailSender = emailSender;
        }
        

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            var result = await _signInManager.PasswordSignInAsync(
                                            loginModel.UserNameOrEmail, 
                                            loginModel.Password, 
                                            loginModel.RememberMe, 
                                            lockoutOnFailure: true);
            // tìm user với email
            if (ModelState.IsValid)
            {
                if (!result.Succeeded && AppUltilities.IsValidEmail(loginModel.UserNameOrEmail))
                {
                    var user = await _userManager.FindByEmailAsync(loginModel.UserNameOrEmail);
                    if (user != null)
                    {
                        result = await _signInManager.PasswordSignInAsync(user.UserName, loginModel.Password, loginModel.RememberMe, lockoutOnFailure: true);
                    }
                }

                if (result.Succeeded)
                {
                    _logger.LogInformation(1, "User đăng nhập");
                }

                if (result.IsLockedOut)
                {
                    return View("Lockout");
                }
                return RedirectToAction("Index", "Home", new {area = ""});

            }

            ModelState.AddModelError("", "Không thể đăng nhập");
            return View(loginModel);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User đăng xuất");
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerModel)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = registerModel.Username,
                    Email = registerModel.Email
                };
                var result = await _userManager.CreateAsync(user, registerModel.Password);

                if (result.Succeeded)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    var callbackUrl = Url.ActionLink(
                            action: nameof(ConfirmEmail),
                            values: new { area = "Identity", userId = user.Id, code = code },
                            protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(
                            registerModel.Email,
                            "Xác nhận địa chỉ Email",
                            @$"Bạn đã đăng ký tài khoản trên Phimmoi, 
                           hãy <a href='{HtmlEncoder.Default.Encode(callbackUrl!)}'>bấm vào đây</a> 
                           để kích hoạt tài khoản."
                        );

                    if (_userManager.Options.SignIn.RequireConfirmedEmail)
                    {
                        return LocalRedirect(Url.Action(nameof(RegisterConfirmation))!);
                    }
                }
                ModelState.AddModelError("", result.ToString()!);
            }
            return View(registerModel);

        }
        public IActionResult RegisterConfirmation()
        {
            return View();
        }
        public async Task<IActionResult> ConfirmEmail(string? userId, string? code)
        {
            if (userId == null || code == null)
            {
                return View("ErrorConfirmEmail");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("ErrorConfirmEmail");
            }
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);

            return View(result.Succeeded ? "ConfirmEmail" : "ErrorConfirmEmail");
            
        }
    }
}
