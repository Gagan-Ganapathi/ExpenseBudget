using ExpenseBudget.Data;
using ExpenseBudget.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace ExpenseBudget
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddLogging();

            builder.Services.AddDbContext<ExpenseBudgetContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



            // Add Services

            builder.Services.AddHttpClient();
            builder.Services.AddScoped<IExpenseBudgetService, ExpenseBudgetService>();
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials(); // If you want to allow cookies or auth credentials
                });
            });

            // Add Logging
            //builder.Services.AddLogging(logging =>
            //{
            //    logging.ClearProviders();
            //    logging.AddConsole();
            //    logging.AddDebug();
            //});

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors(); // Use default CORS policy

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }

       
    }
}