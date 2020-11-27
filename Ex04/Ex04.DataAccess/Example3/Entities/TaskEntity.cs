using System.Collections.Generic;
using Ex04.DataAccess.Example3.Enums;

namespace Ex04.DataAccess.Example3.Entities
{
    public class TaskEntity
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public TaskPriorityLevel PriorityLevel { get; set; }

        public bool IsCompleted { get; set; }

        public int TaskListId { get; set; }
        public TaskListEntity TaskList { get; set; }

        public ICollection<TaskAssigneeEntity> TaskAssignees { get; set; }
    }
}
