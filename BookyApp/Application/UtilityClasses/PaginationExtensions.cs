using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.UtilityClasses
{
    public static class PaginationExtensions
    {
        public static IEnumerable<T> Paginate<T>(this IEnumerable<T> source, int pageNumber, int pageSize)
        {
            if (pageNumber < 1) throw new ArgumentException("Page number should be greater than or equal to 1.");
            if (pageSize < 1) throw new ArgumentException("Page size should be greater than or equal to 1.");

            return source.Select((item, index) => new { item, index })
                         .Where(x => x.index >= (pageNumber - 1) * pageSize && x.index < pageNumber * pageSize)
                         .Select(x => x.item);
        }

        public static PagedResult<T> ToPagedResult<T>(this IEnumerable<T> source, int pageNumber, int pageSize)
        {
            var pagedItems = source.Paginate(pageNumber, pageSize).ToList();
            var totalItems = source.Count();

            return new PagedResult<T>
            {
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Items = pagedItems
            };
        }
    }

    public class PagedResult<T>
    {
        public int TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public List<T> Items { get; set; }
    }
}
