using Microsoft.EntityFrameworkCore;

namespace KevBlog.Infrastructure.Helpers
{
    public class PageList<T> : List<T>
    {
        public PageList(IEnumerable<T> items, int count, int pageNum, int pageSize)
        {
            CurrentPage = pageNum;
            TotalPages = (int) Math.Ceiling(count / (double) pageSize);
            PageSize = pageSize;
            TotalCount = count;
            AddRange(items);
        }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public static async Task<PageList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize){
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber-1) * pageSize).Take(pageSize).ToListAsync();
            return new PageList<T>(items, count, pageNumber, pageSize);
        }
    }
}