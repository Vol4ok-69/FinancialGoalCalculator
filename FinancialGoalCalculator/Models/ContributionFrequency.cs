using System;
using System.Collections.Generic;

namespace Finance.Models;

public partial class ContributionFrequency
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<GoalPlan> GoalPlans { get; set; } = [];
}
