using Finance.Interfaces;
using Finance.Models;
using Microsoft.EntityFrameworkCore;

namespace Finance.Services;

public class FinancialGoalService : IFinancialGoalService
{
    private readonly DataBaseContext _db;
    private readonly ISessionService _sessionService;

    public FinancialGoalService(DataBaseContext db, ISessionService sessionService)
    {
        ArgumentNullException.ThrowIfNull(db);
        _db = db;
        _sessionService = sessionService;
    }

    public void CreateGoal(
        string name,
        string description,
        decimal targetAmount,
        decimal initialAmount,
        DateTime? deadline,
        int priorityId,
        int categoryId,
        int frequencyId,
        decimal plannedAmount)
    {
        using var transaction = _db.Database.BeginTransaction();

        var activeStatus = _db.GoalStatuses.FirstOrDefault(s => s.Name == "Активна")
            ?? _db.GoalStatuses.FirstOrDefault(s => s.Id == 1);

        var goal = new FinancialGoal
        {
            Name = name,
            Description = description,
            TargetAmount = targetAmount,
            CurrentAmount = initialAmount,
            CreatedAt = DateTime.Now,
            Deadline = deadline,
            UserId = _sessionService.CurrentUserId,
            PriorityId = priorityId,
            CategoryId = categoryId,
            StatusId = activeStatus.Id
        };

        _db.FinancialGoals.Add(goal);

        var plan = new GoalPlan
        {
            FinancialGoal = goal,
            FrequencyId = frequencyId,
            PlannedAmount = plannedAmount
        };

        try
        {
            _db.GoalPlans.Add(plan);
            _db.SaveChanges();
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw new Exception("Ошибка при создании финансовой цели", ex);
        }
    }
    public List<FinancialGoal> LoadAllGoals()
    {
        return [.. _db.FinancialGoals
            .Include(g => g.Priority)
            .Include(g => g.Status)
            .Include(g => g.Category)
            .Include(g => g.GoalPlans)
                .ThenInclude(p => p.Frequency)
            .Where(g => g.UserId == _sessionService.CurrentUserId)
            .OrderByDescending(g => g.CreatedAt)];
    }

    public FinancialGoal GetGoalWithDetails(int goalId)
    {
        return _db.FinancialGoals
            .Include(g => g.Priority)
            .Include(g => g.Status)
            .Include(g => g.Category)
            .Include(g => g.GoalPlans)
                .ThenInclude(p => p.Frequency)
            .Include(g => g.GoalContributions)
            .FirstOrDefault(g => g.Id == goalId && g.UserId == _sessionService.CurrentUserId)
            ?? throw new Exception("Финансовая цель не найдена");
    }

    public void AddContribution(int goalId, decimal amount)
    {
        using var transaction = _db.Database.BeginTransaction();

        var goal = _db.FinancialGoals
            .Include(g => g.GoalContributions)
            .FirstOrDefault(g => g.Id == goalId && g.UserId == _sessionService.CurrentUserId) ?? throw new Exception("Цель не найдена");
        var contribution = new GoalContribution
        {
            FinancialGoalId = goalId,
            Amount = amount,
            Date = DateTime.Now
        };

        goal.CurrentAmount += amount;

        if (goal.CurrentAmount >= goal.TargetAmount)
        {
            var completedStatus = _db.GoalStatuses.FirstOrDefault(s => s.Name == "Достигнута");
            if (completedStatus != null)
                goal.StatusId = completedStatus.Id;
        }

        try
        {
            _db.GoalGoalContributions.Add(contribution);
            _db.SaveChanges();
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw new Exception("Ошибка при добавлении взноса", ex);
        }
    }

    public int ChangeStatus(int goalId, int newStatusId)
    {
        var goal = _db.FinancialGoals
            .FirstOrDefault(g => g.Id == goalId && g.UserId == _sessionService.CurrentUserId) ?? throw new Exception("Цель не найдена");
        goal.StatusId = newStatusId;

        try
        {
            _db.SaveChanges();
            return newStatusId;
        }
        catch (Exception ex)
        {
            throw new Exception("Ошибка при изменении статуса цели", ex);
        }
    }

    public void DeleteGoal(int goalId)
    {
        using var transaction = _db.Database.BeginTransaction();

        var goal = _db.FinancialGoals
            .Include(g => g.GoalPlans)
            .Include(g => g.GoalContributions)
            .FirstOrDefault(g => g.Id == goalId && g.UserId == _sessionService.CurrentUserId);

        if (goal == null)
            return;

        _db.GoalPlans.RemoveRange(goal.GoalPlans);
        _db.GoalGoalContributions.RemoveRange(goal.GoalContributions);
        _db.FinancialGoals.Remove(goal);

        try
        {
            _db.SaveChanges();
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw new Exception("Ошибка при удалении цели", ex);
        }
    }

    public void UpdateGoalStatuses()
    {
        var failedStatus = _db.GoalStatuses.FirstOrDefault(s => s.Name == "Провалена");
        var activeStatus = _db.GoalStatuses.FirstOrDefault(s => s.Name == "Активна");

        if (failedStatus == null || activeStatus == null)
            return;

        var now = DateTime.Now;

        var expiredGoals = _db.FinancialGoals
            .Where(g =>
                g.UserId == _sessionService.CurrentUserId &&
                g.Deadline != null &&
                g.Deadline < now &&
                g.StatusId == activeStatus.Id)
            .ToList();

        foreach (var goal in expiredGoals)
        {
            goal.StatusId = failedStatus.Id;
        }

        _db.SaveChanges();
    }

    public void UpdateGoal(
        int goalId,
        string name,
        string description,
        decimal targetAmount,
        decimal currentAmount,
        DateTime? deadline)
    {
        var goal = _db.FinancialGoals
            .FirstOrDefault(g => g.Id == goalId && g.UserId == _sessionService.CurrentUserId)
            ?? throw new Exception("Цель не найдена");

        goal.Name = name;
        goal.Description = description;
        goal.TargetAmount = targetAmount;
        goal.CurrentAmount = currentAmount;
        goal.Deadline = deadline;

        var completed = _db.GoalStatuses.FirstOrDefault(s => s.Name == "Достигнута");
        var active = _db.GoalStatuses.FirstOrDefault(s => s.Name == "Активна");

        if (completed != null && active != null)
        {
            goal.StatusId = goal.CurrentAmount >= goal.TargetAmount
                ? completed.Id
                : active.Id;
        }

        _db.SaveChanges();
    }
}
