using LogHub2.Data;
using LogHub2.Models;
using LogHub2.Models.Entities;
using Microsoft.AspNetCore.Http;
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
  
        protected void SetLoggedInUser(int userId)
        {
            HttpContext.Session.SetString("CurrentParent", Convert.ToString(userId));
        }

        protected string GetLoggedInUser()
        {
            return HttpContext.Session.GetString("CurrentParent");
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
                User user = new User (viewModel.Username, viewModel.Password);

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
        public async Task<IActionResult> Login(UserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await dbContext.Users
                    .FirstOrDefaultAsync(u => u.Username == viewModel.Username && u.Password == viewModel.Password);
                if (user != null)
                {
                    SetLoggedInUser(user.Id);
                    return RedirectToAction("List", "Parents");
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
            HttpContext.Session.Remove("CurrentParent");
            return RedirectToAction("Login");
        }
    }
}
