using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PayrollWebApp.Models;

namespace PayrollWebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDBContext context;

        public UserController(ApplicationDBContext context)
        {
            this.context = context;
        }
        // GET: UserController
        public ActionResult Index(int? id)
        {
            var data = context.Employees.Find(id);
            return View(data);
        }
        public IActionResult Privacy(int? id)
        {
            var data = context.Employees.Find(id);
            return View(data);
        }

        // GET: EmployeeController/Create
        public ActionResult Create()
        {
            IEnumerable<SelectListItem> DesignationTypes =
                    context.Designations.Select(x => new SelectListItem
                    {
                        Text = x.DesignationType,
                        Value = x.DesignationId.ToString()
                    });
            ViewBag.Designations = DesignationTypes;

            List<SelectListItem> Gender = new()
            {
                new SelectListItem{Value="Male",Text="Male"},
                new SelectListItem{Value="Female",Text="Female"}
            };
            ViewBag.gender = Gender;

            List<SelectListItem> Role = new()
            {
                new SelectListItem{Value="Admin",Text="Admin"},
                new SelectListItem{Value="User",Text="User"}
            };
            ViewBag.role = Role;

            return View();
        }

        // POST: EmployeeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EmployeeEntity collection)
        {

            if (ModelState.IsValid)
            {
                context.Employees.Add(collection);
                context.SaveChanges();
                return RedirectToAction("LogIn","Employee");
            }
            return View();
        }

        // GET: EmployeeController/Edit/5
        public ActionResult Edit(int? id)
        {
            List<SelectListItem> Gender = new()
            {
                new SelectListItem{Value="Male",Text="Male"},
                new SelectListItem{Value="Female",Text="Female"}
            };
            ViewBag.gender = Gender;

            List<SelectListItem> Role = new()
            {
                new SelectListItem{Value="Admin",Text="Admin"},
                new SelectListItem{Value="User",Text="User"}
            };
            ViewBag.role = Role;

            IEnumerable<SelectListItem> DesignationTypes =
                    context.Designations.Select(x => new SelectListItem
                    {
                        Text = x.DesignationType,
                        Value = x.DesignationId.ToString()
                    });
            ViewBag.Designations = DesignationTypes;

            var data = context.Employees.Find(id);
            data.Designations = context.Designations.FirstOrDefault(x => x.DesignationId == data.DesignationId);

            if (data == null)
            {
                return NotFound();
            }

            return View(data);
        }

        // POST: EmployeeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EmployeeEntity collection)
        {
            try
            {
                context.Employees.Update(collection);
                context.SaveChanges();
                return RedirectToAction("UserProfile", "Employee", new { id = collection.EmployeeId });
            }
            catch
            {
                return View();
            }
        }
    }
}
