using ChatApp.AppData;
using ChatApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ChatApp.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ChatAppContext _context;

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "username")]
            public string UserName { get; set; }

            [Required]
            [Display(Name = "password")]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public LoginModel(ChatAppContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {

        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            var user = await _context.Users
                .Where(e => e.Email == Input.UserName && e.Password == Input.Password)
                .FirstOrDefaultAsync();

            if (user != null)
            {
                return LocalRedirect("~/Index");
            }
            else
            {
                ErrorMessage = "Wrong mail or password.";
                return Page();
            }
        }
    }
}