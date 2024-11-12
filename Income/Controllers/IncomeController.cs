using IncomeMicroservice.Models.DTO;
using IncomeMicroservice.Models;
using Microsoft.AspNetCore.Mvc;
using IncomeMicroservice.Services;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using IncomeMicroservice.Utils;

namespace IncomeMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncomeController : ControllerBase
    {
        private readonly IIncomeService _incomeRepository;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public IncomeController(IIncomeService incomeRepository, HttpClient httpClient,
        IConfiguration configuration)
        {
            _incomeRepository = incomeRepository;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        //[HttpPost]
        //public async Task<ActionResult<Income>> CreateIncome(IncomeDto incomeDto)
        //{
        //    var income = new Income
        //    {
        //        UserId = incomeDto.UserId,
        //        Amount = incomeDto.Amount,
        //        Date = incomeDto.Date,
        //        Source = incomeDto.Source,
        //        Description = incomeDto.Description
        //    };

        //    var result = await _incomeRepository.CreateIncome(income);
        //    return CreatedAtAction(nameof(GetIncome), new { id = result.IncomeId }, result);
        //}

        [HttpPost]
        public async Task<ActionResult<Income>> CreateIncome(IncomeDto incomeDto)
        {
            var income = new Income
            {
                UserId = incomeDto.UserId,
                Amount = incomeDto.Amount,
                Date = incomeDto.Date,
                Source = incomeDto.Source,
                Description = incomeDto.Description
            };

            var result = await _incomeRepository.CreateIncome(income);

            // Notify BudgetExpense service
            var budgetExpenseApiUrl =
                _configuration["ServiceUrls:BudgetExpense"] +
                "/api/expensebudget/income";

            var monthlyIncome = await CalculateMonthlyIncome(income.UserId);
            var incomeUpdateDto = new MonthlyIncomeDto1
            {
                UserId = income.UserId,
                TotalMonthlyIncome = monthlyIncome,
                Month = income.Date
            };

            var content = new StringContent(
                JsonSerializer.Serialize(incomeUpdateDto),
                Encoding.UTF8,
                "application/json");

            // Fixed: Don't assign void result to variable
            await _httpClient.PostAsync(budgetExpenseApiUrl, content);

            return CreatedAtAction(
                nameof(GetIncome),
                new { id = result.IncomeId },
                result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Income>> GetIncome(int id)
        {
            var income = await _incomeRepository.GetIncomeById(id);
            if (income == null)
                return NotFound();

            return Ok(income);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Income>>> GetUserIncomes(string userId)
        {
            var incomes = await _incomeRepository.GetIncomesByUserId(userId);
            return Ok(incomes);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Income>> UpdateIncome(int id, IncomeDto incomeDto)
        {
            var existingIncome = await _incomeRepository.GetIncomeById(id);
            if (existingIncome == null)
                return NotFound();

            existingIncome.Amount = incomeDto.Amount;
            existingIncome.Date = incomeDto.Date;
            existingIncome.Source = incomeDto.Source;
            existingIncome.Description = incomeDto.Description;

            var updatedIncome = await _incomeRepository.UpdateIncome(existingIncome);
            return Ok(updatedIncome);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteIncome(int id)
        {
            var result = await _incomeRepository.DeleteIncome(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("monthly/{userId}")]
        public async Task<ActionResult<MonthlyIncomeDto1>> GetMonthlyIncome(string userId)
        {
            var currentMonth = DateTime.UtcNow.Date.StartOfMonth();
            var monthlyIncome = await _incomeRepository.GetIncomesByUserId(userId);

            var totalMonthlyIncome = monthlyIncome
                .Where(i => i.Date.Year == currentMonth.Year &&
                           i.Date.Month == currentMonth.Month)
                .Sum(i => i.Amount);

            var incomeDto = new MonthlyIncomeDto1
            {
                UserId = userId,
                TotalMonthlyIncome = totalMonthlyIncome,
                Month = currentMonth
            };

            return Ok(incomeDto);
        }


        private async Task<decimal> CalculateMonthlyIncome(string userId)
        {
            var currentMonth = DateTime.UtcNow.Date.StartOfMonth();
            var monthlyIncome = await _incomeRepository.GetIncomesByUserId(userId);

            return monthlyIncome
              .Where(i => i.Date.Year == currentMonth.Year &&
                         i.Date.Month == currentMonth.Month)
              .Sum(i => i.Amount);
        }
    }
}

