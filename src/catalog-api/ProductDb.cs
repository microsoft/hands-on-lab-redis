using Microsoft.EntityFrameworkCore;

class ProductDb : DbContext
{
    public ProductDb(DbContextOptions<ProductDb> options)
        : base(options) { }

    public DbSet<Product> Products => Set<Product>();
}
