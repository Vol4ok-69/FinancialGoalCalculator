using System;
using System.Collections.Generic;

namespace Finance.Models;

public partial class GoalCategory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<FinancialGoal> FinancialGoals { get; set; } = [];
}
