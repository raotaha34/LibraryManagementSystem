using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.Common
{
    public class PaginatedList<T>
    {
        public List<T>? Items { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;        
    }
}
