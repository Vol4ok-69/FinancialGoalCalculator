using System;
using System.Collections.Generic;

namespace Finance.Models;

public partial class GoalPlan
{
    public int Id { get; set; }

    public int FinancialGoalId { get; set; }

    public int FrequencyId { get; set; }

    public decimal PlannedAmount { get; set; }

    public virtual FinancialGoal FinancialGoal { get; set; } = null!;

    public virtual ContributionFrequency Frequency { get; set; } = null!;
}
