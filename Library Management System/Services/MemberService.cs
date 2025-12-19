using Library_Management_System.Models;
using Library_Management_System.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.Services
{
    public class MemberService : IMemberService
    {
        private readonly LibraryManagementContext _context;

        public MemberService(LibraryManagementContext context)
        {
            _context = context;
        }

        public async Task<List<Member>> GetAllAsync()
        {
            return await _context.Members
                .Where(m => m.IsActive)
                .ToListAsync();
        }

        public async Task<Member?> GetByIdAsync(int id)
        {
            return await _context.Members
                .FirstOrDefaultAsync(m => m.Id == id && m.IsActive);
        }

        public async Task AddAsync(Member member)
        {
                member.CreatedAt = DateTime.Now;
                member.UpdatedAt = DateTime.Now;
                member.CreatedBy = "System";
                member.UpdatedBy = "System";

                _context.Members.Add(member);
                await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Member member)
        {
            member.CreatedAt = DateTime.Now;
            member.UpdatedAt = DateTime.Now;
            member.CreatedBy = "System";
            member.UpdatedBy = member.CreatedBy;
            member.RoleId = 2;

            _context.Members.Update(member);
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            var member = await GetByIdAsync(id);
            if (member == null) return;

            member.IsActive = false;
            member.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
        }
    }
}
