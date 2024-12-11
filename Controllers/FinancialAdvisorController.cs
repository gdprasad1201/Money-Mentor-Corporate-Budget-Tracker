using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Expense_Tracker.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Expense_Tracker.Models;

namespace Expense_Tracker.Controllers
{
    [Authorize(Roles = "Admin, User")] // Restrict access to Admin and User roles
    public class FinancialAdvisorController : Controller
    {
        private readonly OpenAIService _openAIService;
        private readonly UserManager<ApplicationUser> _userManager;

        public FinancialAdvisorController(OpenAIService openAIService, UserManager<ApplicationUser> userManager)
        {
            _openAIService = openAIService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Fetch the current logged-in user
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                ViewBag.FirstName = user.FirstName; // Assign user's first name to ViewBag
                ViewBag.LastName = user.LastName;   // Assign user's last name to ViewBag
            }
            else
            {
                ViewBag.FirstName = "Unknown"; // Fallback value
                ViewBag.LastName = "User";
            }

            return View();
        }

        [HttpPost("FinancialAdvisor/GetAdvice")]
        public async Task<IActionResult> GetAdvice([FromBody] UserPrompt prompt)
        {
            if (prompt == null || string.IsNullOrEmpty(prompt.Prompt))
            {
                return BadRequest("Prompt cannot be empty.");
            }

            try
            {
                var advice = await _openAIService.GetResponse(prompt.Prompt);
                return Ok(new { advice });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }

    public class UserPrompt
    {
        public string Prompt { get; set; }
    }
}
