using ExpenseBudget.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseBudget.Data
{
    public class ExpenseBudgetContext:DbContext
    {
       
        public ExpenseBudgetContext(DbContextOptions<ExpenseBudgetContext> options)
            : base(options)
        {
        }

        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Budget> Budgets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Expense>()
                .HasIndex(e => e.UserId);

            modelBuilder.Entity<Budget>()
                .HasIndex(b => b.UserId);

            modelBuilder.Entity<Budget>()
                .HasIndex(b => new { b.UserId, b.Category })
                .IsUnique();
        }
    }
}

