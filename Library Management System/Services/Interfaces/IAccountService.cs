using Library_Management_System.Common;
namespace Library_Management_System.Services.Interfaces
{
    public interface IAccountService
    {
        UserViewModel? ValidateUser(string email, string password);
    }
}
