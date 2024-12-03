using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using Expense_Tracker.Models;

namespace Expense_Tracker.Controllers
{
    //[Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // Display user profile
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var model = new UserProfileViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName, // Ensure FirstName is included
                Email = user.Email,
                ProfilePictureUrl = user.ProfilePictureUrl ?? "https://i.natgeofe.com/n/548467d8-c5f1-4551-9f58-6817a8d2c45e/NationalGeographic_2572187_2x1.jpg" // Default image URL
            };

            return View(model);
        }

        // Dashboard action method
        public async Task<IActionResult> Dashboard()
        {
            var user = await _userManager.GetUserAsync(User);  // Fetch the user from the identity
            if (user == null)
            {
                return NotFound();
            }

            // Make sure to pass the user data (like FirstName and UserName) to the view model
            var model = new UserProfileViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,  // Ensure FirstName is set here
                Email = user.Email,
                ProfilePictureUrl = user.ProfilePictureUrl ?? "https://i.natgeofe.com/n/548467d8-c5f1-4551-9f58-6817a8d2c45e/NationalGeographic_2572187_2x1.jpg" // Default picture URL
            };

            return View(model); // Pass the model to the view
        }



        // Edit user profile methods
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            // Get the current logged-in user
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account"); // Or wherever you'd like to redirect if the user is not found
            }

            // Store the first name and last name in the ViewBag for easy access in the view
            ViewBag.FirstName = user.FirstName;
            ViewBag.LastName = user.LastName;
            ViewBag.ProfilePictureUrl = user.ProfilePictureUrl;
            // Prepare the view model
            var model = new EditProfileViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                ProfilePictureUrl = user.ProfilePictureUrl, // Assuming this is a valid URL to the profile picture
                FirstName = user.FirstName, // Pass first name (for form population)
                LastName = user.LastName  // Pass last name (for form population)
            };

            return View(model);
        }



        // Edit user profile - POST method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProfileViewModel model, IFormFile ProfilePicture)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                user.UserName = model.UserName;
                user.Email = model.Email;

                // Handle profile picture upload
                if (ProfilePicture != null && ProfilePicture.Length > 0)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", ProfilePicture.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ProfilePicture.CopyToAsync(stream);
                    }

                    user.ProfilePictureUrl = $"/images/{ProfilePicture.FileName}"; // Save the path to the database
                }

                await _userManager.UpdateAsync(user);

                return RedirectToAction("Profile");
            }

            return View(model);
        }

        // Index action - retrieves user info and redirects to Profile
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                // Pass ProfilePictureUrl and FirstName to the view
                ViewBag.FirstName = user.FirstName;
                ViewBag.LastName = user.LastName;
                ViewBag.ProfilePictureUrl = user.ProfilePictureUrl;
            }

            return View();
        }
    }
}
