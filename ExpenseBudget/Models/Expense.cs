﻿namespace ExpenseBudget.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string PaymentMethod { get; set; }

    }
}
