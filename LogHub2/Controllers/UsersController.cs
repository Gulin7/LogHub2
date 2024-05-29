using LogHub2.Data;
using LogHub2.Models;
using LogHub2.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LogHub2.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public UsersController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
  
        protected void SetLoggedInUser(string username)
        {
            HttpContext.Session.SetString("CurrentUser", username);
        }

        protected string GetLoggedInUser()
        {
            return HttpContext.Session.GetString("CurrentUser");
        }

        protected bool IsUserLoggedIn()
        {
            return !string.IsNullOrEmpty(GetLoggedInUser());
        }
        

        // Register
        [HttpGet("/register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Username = viewModel.Username,
                    Password = viewModel.Password,
                    Email = viewModel.Email,
                };

                await dbContext.Users.AddAsync(user);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("Login");
            }
            return View(viewModel);
        }

        // Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await dbContext.Users
                    .FirstOrDefaultAsync(u => u.Username == viewModel.Username && u.Password == viewModel.Password);
                if (user != null)
                {
                    SetLoggedInUser(user.Username);
                    return RedirectToAction("List", "Logs");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid username or password.");
                    return View(viewModel);
                }
            }
            return View(viewModel);
        }

        // Logout
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("CurrentUser");
            return RedirectToAction("Login");
        }
    }
}
