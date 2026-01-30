using BCrypt.Net;
using Library_Management_System.Common;
using Library_Management_System.Models;
using Library_Management_System.Services.Interfaces;


namespace Library_Management_System.Services
{
    public class AccountService : IAccountService
    {
        private readonly LibraryManagementContext _context;

        public AccountService(LibraryManagementContext context)
        {
            _context = context;
        }

        public UserViewModel? ValidateUser(string email, string password)
        {
            var user = _context.Members.FirstOrDefault(u => u.Email == email && u.PasswordHash == password);
            if (user == null) return null;
            return new UserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.RoleId == 1 ? "Admin" : "Member",
                TwoFactorEnabled = user.TwoFactorEnabled,
                TwoFactorSecret = user.TwoFactorSecret
            };

        }
    }
}