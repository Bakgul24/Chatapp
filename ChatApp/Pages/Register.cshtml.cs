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
    public class RegisterModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ChatAppContext _context;

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "name")]
            public string Name { get; set; }

            [Required]
            [Display(Name = "surname")]
            public string Surname { get; set; }

            [Required]
            [Display(Name = "email")]
            public string Email { get; set; }

            [Required]
            [Display(Name = "password")]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public RegisterModel(ILogger<IndexModel> logger, ChatAppContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task OnGetAsync()
        {

        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            var query = await _context.Users.AddAsync(new User
            {
                Email = Input.Email,
                Password = Input.Password,
                FirstName = Input.Name,
                LastName = Input.Surname
            });

            await _context.SaveChangesAsync();

            return LocalRedirect("~/Login");
        }
    }
}