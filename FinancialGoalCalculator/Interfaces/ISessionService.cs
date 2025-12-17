using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Interfaces
{
    public interface ISessionService
    {
        int CurrentUserId { get; }
    }
}
