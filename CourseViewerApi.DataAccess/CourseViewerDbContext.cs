using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CourseViewerApi.DataAccess
{
    public class CourseViewerDbContext : DbContext
    {
        public CourseViewerDbContext(DbContextOptions<CourseViewerDbContext> options)
            : base(options)
        {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
