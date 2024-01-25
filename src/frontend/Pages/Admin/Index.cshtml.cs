using CafeReadConf.Frontend.Models;
using CafeReadConf.Frontend.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Azure;
using Microsoft.Net.Http.Headers;

namespace CafeReadConf.Pages.Admin
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private IUserService _userService;
        private IConfiguration _configuration;
        public string? Secret { get; set; }
        [BindProperty]
        public Usermodel Input { get; set; }

        public PageResult OnGet()
        {
            return Page();
        }

        public IndexModel(ILogger<IndexModel> logger, IUserService userService, IConfiguration configuration)
        {
            _logger = logger;
            _userService = userService;
            _configuration = configuration;
            Secret = _configuration.GetValue<string>("secret");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Please fill in all the inputs before hitting the \"Add New User\" Button");
            }
            else
            {
                try
                {
                    _userService.AddUser(Input);
                    TempData["UserAddedOK"] = "User added successfully";
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    ModelState.AddModelError(string.Empty, "Something went wrong while adding the user. please try again later");
                }
            }


            return Page();
        }
    }
}