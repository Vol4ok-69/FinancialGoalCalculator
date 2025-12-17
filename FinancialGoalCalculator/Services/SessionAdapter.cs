using Finance.Interfaces;

namespace Finance.Services
{
    public class SessionAdapter: ISessionService
    {
        public int CurrentUserId => SessionManager.CurrentUserId;
    }
}
