using System;
using System.Collections.Generic;

namespace Ex04.DataAccess.Example3.Entities
{
    public class RouteEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTimeOffset DateCreated { get; set; }

        public ICollection<RouteSubscriberEntity> RouteSubscribers { get; set; }

        public ICollection<TaskListEntity> TaskLists { get; set; }
    }
}
