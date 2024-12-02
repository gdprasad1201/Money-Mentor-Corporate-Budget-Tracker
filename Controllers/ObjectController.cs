using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Expense_Tracker.Models;
using System.Linq;

namespace Expense_Tracker.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ObjectController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index() => View(_context.ExampleObjects.ToList());

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(ExampleObject obj)
        {
            if (ModelState.IsValid)
            {
                _context.ExampleObjects.Add(obj);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        [HttpGet]
        public IActionResult Edit(int id) => View(_context.ExampleObjects.Find(id));

        [HttpPost]
        public IActionResult Edit(ExampleObject obj)
        {
            if (ModelState.IsValid)
            {
                _context.ExampleObjects.Update(obj);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Delete(int id)
        {
            var obj = _context.ExampleObjects.Find(id);
            if (obj != null)
            {
                _context.ExampleObjects.Remove(obj);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
