
using SavingsInvestment.Services.Interface;
using SavingsInvestment.Services;
using Microsoft.EntityFrameworkCore;
using SavingsInvestment.Data;

namespace SavingsInvestment
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<SavingsInvestmentContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            // Add HTTP Clients
            builder.Services.AddHttpClient<IExpenseBudgetService, ExpenseBudgetServiceClient>();
            builder.Services.AddHttpClient<IInvestmentMarketDataService, InvestmentMarketDataService>();
            builder.Services.AddHttpClient<INotificationService, NotificationService>();
            builder.Services.AddHttpClient<ISavingsInvestmentService, SavingsInvestmentService>();

            // Register services
            //builder.Services.AddScoped<IExpenseBudgetService, ExpenseBudgetServiceClient>();
            //builder.Services.AddScoped<IInvestmentMarketDataService, InvestmentMarketDataService>();
            //builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<ISavingsInvestmentService, SavingsInvestmentService>();


            builder.Services.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
                logging.AddDebug();
            });


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseRouting();
           

            app.MapControllers();

            app.Run();
        }
    }
}
