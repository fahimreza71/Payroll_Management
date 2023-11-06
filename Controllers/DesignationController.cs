using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayrollWebApp.Models;

namespace PayrollWebApp.Controllers
{
    public class DesignationController : Controller
    {
        private readonly ApplicationDBContext context;

        public DesignationController(ApplicationDBContext context)
        {
            this.context = context;
        }
        public ActionResult Index()
        {
            var data = context.Designations.ToList();
            return View(data);
        }
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null || context.Designations == null)
            {
                return NotFound();
            }

            var data = await context.Designations
                .FirstOrDefaultAsync(m => m.DesignationId == id);
            if (data == null)
            {
                return NotFound();
            }
            return View(data);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(DesignationEntity input)
        {
            try
            {
                await context.Designations.AddAsync(input);
                await context.SaveChangesAsync();
                TempData["msg"] = "Designation Created";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public async Task<ActionResult> Edit(int? id)
        {
            var data = await context.Designations.FindAsync(id);
            if (data == null)
            {
                return NotFound();
            }
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, DesignationEntity collection)
        {
            try
            {
                context.Designations.Update(collection);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }

        }

        //public async Task<ActionResult> Edit(DesignationEntity designation)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        context.Designations.Update(designation);
        //        await context.SaveChangesAsync();
        //        TempData["msg"] = "Designation Updated";
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View();
        //}

        public ActionResult Delete(int? id)
        {
            var details = context.Designations.Find(id);
            return View(details);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(DesignationEntity designation)
        {
            try
            {
                context.Designations.Remove(designation);
                await context.SaveChangesAsync();
                TempData["msg"] = "Designation Deleted";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
