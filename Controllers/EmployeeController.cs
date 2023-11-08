using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PayrollWebApp.Models;

namespace PayrollWebApp.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDBContext context;

        public EmployeeController(ApplicationDBContext context)
        {
            this.context = context;
        }

        // GET: EmployeeController
        public ActionResult Index()
        {
            var data = context.Employees.ToList();
            foreach (var obj in data)
            {
                obj.Designations = context.Designations.FirstOrDefault
                    (u => u.DesignationId == obj.DesignationId);
            }
            return View(data);
        }

        // GET: EmployeeController/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null || context.Employees == null)
            {
                return NotFound();
            }

            var data = context.Employees
                .FirstOrDefault(m => m.EmployeeId == id);
            if (data == null)
            {
                return NotFound();
            }

            var desigID = data.DesignationId;
            foreach (var obj in context.Designations.ToList())
            {
                if (obj.DesignationId == desigID)
                {
                    ViewBag.data = obj.DesignationType;
                }
            }
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
                return RedirectToAction(nameof(Index));
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
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EmployeeController/Delete/5
        public ActionResult Delete(int id)
        {
            var data = context.Employees.Find(id);

            if (data == null)
            {
                return NotFound();
            }
            ViewBag.data = "N/A";
            var desigID = data.DesignationId;
            foreach (var obj in context.Designations.ToList())
            {
                if (obj.DesignationId == desigID)
                {
                    ViewBag.data = obj.DesignationType;
                }
            }

            return View(data);
        }

        // POST: EmployeeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, EmployeeEntity employee)
        {
            var emp = context.Employees.Find(id);
            var data = context.Payrolls.FirstOrDefault(x => x.EmployeeId == id);

            if (emp != null)
            {
                context.Payrolls.Remove(data);
                context.SaveChanges();
                context.Employees.Remove(emp);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View();

        }

        [HttpGet]
        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(string Email, string Password)
        {
            ViewBag.LogInStatus = "";
            
                var User = context.Employees.FirstOrDefault
                    (x => x.Email ==  Email && x.Password == Password);
                if (User == null)
                {
                    ViewBag.LoginStatus = "Login Failed. Please re-enter Email and Password Correctly";

                }
                else
                {
                    if(User.Role == "User")
                    {
                        return RedirectToAction("Index", "User", new { id = User.EmployeeId});
                    }
                    else
                    {
                        return RedirectToAction("Index", "Admin", new { id = User.EmployeeId });
                    }
                }
            return View();
        }

        public ActionResult LogOut()
        {
            return RedirectToAction("LogIn");
        }

        public ActionResult UserProfile(int? id)
        {
            var data = context.Employees.Find(id);
            if(data.DesignationId != null) 
            {
                data.Designations = context.Designations.Find(data.DesignationId);
            }
            return View(data);
        }
    }
}