using Library_Management_System.Models;

namespace Library_Management_System.Services.Interfaces
{
    public interface IBookServices
    {
        Task<List<Book>> GetAllAsync();
        Task<Book?> GetByIdAsync(int id);
        Task AddAsync(Book book);
        Task UpdateAsync(Book book);
        Task SoftDeleteAsync(int id);
    }
}
