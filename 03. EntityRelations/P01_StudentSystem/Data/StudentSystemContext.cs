using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Data
{
    public class StudentSystemContext : DbContext
    {
        public virtual DbSet<Course> Courses { get; set; }

        public virtual DbSet<Homework> Homeworks { get; set; }

        public virtual DbSet<Resource> Resources { get; set; }

        public virtual DbSet<Student> Students { get; set; }

        public virtual DbSet<StudentCourse> StudentsCourses { get; set; }

        public StudentSystemContext(DbContextOptions contextOptions) : base(contextOptions)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"
                    Server=DIMITARPC;Database=StudentSystem;
                    Integrated Security=True;
                    Trust Server Certificate=True;");
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Resource>(x =>
            {
                x.HasOne(x => x.Course)
                .WithMany(x => x.Resources)
                .HasForeignKey(x => x.CourseId);
            });

            modelBuilder.Entity<Homework>(x =>
            {
                x.HasOne(x => x.Student)
                .WithMany(x => x.Homeworks)
                .HasForeignKey(x => x.StudentId);

                x.HasOne(x => x.Course)
                .WithMany(x => x.Homeworks)
                .HasForeignKey(x => x.CourseId);
            });

            modelBuilder.Entity<StudentCourse>(x =>
            {
                x.ToTable("StudentsCourses");

                x.HasKey(x => new { x.StudentId, x.CourseId });

                x.HasOne(x => x.Student)
                .WithMany(x => x.StudentsCourses)
                .HasForeignKey(x => x.StudentId);

                x.HasOne(x => x.Course)
                .WithMany(x => x.StudentsCourses)
                .HasForeignKey(x => x.CourseId);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
