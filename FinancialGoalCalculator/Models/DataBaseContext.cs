using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Finance.Models;

public partial class DataBaseContext : DbContext
{
    public DataBaseContext()
    {
    }

    public DataBaseContext(DbContextOptions<DataBaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ContributionFrequency> ContributionFrequencies { get; set; }

    public virtual DbSet<FinancialGoal> FinancialGoals { get; set; }

    public virtual DbSet<GoalCategory> GoalCategories { get; set; }

    public virtual DbSet<GoalContribution> GoalGoalContributions { get; set; }

    public virtual DbSet<GoalPlan> GoalPlans { get; set; }

    public virtual DbSet<GoalStatus> GoalStatuses { get; set; }

    public virtual DbSet<Priority> Priorities { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var basePath = AppDomain.CurrentDomain.BaseDirectory;
        var configPath = Path.Combine(basePath, "appsettings.json");

        if (!File.Exists(configPath))
            throw new FileNotFoundException("Файл appsettings.json не найден", configPath);

        string jsonString = File.ReadAllText(configPath);

        string? connString = null;
        using (JsonDocument doc = JsonDocument.Parse(jsonString))
        {
            if (doc.RootElement.TryGetProperty("ConnectionString", out JsonElement element))
            {
                connString = element.GetString();
            }
        }

        if (string.IsNullOrWhiteSpace(connString))
            throw new Exception("ConnectionString не найден в appsettings.json");

        optionsBuilder.UseNpgsql(connString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ContributionFrequency>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<FinancialGoal>(entity =>
        {
            entity.HasIndex(e => e.CategoryId, "IX_FinancialGoals_CategoryId");

            entity.HasIndex(e => e.PriorityId, "IX_FinancialGoals_PriorityId");

            entity.HasIndex(e => e.StatusId, "IX_FinancialGoals_StatusId");

            entity.HasIndex(e => e.UserId, "IX_FinancialGoals_UserId");

            entity.Property(e => e.CreatedAt).HasColumnType("timestamp without time zone");
            entity.Property(e => e.CurrentAmount).HasPrecision(14, 2);
            entity.Property(e => e.Deadline).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.TargetAmount).HasPrecision(14, 2);

            entity.HasOne(d => d.Category).WithMany(p => p.FinancialGoals).HasForeignKey(d => d.CategoryId);

            entity.HasOne(d => d.Priority).WithMany(p => p.FinancialGoals).HasForeignKey(d => d.PriorityId);

            entity.HasOne(d => d.Status).WithMany(p => p.FinancialGoals).HasForeignKey(d => d.StatusId);

            entity.HasOne(d => d.User).WithMany(p => p.FinancialGoals).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<GoalCategory>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<GoalContribution>(entity =>
        {
            entity.HasIndex(e => e.FinancialGoalId, "IX_GoalGoalContributions_FinancialGoalId");

            entity.Property(e => e.Amount).HasPrecision(14, 2);
            entity.Property(e => e.Date).HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.FinancialGoal).WithMany(p => p.GoalContributions).HasForeignKey(d => d.FinancialGoalId);
        });

        modelBuilder.Entity<GoalPlan>(entity =>
        {
            entity.HasIndex(e => e.FinancialGoalId, "IX_GoalPlans_FinancialGoalId");

            entity.HasIndex(e => e.FrequencyId, "IX_GoalPlans_FrequencyId");

            entity.Property(e => e.PlannedAmount).HasPrecision(14, 2);

            entity.HasOne(d => d.FinancialGoal).WithMany(p => p.GoalPlans).HasForeignKey(d => d.FinancialGoalId);

            entity.HasOne(d => d.Frequency).WithMany(p => p.GoalPlans).HasForeignKey(d => d.FrequencyId);
        });

        modelBuilder.Entity<GoalStatus>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Priority>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.HashPassword).HasMaxLength(255);
            entity.Property(e => e.Username).HasMaxLength(255);
        });
        modelBuilder.Entity<GoalCategory>().HasData(
            new GoalCategory { Id = 1, Name = "Покупки" },
            new GoalCategory { Id = 2, Name = "Сбережения" },
            new GoalCategory { Id = 3, Name = "Инвестиции" }
        );

        modelBuilder.Entity<GoalStatus>().HasData(
            new GoalStatus { Id = 1, Name = "Активна" },
            new GoalStatus { Id = 2, Name = "Достигнута" },
            new GoalStatus { Id = 3, Name = "Провалена" }
        );

        modelBuilder.Entity<Priority>().HasData(
            new Priority { Id = 1, Name = "Низкий" },
            new Priority { Id = 2, Name = "Средний" },
            new Priority { Id = 3, Name = "Высокий" }
        );

        modelBuilder.Entity<ContributionFrequency>().HasData(
            new ContributionFrequency { Id = 1, Name = "Без повторения" },
            new ContributionFrequency { Id = 2, Name = "Ежедневно" },
            new ContributionFrequency { Id = 3, Name = "Еженедельно" },
            new ContributionFrequency { Id = 4, Name = "Ежемесячно" }
        );
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
