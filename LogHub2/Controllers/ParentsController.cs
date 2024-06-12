using LogHub2.Data;
using LogHub2.Models;
using LogHub2.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LogHub2.Controllers
{
    public class ParentsController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public ParentsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Add()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("CurrentParent")))
            {
                return RedirectToAction("Login", "Users");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddParentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {

                string username = HttpContext.Session.GetString("CurrentParent");
                if (username == null)
                {
                    return RedirectToAction("Login", "Parents");
                }

                var log = new Parent
                (
                    viewModel.Name,
                    viewModel.Type,
                    Convert.ToInt32(HttpContext.Session.GetString("CurrentParent"))
                );

                await dbContext.Parents.AddAsync(log);
                await dbContext.SaveChangesAsync();

                return RedirectToAction("List", "Parents");
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> UserLogs()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("CurrentParent")))
            {
                return RedirectToAction("Login", "Users");
            }
            int userId = Convert.ToInt32(HttpContext.Session.GetString("CurrentParent"));
            //var parents = await dbContext.Parents.Where(p => p.userId == userId).ToListAsync();

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> List(string type)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("CurrentParent")))
            {
                return RedirectToAction("Login", "Users");
            }
            IQueryable<Parent> parentsQuery = dbContext.Parents;


            if (!string.IsNullOrEmpty(type))
            {
                parentsQuery = parentsQuery.Where(l => l.Type == type);
            }

            var parents = await parentsQuery.ToListAsync();
            return View(parents);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {

            var parent = await dbContext.Parents.FirstOrDefaultAsync(l => l.Id == id);
            if (parent == null)
            {
                return NotFound();
            }

            dbContext.Parents.Remove(parent);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("List");
        }
    }
}

