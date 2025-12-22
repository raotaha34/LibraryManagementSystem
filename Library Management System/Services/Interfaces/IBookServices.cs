using Library_Management_System.Common;
using Library_Management_System.Models;

namespace Library_Management_System.Services.Interfaces
{
    public interface IBookServices
    {
        Task<List<Book>> GetAllAsync();
        Task<PaginatedList<Book>> GetAllPaginatedAsync(int page, int pageSize);
        Task<Book?> GetByIdAsync(int id);
        Task<bool> AddAsync(Book book);
        Task UpdateAsync(Book book);
        Task SoftDeleteAsync(int id);
    }
}
