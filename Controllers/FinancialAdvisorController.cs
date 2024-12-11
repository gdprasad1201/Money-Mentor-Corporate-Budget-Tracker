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
        public async Task<IActionResult> Index()
        {
            await _openAIService.GetResponse();
            return View();
        }
    }
}
