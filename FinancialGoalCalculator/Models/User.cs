using System;
using System.Collections.Generic;

namespace Finance.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string HashPassword { get; set; } = null!;

    public virtual ICollection<FinancialGoal> FinancialGoals { get; set; } = [];
}
