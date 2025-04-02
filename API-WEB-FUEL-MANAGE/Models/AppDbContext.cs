using Microsoft.EntityFrameworkCore;

namespace API_WEB_FUEL_MANAGE.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
             
        }
     public DbSet<Veiculo> Veiculos { get; set; }
     public DbSet<Consumo> Consumos { get; set; }

    }

}
