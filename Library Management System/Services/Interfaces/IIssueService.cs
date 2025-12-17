using Library_Management_System.Models;

namespace Library_Management_System.Services.Interfaces
{
    public interface IIssueService
    {
        Task<List<IssuedBook>> GetAllIssuedBooksAsync();
        Task<bool> IssueBookAsync(int bookId, int memberId);
        Task<bool> ReturnBookAsync(int issuedBookId);
    }
}
