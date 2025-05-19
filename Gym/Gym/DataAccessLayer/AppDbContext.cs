using Gym.Models;
using Microsoft.EntityFrameworkCore;

namespace Gym.DataAccessLayer
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }    

        public DbSet<Course>Courses { get; set; }

        public DbSet<Teacher> Teachers { get; set; }
    }
}
