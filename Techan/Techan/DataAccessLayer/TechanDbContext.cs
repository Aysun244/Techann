using Microsoft.EntityFrameworkCore;
using Techan.Models;
namespace Techan.DataAccessLayer
{
    public class TechanDbContext : DbContext
    {
        public TechanDbContext(DbContextOptions opt) :base(opt)
        {
            
        }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Product> Products { get; set; }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=Techan;Trusted_Connection=True;TrustServerCertificate=True;");
        //    base.OnConfiguring(optionsBuilder);
        //}
    }
}
