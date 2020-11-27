using Microsoft.EntityFrameworkCore;

namespace Ex04.DataAccess.Example1
{
    public class Example1DbContext : DbContext
    {
        public DbSet<StudentEntity> Students { get; set; }

        public Example1DbContext(DbContextOptions options) 
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentEntity>().HasData(new StudentEntity[]
            {
                new(){Id = 1, Name="Вася"},
                new(){Id = 2, Name="Петя"},
                new(){Id = 3, Name="Вова"}
            });
        }
    }
}
