using System.Linq;
using Ex04.Services.Helpers;

namespace Ex04.Services.Extensions
{
    public static class Extensions
    {
        /// <summary>
        /// Метод расширения выполняющий запрос к источнику данных 
        /// с учётом аргументов пагинации.
        /// </summary>
        /// <param name="source">Источник данных.</param>
        /// <param name="pageNumber">Номер страницы которую следует получить. (Пагинация)</param>
        /// <param name="pageSize">Максимальное количество элементов которое может быть на странице. (Пагинация)</param>
        /// <returns>Коллекцию элементов внутри объекта с метаданными пагинации.</returns>
        public static PagedList<T> ToMyPagedList<T>(
            this IQueryable<T> source,
            int pageNumber,
            int pageSize) where T : class
        {
            var items = 
                source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            int count = source.Count();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
