using Finance.Interfaces;
using Finance.Models;
using Finance.Pages;
using Finance.Services;
using Finance.UserControls;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
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
            services.AddDbContext<DataBaseContext>(options =>
            {
                options.UseNpgsql("Host=localhost;Port=5432;Database=FinancialGoals;Username=postgres;Password=Rntv1103");
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