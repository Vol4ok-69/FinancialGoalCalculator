using System;
using System.Collections.Generic;

namespace Finance.Models;

public partial class GoalContribution
{
    public int Id { get; set; }

    public int FinancialGoalId { get; set; }

    public decimal Amount { get; set; }

    public DateTime Date { get; set; }

    public virtual FinancialGoal FinancialGoal { get; set; } = null!;
}
