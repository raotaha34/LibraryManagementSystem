using Library_Management_System.Common;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.Helpers
{
    public static class PaginatedHelper<T>
    {
        public static async Task<PaginatedList<T>> CreateAsync(
            IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();

            var items = await source
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedList<T>
            {
                Items = items,
                PageIndex = pageIndex,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)
            };
        }
    }
}
