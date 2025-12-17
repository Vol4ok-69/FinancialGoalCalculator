using Finance.Models;

namespace Finance.Services
{
    public static class SessionManager
    {
        public static int CurrentUserId { get; private set; }
        public static void StartSession(User user)
        {
            CurrentUserId = user.Id;
        }
        public static void StopSession()
        {
            CurrentUserId = 0;
        }
    }
}
