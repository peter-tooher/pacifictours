using Microsoft.Identity.Client;

namespace PacificToursApp.Server.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Tour> Tours { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<SingleSuite> SingleSuites { get; set; }
        public DbSet<DoubleSuite> DoubleSuites { get; set; }
        public DbSet<FamilySuite> FamilySuites { get; set; }
    }
}