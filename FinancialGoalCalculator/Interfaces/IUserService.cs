using Finance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Interfaces
{
    public interface IUserService
    {
        User Authorization(string login, string password);
        User Registration(string login, string password, string verifyPassword);
    }
}
