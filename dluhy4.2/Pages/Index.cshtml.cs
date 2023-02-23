using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
namespace dluhy4._2;
[Authorize]
public class IndexModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;

    public SelectList UserSelectList { get; set; }

    [BindProperty]
    public int GiverId { get; set; }

    [BindProperty]
    public int ReceiverId { get; set; }

    [BindProperty]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
    public decimal Amount { get; set; }

    public IndexModel(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }



    public async Task OnGetAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        UserSelectList = new SelectList(users, "Id", "UserName");
    }


    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            //create transaction
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

            var app = new SettleUpApp();
            app.CreateTransaction(GiverId, ReceiverId, Amount);

            return RedirectToPage();
            //show balance

        }

        await OnGetAsync();
        return Page();
    }
}
