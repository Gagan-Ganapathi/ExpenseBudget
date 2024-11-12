using Microsoft.EntityFrameworkCore;
using SavingsInvestment.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace SavingsInvestment.Data
{
    public class SavingsInvestmentContext : DbContext
    {
        public SavingsInvestmentContext(DbContextOptions<SavingsInvestmentContext> options)
        : base(options) { }

        public DbSet<Investment> Investments { get; set; }
        public DbSet<InvestmentDetails> InvestmentDetails { get; set; }
        public DbSet<SavingsGoal> SavingsGoals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Investment>()
                .HasOne(i => i.Details)
                .WithOne(d => d.Investment)
                .HasForeignKey<InvestmentDetails>(d => d.InvestmentId);
        }
    }
}
