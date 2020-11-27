using Ex04.Dtos;
using Ex04.Services.Helpers;

namespace Ex04.Services.Interfaces
{
    public interface ITaskService
    {
        PagedList<TaskDto> GetTasks(int pageNumber, int pageSize);
    }
}
