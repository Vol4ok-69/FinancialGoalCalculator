using System;
using System.Collections.Generic;

namespace Finance.Models;

public partial class FinancialGoal
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal TargetAmount { get; set; }

    public decimal CurrentAmount { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? Deadline { get; set; }

    public int UserId { get; set; }

    public int PriorityId { get; set; }

    public int StatusId { get; set; }

    public int CategoryId { get; set; }

    public virtual GoalCategory Category { get; set; } = null!;

    public virtual ICollection<GoalContribution> GoalContributions { get; set; } = [];

    public virtual ICollection<GoalPlan> GoalPlans { get; set; } = [];

    public virtual Priority Priority { get; set; } = null!;

    public virtual GoalStatus Status { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
