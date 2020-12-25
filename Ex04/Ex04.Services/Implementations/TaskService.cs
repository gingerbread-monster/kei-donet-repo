using Microsoft.EntityFrameworkCore;
using Ex04.DataAccess.Example3;
using Ex04.Dtos;
using Ex04.Services.Extensions;
using Ex04.Services.Helpers;
using Ex04.Services.Interfaces;
using AutoMapper;

namespace Ex04.Services.Implementations
{
    /// <summary>
    /// Cервис предоставляющий методы для взаимодействия с задачами.
    /// </summary>
    public class TaskService : ITaskService
    {
        readonly Example3DbContext _dbContext;

        readonly IMapper _mapper;

        public TaskService(
            Example3DbContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public PagedList<TaskDto> GetAllTasks(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
                pageNumber = 1;
            if (pageSize <= 0)
                pageSize = 2;

            var pagedTaskEntities = _dbContext
                .Tasks
                .AsNoTracking()
                .ToMyPagedList(pageNumber, pageSize);

            var pagedTaskDtos = _mapper.Map<PagedList<TaskDto>>(pagedTaskEntities);

            return pagedTaskDtos;
        }
    }
}
