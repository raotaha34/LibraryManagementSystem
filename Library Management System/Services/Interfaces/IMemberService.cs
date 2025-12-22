using Library_Management_System.Common;
using Library_Management_System.Models;

namespace Library_Management_System.Services.Interfaces
{
    public interface IMemberService
    {
        Task<List<Member>> GetAllAsync();
        Task<PaginatedList<Member>> GetAllPaginatedAsync(int page, int pageSize);
        Task<Member?> GetByIdAsync(int id);
        Task<bool> AddAsync(Member member);
        Task UpdateAsync(Member member);
        Task SoftDeleteAsync(int id);
    }
}
