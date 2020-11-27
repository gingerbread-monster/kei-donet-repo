using System.Collections.Generic;

namespace Ex04.DataAccess.Example3.Entities
{
    public class TaskListEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int RouteId { get; set; }
        public RouteEntity Route { get; set; }

        public ICollection<TaskEntity> Tasks { get; set; }
    }
}
