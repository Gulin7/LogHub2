using LogHub2.Data;
using LogHub2.Models;
using LogHub2.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LogHub2.Controllers
{
    public class LogsController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public LogsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Add()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("CurrentUser")))
            {
                return RedirectToAction("Login", "Users");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddLogViewModel viewModel)
        {
            if (ModelState.IsValid)
            {

                string username = HttpContext.Session.GetString("CurrentUser");
                if (username == null)
                {
                    return RedirectToAction("Login", "Users");
                }

                var log = new Log
                {
                    Type = viewModel.Type,
                    Severity = viewModel.Severity,
                    Username = username,
                    Message = viewModel.Message,
                };

                await dbContext.Logs.AddAsync(log);
                await dbContext.SaveChangesAsync();

                return RedirectToAction("List", "Logs");
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> UserLogs()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("CurrentUser")))
            {
                return RedirectToAction("Login", "Users");
            }
            string username = HttpContext.Session.GetString("CurrentUser");
            var logs = await dbContext.Logs.Where(l => l.Username == username).ToListAsync();

            return View(logs);
        }

        [HttpGet]
        public async Task<IActionResult> List(string severity, string type)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("CurrentUser")))
            {
                return RedirectToAction("Login", "Users");
            }
            IQueryable<Log> logsQuery = dbContext.Logs;

            if (!string.IsNullOrEmpty(severity))
            {
                logsQuery = logsQuery.Where(l => l.Severity == severity);
            }

            if (!string.IsNullOrEmpty(type))
            {
                logsQuery = logsQuery.Where(l => l.Type == type);
            }

            var logs = await logsQuery.ToListAsync();
            return View(logs);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {

            var log = await dbContext.Logs.FirstOrDefaultAsync(l => l.Id == id);
            if (log == null)
            {
                return NotFound();
            }

            dbContext.Logs.Remove(log);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("List");
        }
    }
}

