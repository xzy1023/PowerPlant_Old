using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PowerPlant.Tools
{
    public class PaginationList<T> : List<T>
    {
        public int TotalCount { get; set; }

        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public bool IsFirstPage => CurrentPage == 1;
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public PaginationList(int totalCount, int currentPage, int pageSize, List<T> items)
        {
            TotalCount = totalCount;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            AddRange(items);
        }

        public static async Task<PaginationList<T>> CreateAsync(int currentPage, int pageSize, IQueryable<T> query)
        {
            var totalCount = await query.CountAsync();
            var skip = (currentPage - 1) * pageSize;
            query = query.Skip(skip).Take(pageSize);
            var items = await query.ToListAsync();
            return new PaginationList<T>(totalCount, currentPage, pageSize, items);
        }
    }
}
