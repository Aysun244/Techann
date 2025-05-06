using Microsoft.EntityFrameworkCore;
using Techan.Models;
namespace Techan.DataAccessLayer
{
    public class TechanDbContext : DbContext
    {
        public DbSet<Slider> Sliders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=Techan;Trusted_Connection=True;TrustServerCertificate=True;");
            base.OnConfiguring(optionsBuilder);
        }      
    }
}
