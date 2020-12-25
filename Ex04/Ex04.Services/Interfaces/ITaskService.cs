using Ex04.Dtos;
using Ex04.Services.Helpers;

namespace Ex04.Services.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса который описывает возможные действия с задачами.
    /// </summary>
    public interface ITaskService
    {
        /// <summary>
        /// Метод для получения всех сохранённых задач, с учётом аргументов пагинации.
        /// </summary>
        /// <param name="pageNumber">Номер страницы которую следует получить. (Пагинация)</param>
        /// <param name="pageSize">Максимальное количество элементов которое может быть на странице. (Пагинация)</param>
        /// <returns>Коллекцию задач внутри объекта с метаданными пагинации.</returns>
        PagedList<TaskDto> GetAllTasks(int pageNumber, int pageSize);
    }
}
