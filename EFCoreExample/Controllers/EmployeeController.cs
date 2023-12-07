using EFCoreExample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EFCoreExample.Controllers
{
    public class EmployeeController : Controller
    {
        private CompanyContext context;
        public EmployeeController(CompanyContext cc)
        {
            context = cc;
        }

        public IActionResult Index()
        {
            return View(context.Employee.Include(s => s.Department));
        }

        public IActionResult Create()
        {
            List<SelectListItem> dept = new List<SelectListItem>();
            dept = context.Department.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList();
            ViewBag.Department = dept;

            return View();
        }

        [HttpPost]
        [ActionName("Create")]
        public async Task<IActionResult> Create_Post()
        {
            /*context.Add(emp);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");*/

            var emptyEmployee = new Employee();

            if (await TryUpdateModelAsync<Employee>(emptyEmployee, "", s => s.Name, s => s.DepartmentId, s => s.Designation))
            {
                context.Employee.Add(emptyEmployee);
                await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View();
        }

        public async Task<IActionResult> Update(int id)
        {
            Employee emp = await context.Employee.Where(e => e.Id == id).FirstOrDefaultAsync();

            List<SelectListItem> dept = new List<SelectListItem>();
            dept = context.Department.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList();
            ViewBag.Department = dept;

            return View(emp);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Employee emp)
        {
            context.Update(emp);
            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var emp = new Employee() { Id = id };
            context.Remove(emp);
            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
