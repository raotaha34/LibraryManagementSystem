using Library_Management_System.Common;
using Library_Management_System.Models;

namespace Library_Management_System.Services.Interfaces
{
    public interface IIssueService
    {
        Task<PaginatedList<IssuedBook>> GetAllIssuedBooksAsync(int page, int pageSize);
        Task<bool> IssueBookAsync(int bookId, int memberId);
        Task<bool> ReturnBookAsync(int issuedBookId);

        Task<List<IssuedBook>> GetIssuedBooksByMemberAsync(int memberId);
    }
}


