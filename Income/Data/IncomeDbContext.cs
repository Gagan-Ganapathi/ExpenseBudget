using IncomeMicroservice.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace IncomeMicroservice.Data
{
    public class IncomeDbContext : DbContext
    {
        public IncomeDbContext(DbContextOptions<IncomeDbContext> options) : base(options)
        {
        }

        public DbSet<Income> Incomes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Income>()
                .Property(i => i.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Income>()
                .Property(i => i.Date)
                .HasColumnType("datetime2");
        }

    }
}
