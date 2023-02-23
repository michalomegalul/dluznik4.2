using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
namespace dluhy4._2;
public class LoginModel : PageModel
{
    [BindProperty]
    public string Username { get; set; }

    [BindProperty]
    public string Password { get; set; }

    public string ErrorMessage { get; set; }
    
    

    [Authorize]
    public async Task<IActionResult> OnGetAsync()
    {

        var user = await _userManager.GetUserAsync(User);
        if (user != null)
        {
            Username = user.UserName;
        }
        return Page();
    }
    private readonly UserManager<IdentityUser> _userManager;

    public LoginModel(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }
    public async Task<IActionResult> OnPostAsync()
    {
        SettleUpApp app = new SettleUpApp();
        if (app.Login(Username, Password))
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, Username)
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            return LocalRedirect("/IndexModel");
        }
        else
        {
            ErrorMessage = "Invalid username or password.";
            return Page();

        }
    }
}


