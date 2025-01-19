using LogisticTracking.Model;
using Microsoft.EntityFrameworkCore;

namespace LogisticTracking.Db;

public class LogisticDbContext : DbContext
{
    public LogisticDbContext(DbContextOptions<LogisticDbContext> options) : base(options)
    {
    }

    public DbSet<DeliveryModel> Deliveries { get; set; }
    public DbSet<DeliveryPartner> DeliveryPartners { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DeliveryModel>();

        base.OnModelCreating(modelBuilder);
    }
}