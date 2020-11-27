using System.Linq;
using Microsoft.EntityFrameworkCore;
using Ex04.DataAccess.Example3;
using Ex04.Dtos;
using Ex04.Services.Extensions;
using Ex04.Services.Helpers;
using Ex04.Services.Interfaces;

namespace Ex04.Services.Implementations
{
    public class TaskService : ITaskService
    {
        readonly Example3DbContext _dbContext;

        public TaskService(Example3DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public PagedList<TaskDto> GetTasks(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
                pageNumber = 1;
            if (pageSize <= 0)
                pageSize = 2;

            var pagedTaskEntities = _dbContext
                .Tasks
                .AsNoTracking()
                .ToMyPagedList(pageNumber, pageSize);

            #region Маппинг
            PagedList<TaskDto> pagedTaskDtos = new()
            {
                CurrentPage = pagedTaskEntities.CurrentPage,
                PageSize = pagedTaskEntities.PageSize,
                TotalPages = pagedTaskEntities.TotalPages,
                TotalCount = pagedTaskEntities.TotalCount,
                Items = pagedTaskEntities.Items.Select(taskEntity => new TaskDto(
                    Id: taskEntity.Id,
                    Description: taskEntity.Description,
                    IsCompleted: taskEntity.IsCompleted,
                    PriorityLevel: taskEntity.PriorityLevel))
            };
            #endregion

            return pagedTaskDtos;
        }
    }
}
