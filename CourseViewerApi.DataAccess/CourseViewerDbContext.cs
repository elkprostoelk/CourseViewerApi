using CourseViewerApi.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CourseViewerApi.DataAccess
{
    public class CourseViewerDbContext : IdentityDbContext<User>
    {
        public CourseViewerDbContext(DbContextOptions<CourseViewerDbContext> options)
            : base(options)
        {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "478937a6-8375-4261-9941-4917f0301509", ConcurrencyStamp = "b7e01bfb-f2c7-4f1f-9f25-b6dea5e46edb", Name = "admin", NormalizedName = "ADMIN"},
                new IdentityRole { Id = "305efec5-731f-4aa4-9998-4ce37b71bbf2", ConcurrencyStamp = "48b5743c-8f32-4c0a-a02c-8ad1a089ae9d", Name = "teacher", NormalizedName = "TEACHER" },
                new IdentityRole { Id = "a6e17dcd-86a8-4089-97b5-afd455d36ba3", ConcurrencyStamp = "3ce8de57-906f-432f-a92c-9bc5dba508d8", Name = "student", NormalizedName = "STUDENT" }
                );
        }
    }
}
