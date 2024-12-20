﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Expense_Tracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Expense_Tracker.Controllers
{
    [Authorize(Roles = "Admin, User")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CategoryController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private async Task SetUserInfo()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                ViewBag.FirstName = user.FirstName;
                ViewBag.LastName = user.LastName;
            }
        }

        // GET: Category
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            await SetUserInfo();
            if (User.Identity.IsAuthenticated)
            {
                return _context.Categories != null ?
                            View(await _context.Categories.ToListAsync()) :
                            Problem("Entity set 'ApplicationDbContext.Categories' is null.");
            }
            else
            {
                // Return an empty list or a different view for anonymous users
                return View(new List<Category>());
            }
        }

        // GET: Category/AddOrEdit
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new Category());
            else
                return View(await _context.Categories.FindAsync(id));
        }

        // POST: Category/AddOrEdit
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> AddOrEdit([Bind("CategoryId,Title,Icon,Type")] Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.CategoryId == 0)
                    _context.Add(category);
                else
                    _context.Update(category);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Categories' is null.");
            }
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}