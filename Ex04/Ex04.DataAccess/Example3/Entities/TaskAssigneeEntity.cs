namespace Ex04.DataAccess.Example3.Entities
{
    public class TaskAssigneeEntity
    {
        public int TaskId { get; set; }
        public TaskEntity Task { get; set; }

        public int RouteSubscriberId { get; set; }
        public RouteSubscriberEntity RouteSubscriber { get; set; }
    }
}
