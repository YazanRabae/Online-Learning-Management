using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using LMS.Service.Common.Constants;
using Microsoft.EntityFrameworkCore;

namespace LMS.Service.DTOs.Shared
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }

        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPages;

        public static PaginatedList<T> CreateAsync(List<T> source, int pageIndex)
        {
            int pageSize = PaginationConstants.PageSize;
            return new PaginatedList<T>(items: source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList(),
               count: source.Count(),
               pageIndex,
               pageSize);
        }

    }
}
