using Microsoft.EntityFrameworkCore;
using Ex04.DataAccess.Example3.Entities;
using Ex04.DataAccess.Example3.Extensions;
using Ex04.DataAccess.Example3.Configurations;

namespace Ex04.DataAccess.Example3
{
    public class Example3DbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<UserPaymentInfoEntity> UserPaymentInfo { get; set; }
        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<TaskListEntity> TaskLists { get; set; }
        public DbSet<TaskAssigneeEntity> TaskAssignees { get; set; }
        public DbSet<RouteEntity> Routes { get; set; }
        public DbSet<RouteSubscriberEntity> RouteSubscribers { get; set; }

        public Example3DbContext(DbContextOptions<Example3DbContext> options) 
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RouteEntityConfiguration())
                .ApplyConfiguration(new RouteSubscriberEntityConfiguration())
                .ApplyConfiguration(new TaskAssigneeEntityConfiguration())
                .ApplyConfiguration(new TaskEntityConfiguration())
                .ApplyConfiguration(new TaskListEntityConfiguration())
                .ApplyConfiguration(new UserEntityConfiguration())
                .ApplyConfiguration(new UserPaymentInfoEntityConfiguration());

            modelBuilder.SeedExample3();
        }
    }
}
