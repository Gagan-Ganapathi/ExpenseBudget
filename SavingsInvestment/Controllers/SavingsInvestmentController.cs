using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SavingsInvestment.Models;
using SavingsInvestment.Services;

namespace SavingsInvestment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SavingsInvestmentController : ControllerBase
    {
        private readonly ISavingsInvestmentService _service;

        public SavingsInvestmentController(ISavingsInvestmentService service)
        {
            _service = service;
        }

       [HttpPost("investments")]
        public async Task<IActionResult> AddInvestment(Investment investment)
        {
            var response = await _service.AddInvestmentAsync(investment);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("portfolio/{userId}")]
        public async Task<IActionResult> GetPortfolioSummary(string userId)
        {
            var response = await _service.GetPortfolioSummaryAsync(userId);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost("goals")]
        public async Task<IActionResult> SetSavingsGoal(SavingsGoal goal)
        {
            var response = await _service.SetSavingsGoalAsync(goal);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("goals/{userId}")]
        public async Task<IActionResult> GetSavingsGoals(string userId)
        {
            var response = await _service.GetSavingsGoalsAsync(userId);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
