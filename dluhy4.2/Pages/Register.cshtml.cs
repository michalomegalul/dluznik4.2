using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
namespace dluhy4._2;
public class RegisterModel : PageModel
{
    [BindProperty]
    public string Username { get; set; }

    [BindProperty]
    public string Password { get; set; }
    public string ErrorMessage { get; set; } 

    [Authorize]
    public async Task<IActionResult> OnGetAsync()
    {
        return Page();
    }


    public async Task<IActionResult> OnPostAsync()
    {
        if(Username == null | Password == null)
        {
            ErrorMessage = "Invalid username, password or email";
            return Page();
        }
        else
        {
            SettleUpApp app = new SettleUpApp();
            app.CreateUser(Username, Password);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, Username)
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            
            return LocalRedirect("/Index");
        }
        

    }
}
