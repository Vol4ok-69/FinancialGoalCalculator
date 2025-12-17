using Finance.Interfaces;
using Finance.Models;
using Finance.Pages;
using Finance.Services;
using Finance.UserControls;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace Finance
{
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            string? connString = "";
            string jsonString = File.ReadAllText("C:\\Users\\ivank\\Desktop\\Учёба\\C#\\FinancialGoalCalculator\\FinancialGoalCalculator\\appsettings.json");
            using (JsonDocument doc = JsonDocument.Parse(jsonString))
            {
                if (doc.RootElement.TryGetProperty("ConnectionString", out JsonElement element))
                {
                    connString = element.GetString();
                }
            }
            services.AddDbContext<DataBaseContext>(options =>
            {
                options.UseNpgsql(connString ?? throw new ArgumentException());
            }, ServiceLifetime.Singleton);
            services.AddSingleton<ISessionService, SessionAdapter>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IFinancialGoalService, FinancialGoalService>();
            services.AddTransient<Autho>();
            services.AddTransient<Registr>();
            services.AddTransient<MyGoals>();
            services.AddTransient<CreateGoal>();
            services.AddSingleton<AuthAndRegistrView>();
            services.AddSingleton<MainView>();
            services.AddSingleton<MainWindow>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}