using Finance.Models;

namespace Finance.Interfaces;

public interface IFinancialGoalService
{
    void CreateGoal(
         string name,
    string description,
    decimal targetAmount,
    decimal initialAmount,
    DateTime? deadline,
    int priorityId,
    int categoryId,
    int frequencyId,
    decimal plannedAmount
    );

    List<FinancialGoal> LoadAllGoals();
    FinancialGoal GetGoalWithDetails(int goalId);

    void AddContribution(int goalId, decimal amount);
    int ChangeStatus(int goalId, int newStatusId);

    void DeleteGoal(int goalId);
    void UpdateGoalStatuses();
    void UpdateGoal(
    int goalId,
    string name,
    string description,
    decimal targetAmount,
    decimal currentAmount,
    DateTime? deadline);
}
