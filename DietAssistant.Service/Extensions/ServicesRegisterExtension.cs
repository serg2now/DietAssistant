using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using DietAssistant.DAL.DataContext;
using DietAssistant.DAL.Models;
using DietAssistant.DAL.Repositories;
using DietAssistant.DAL.Repositories.Interfaces;
using DietAssistant.Services.Interfaces;
using DietAssistant.Services.DTOs;

namespace DietAssistant.Services.Extensions
{
    public static class ServicesRegisterExtension
    {
        public static void AddDietAssistantServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DietAssistantContext>(options => options.UseSqlServer(configuration.GetConnectionString("dbConnection"), sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure();
                sqlOptions.ExecutionStrategy(deps => new SqlServerRetryingExecutionStrategy(
                    dependencies: deps,
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: new int[] { 10060, 233, 40613 }
                    )
                );
            }));

            services.AddSingleton<IDietService, DietParametersService>(p => 
            {
                var limits = new DietLimits();
                configuration.GetSection("FoodLimits").Bind(limits);

                return new DietParametersService(limits);
            });

            services.AddAutoMapper(typeof(UserService).Assembly);

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IRepository<ConsumedDish>, ConsumedDishRepository>();
            services.AddScoped<IConsumedDishService, ConsumedDishService>();

            services.AddScoped<IRepository<DailyReport>, DailyReportRepository>();
            services.AddScoped<IReportService, ReportService>();

            services.AddScoped<IAdminReportService, AdminReportService>();

            services.AddTransient<IParametersCalculationService, ParametersCalculationService>();
        }
    }
}
