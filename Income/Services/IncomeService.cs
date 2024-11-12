using IncomeMicroservice.Data;
using IncomeMicroservice.Models;
using Microsoft.EntityFrameworkCore;

namespace IncomeMicroservice.Services
{
    public class IncomeService : IIncomeService
    {
        private readonly IncomeDbContext _db;

        public IncomeService(IncomeDbContext db)
        {
            _db = db;
        }

        public async Task<Income> CreateIncome(Income income)
        {
            _db.Incomes.Add(income);
            await _db.SaveChangesAsync();
            return income;
        }

        public async Task<Income> GetIncomeById(int id)
        {
            return await _db.Incomes.FindAsync(id);
        }

        public async Task<IEnumerable<Income>> GetIncomesByUserId(string userId)
        {
            return await _db.Incomes
                .Where(i => i.UserId == userId)
                .OrderByDescending(i => i.Date)
                .ToListAsync();
        }

        public async Task<Income> UpdateIncome(Income income)
        {
            _db.Incomes.Update(income);
            await _db.SaveChangesAsync();
            return income;
        }

        public async Task<bool> DeleteIncome(int id)
        {
            var income = await _db.Incomes.FindAsync(id);
            if (income == null) return false;

            _db.Incomes.Remove(income);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
