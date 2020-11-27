using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ex04.DataAccess.Example3.Entities;

namespace Ex04.DataAccess.Example3.Configurations
{
    class TaskListEntityConfiguration :
        IEntityTypeConfiguration<TaskListEntity>
    {
        public void Configure(EntityTypeBuilder<TaskListEntity> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedOnAdd();

            builder.HasOne(t => t.Route)
                .WithMany(r => r.TaskLists)
                .HasForeignKey(t => t.RouteId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
