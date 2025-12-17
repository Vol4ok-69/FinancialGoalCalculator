using Finance.Interfaces;
using Finance.Models;
using Microsoft.EntityFrameworkCore;

namespace Finance.Services
{
    public class UserService : IUserService
    {

        private readonly DataBaseContext _db;
        public UserService(DataBaseContext db)
        {
            ArgumentNullException.ThrowIfNull(db);
            _db = db;
        }
        public User Authorization(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Заполните все поля!");
            }
            User? user = _db.Users.FirstOrDefault(u => u.Username == login);
            if (user != null)
            {
                if (BCrypt.Net.BCrypt.Verify(password, user.HashPassword))
                {
                    SessionManager.StartSession(user);
                    return user;
                }
                else
                {
                    throw new InvalidOperationException("Неверный логин или пароль");
                }
            }
            else
            {
                throw new KeyNotFoundException("Учетной записи не существует");
            }
        }
        public User Registration(string login, string password, string verifyPassword)
        {
            if (_db.Users.Any(u => u.Username == login))
            {
                throw new InvalidOperationException("Логин занят");
            }
            if (password != verifyPassword)
            {
                throw new InvalidOperationException("Пароли не совпадают");
            }
            string hashPassword = BCrypt.Net.BCrypt.HashPassword(password);
            return SaveUser(login, hashPassword);

        }
        private User SaveUser(string login, string password)
        {
            try
            {
                var user = new User()
                {
                    Username = login,
                    HashPassword = password
                };
                _db.Users.Add(user);
                _db.SaveChanges();
                SessionManager.StartSession(user);
                return user;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Ошибка создания учетной записи", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Непредвиденная ошибка при создании учетной записи.", ex);
            }
        }
    }
}
