using ChargeSharedProto2.Data.Configurations;
using ChargeSharedProto2.Models;
using Microsoft.EntityFrameworkCore;

namespace ChargeSharedProto2.Data.Contexts
{
    public class ChargeShareDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        string dbConnectionString;

        public DbSet<ChargeStation> Chargers { get; set; }
        public DbSet<UserAdres> Adresses { get; set; }

        public ChargeShareDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            dbConnectionString = _configuration.GetConnectionString("DefaultConnection2");
        }

        public ChargeShareDbContext(DbContextOptions<ChargeShareDbContext> context, IConfiguration configuration) : base(context)
        {
            _configuration = configuration;
            dbConnectionString = _configuration.GetConnectionString("DefaultConnection2");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connectionString: dbConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            /*modelBuilder.ApplyConfiguration();*/
        }
    }
}
