using System;
using System.Collections.Generic;

namespace Ex04.Services.Helpers
{
    /// <summary>
    /// Вспомогательный класс для пагинации содержащий метаданные.
    /// </summary>
    public class PagedList<T> where T : class
    {
        public IEnumerable<T> Items { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int PageSize { get; set; }

        public int TotalItemsCount { get; set; }

        public bool HasPrevious => CurrentPage > 1;

        public bool HasNext => CurrentPage < TotalPages;

        public PagedList() { }

        public PagedList(IEnumerable<T> items, int totalItemsCount, int selectedPageNumber, int pageSize)
        {
            Items = items;
            TotalItemsCount = totalItemsCount;
            PageSize = pageSize;
            CurrentPage = selectedPageNumber;
            TotalPages = pageSize == 0 ?
                selectedPageNumber :
                (int)Math.Ceiling(totalItemsCount / (double)pageSize);
        }
    }
}
