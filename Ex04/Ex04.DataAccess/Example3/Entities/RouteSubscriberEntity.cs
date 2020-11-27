using System.Collections.Generic;

namespace Ex04.DataAccess.Example3.Entities
{
    public class RouteSubscriberEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public UserEntity User { get; set; }

        public int RouteId { get; set; }
        public RouteEntity Route { get; set; }

        public ICollection<TaskAssigneeEntity> AssignedTasks { get; set; }
    }
}
