using IncomeMicroservice.Models;

namespace IncomeMicroservice.Services
{
    public interface IIncomeService
    {

        Task<Income> CreateIncome(Income income);
        Task<Income> GetIncomeById(int id);
        Task<IEnumerable<Income>> GetIncomesByUserId(string userId);
        Task<Income> UpdateIncome(Income income);
        Task<bool> DeleteIncome(int id);
    }
}
