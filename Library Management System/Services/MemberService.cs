using Library_Management_System.Common;
using Library_Management_System.Helpers;
using Library_Management_System.Models;
using Library_Management_System.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
                .Where(b => b.IsActive == true && b.RoleId == 2)
                .ToListAsync();
        }
        public async Task<PaginatedList<Member>> GetAllPaginatedAsync(int page, int pageSize)
        {
            return await PaginatedHelper<Member>.CreateAsync(_context.Members
                .Where(m => m.IsActive && m.RoleId == 2), page, pageSize);  
        }

        public async Task<Member?> GetByIdAsync(int id)
        {
            return await _context.Members
                .FirstOrDefaultAsync(m => m.Id == id && m.IsActive);
        }

        public async Task<bool> AddAsync(Member member)
        {
            var existingMember = await _context.Members
                .FirstOrDefaultAsync(m => m.Email == member.Email && m.IsActive == true);
            var softdeletedMember = await _context.Members
                .FirstOrDefaultAsync(m => m.Email == member.Email && m.Name == member.Name && m.Phone == member.Phone && m.IsActive == false);
            if (existingMember != null)
            {
                return false;
            }
            else if(softdeletedMember != null){
                softdeletedMember.IsActive = true;
                // member.PasswordHash = BCrypt.Net.BCrypt.HashPassword(member.PasswordHash);
                _context.Members.Update(softdeletedMember);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                member.CreatedAt = DateTime.Now;
                member.UpdatedAt = DateTime.Now;
                member.CreatedBy = "System";
                member.UpdatedBy = "System";
                // member.PasswordHash = BCrypt.Net.BCrypt.HashPassword(member.PasswordHash);
                _context.Members.Add(member);
                await _context.SaveChangesAsync();
                return true;
            }
        }

        public async Task UpdateAsync(Member member)
        {
            member.CreatedAt = DateTime.Now;
            member.UpdatedAt = DateTime.Now;
            member.CreatedBy = "System";
            member.UpdatedBy = member.CreatedBy;
            // member.PasswordHash = BCrypt.Net.BCrypt.HashPassword(member.PasswordHash);
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
