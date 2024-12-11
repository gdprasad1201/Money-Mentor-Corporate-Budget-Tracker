using Microsoft.AspNetCore.Mvc;
using Expense_Tracker.Services;

namespace Expense_Tracker.Controllers
{
    public class FinancialAdvisorController : Controller
    {
        private readonly OpenAIService _openAIService;

        public FinancialAdvisorController(OpenAIService openAIService)
        {
            _openAIService = openAIService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
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
