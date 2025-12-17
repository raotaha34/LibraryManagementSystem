using Library_Management_System.Models;

namespace Library_Management_System.Services.Interfaces
{
    public interface IMemberService
    {
        Task<List<Member>> GetAllAsync();
        Task<Member?> GetByIdAsync(int id);
        Task AddAsync(Member member);
        Task UpdateAsync(Member member);
        Task SoftDeleteAsync(int id);
    }
}
