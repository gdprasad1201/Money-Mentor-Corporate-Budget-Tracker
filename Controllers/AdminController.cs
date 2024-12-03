using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Expense_Tracker.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Expense_Tracker.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager; // Use ApplicationUser instead of IdentityUser
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        // GET: Admin
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRoles = new List<UserRoleViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles.Add(new UserRoleViewModel
                {
                    User = user,
                    Roles = roles
                });
            }

            var model = new AdminDashboardViewModel
            {
                Users = userRoles,
                Roles = await _roleManager.Roles.ToListAsync()
            };

            return View(model);
        }

        // GET: Admin/UserList
        public async Task<IActionResult> UserList()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRoles = new List<UserRoleViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles.Add(new UserRoleViewModel
                {
                    User = user,
                    Roles = roles
                });
            }

            return View(userRoles);
        }

        // GET: Admin/AssignRole
        public async Task<IActionResult> AssignRole(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var model = new AssignRoleViewModel
            {
                UserId = userId,
                UserName = user.UserName,
                Roles = await _roleManager.Roles.ToListAsync()
            };

            return View(model);
        }

        // POST: Admin/AssignRole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRole(AssignRoleViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound();
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (model.SelectedRoles != null)
            {
                await _userManager.AddToRolesAsync(user, model.SelectedRoles);
            }

            return RedirectToAction(nameof(UserList));
        }

        // GET: Admin/ManageCategories
        public async Task<IActionResult> ManageCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            return View(categories);
        }

        // GET: Admin/CreateCategory
        public IActionResult CreateCategory()
        {
            return View();
        }

        // POST: Admin/CreateCategory
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ManageCategories));
            }

            return View(category);
        }

        // GET: Admin/EditCategory/5
        public async Task<IActionResult> EditCategory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Admin/EditCategory/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(int id, Category category)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(ManageCategories));
            }

            return View(category);
        }

        // GET: Admin/DeleteCategory/5
        public async Task<IActionResult> DeleteCategory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FirstOrDefaultAsync(m => m.CategoryId == id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Admin/DeleteCategory/5
        //Hello
        //Hello
        [HttpPost, ActionName("DeleteCategory")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategoryConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(ManageCategories));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
        }
    }

    public class UserRoleViewModel
    {
        public ApplicationUser User { get; set; } // Use ApplicationUser here instead of IdentityUser
        public IList<string> Roles { get; set; }
    }

    public class AdminDashboardViewModel
    {
        public List<UserRoleViewModel> Users { get; set; }
        public List<IdentityRole> Roles { get; set; }
    }

    public class AssignRoleViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<IdentityRole> Roles { get; set; }
        public IList<string> SelectedRoles { get; set; } // This will hold the selected roles from the view
    }
}