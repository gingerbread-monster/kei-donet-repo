using System;
using System.Collections.Generic;

namespace Ex04.Services.Helpers
{
    public class PagedList<T>
    {
        public IEnumerable<T> Items { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public bool HasPrevious => CurrentPage > 1;

        public bool HasNext => CurrentPage < TotalPages;

        public PagedList() { }

        public PagedList(IEnumerable<T> items, int totalCount, int selectedPageNumber, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            PageSize = pageSize;
            CurrentPage = selectedPageNumber;
            TotalPages = pageSize == 0 ?
                selectedPageNumber :
                (int)Math.Ceiling(totalCount / (double)pageSize);
        }
    }
}
