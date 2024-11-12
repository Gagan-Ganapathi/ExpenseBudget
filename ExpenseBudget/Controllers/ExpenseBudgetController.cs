using ExpenseBudget.Models.DTO;
using ExpenseBudget.Models;
using ExpenseBudget.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseBudget.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseBudgetController : ControllerBase
    {
        private readonly IExpenseBudgetService _service;
        private readonly ILogger<ExpenseBudgetController> _logger;

        public ExpenseBudgetController(
            IExpenseBudgetService service,
            ILogger<ExpenseBudgetController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("expenses/{userId}")]
        public async Task<ActionResult<IEnumerable<Expense>>> GetUserExpenses(string userId)
        {
            var expenses = await _service.GetUserExpensesAsync(userId);
            return Ok(expenses);
        }

        [HttpGet("budgets/{userId}")]
        public async Task<ActionResult<IEnumerable<Budget>>> GetUserBudgets(string userId)
        {
            var budgets = await _service.GetUserBudgetsAsync(userId);
            return Ok(budgets);
        }

        [HttpGet("analysis/{userId}/{category}")]
        public async Task<ActionResult<ExpenseSummaryDTO>> GetCategoryAnalysis(
            string userId, string category)
        {
            var analysis = await _service.GetCategoryAnalysisAsync(userId, category);
            return Ok(analysis);
        }

        [HttpPost("expenses")]
        public async Task<ActionResult<Expense>> AddExpense(Expense expense)
        {
            try
            {
                var result = await _service.AddExpenseAsync(expense);
                return CreatedAtAction(nameof(GetUserExpenses),new { userId = expense.UserId },result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding expense");
                return StatusCode(500, "An error occurred while adding the expense");
            }
        }

       [HttpPost("budgets")]
        public async Task<ActionResult<Budget>> SetBudget(Budget budget)
        {
            try
            {
                var result = await _service.SetBudgetAsync(budget);
                return CreatedAtAction(
                    nameof(GetUserBudgets),
                    new { userId = budget.UserId },
                    result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting budget");
                return StatusCode(500, "An error occurred while setting the budget");
            }
        }

        [HttpGet("summary/{userId}")]
        public async Task<ActionResult<IEnumerable<ExpenseSummaryDTO>>> GetMonthlySummary(
            string userId)
        {
            var summary = await _service.GetMonthlySummaryAsync(userId);
            return Ok(summary);
        }
       
        [HttpPost("income")]
        public async Task<OkResult> UpdateIncome(MonthlyIncomeDto incomeDto)
        {
            await _service.UpdateMonthlyIncome(
                incomeDto.UserId,
                incomeDto.TotalMonthlyIncome);
            return Ok();
        }
    }
}
