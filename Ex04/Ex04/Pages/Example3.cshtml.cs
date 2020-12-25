using Microsoft.AspNetCore.Mvc.RazorPages;
using Ex04.Dtos;
using Ex04.Services.Interfaces;
using Ex04.Services.Helpers;

namespace Ex04.Pages
{
    public class Example3Model : PageModel
    {
        readonly ITaskService _taskService;

        public PagedList<TaskDto> PagedTaskDtos;

        public Example3Model(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public void OnGet(int pageNumber = 1, int pageSize = 2)
        {
            PagedTaskDtos = _taskService.GetAllTasks(pageNumber, pageSize);
        }
    }
}
