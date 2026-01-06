using BCrypt.Net;
using Library_Management_System.Common;
using Library_Management_System.Models;
using Library_Management_System.Services.Interfaces;
using System;

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
            var user = _context.Members
                .FirstOrDefault(u => u.Email == email);
            if (user == null) return null;
            bool valid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (valid == false) return null;
            return new UserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                Role = user.RoleId == 1 ? "Admin" : "Member"
            };
        }
    }
}
