using System.Linq;
using Ex04.Services.Helpers;

namespace Ex04.Services.Extensions
{
    public static class Extensions
    {
        public static PagedList<T> ToMyPagedList<T>(
            this IQueryable<T> source,
            int pageNumber,
            int pageSize)
        {
            var items = 
                source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            int count = source.Count();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
