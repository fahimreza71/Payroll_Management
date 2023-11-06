using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayrollWebApp.Models;

namespace PayrollWebApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDBContext context;

        public AdminController(ApplicationDBContext context)
        {
            this.context = context;
        }
        // GET: UserController
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
