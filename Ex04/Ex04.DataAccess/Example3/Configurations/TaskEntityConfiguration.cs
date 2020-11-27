using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ex04.DataAccess.Example3.Entities;
using Ex04.DataAccess.Example3.Enums;

namespace Ex04.DataAccess.Example3.Configurations
{
    class TaskEntityConfiguration :
        IEntityTypeConfiguration<TaskEntity>
    {
        public void Configure(EntityTypeBuilder<TaskEntity> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id)
                .ValueGeneratedOnAdd();

            builder.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(t => t.IsCompleted)
                .HasDefaultValue(false);

            builder.Property(t => t.PriorityLevel)
                .HasDefaultValue(TaskPriorityLevel.Low);

            builder.HasOne(t => t.TaskList)
                .WithMany(t => t.Tasks)
                .HasForeignKey(t => t.TaskListId);
        }
    }
}
