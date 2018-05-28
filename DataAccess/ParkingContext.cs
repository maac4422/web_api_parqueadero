namespace DataAccess
{
    using DataAccess.Models;
    using Microsoft.EntityFrameworkCore;

    public class ParkingContext : DbContext
    {
       

        public ParkingContext()
        { }

        public ParkingContext(DbContextOptions<ParkingContext> options) : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=parking;Trusted_Connection=True;");
            }
        }

        public DbSet<Vehicle> Vehicles { get; set; }
    }
}
