using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;

public class CosmosDBContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseCosmos(
            Environment.GetEnvironmentVariable("AZURE_COSMOS_CONNECTION_STRING")??"",
            databaseName: Environment.GetEnvironmentVariable("AZURE_COSMOS_DATABASE")??""
        );
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .ToContainer("products");

        modelBuilder.Entity<Product>()
            .HasNoDiscriminator();

        modelBuilder.Entity<Product>()
            .HasPartitionKey(o => o.id);
    }
}
