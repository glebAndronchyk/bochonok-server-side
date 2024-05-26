using bochonok_server_side.dto.category;
using bochonok_server_side.dto.product;
using bochonok_server_side.dto.sale;
using Microsoft.EntityFrameworkCore;

namespace bochonok_server_side.database;

public class DataContext: DbContext
{
    public DataContext(DbContextOptions<DataContext> options): base(options)
    { }

    public virtual DbSet<CategoryDTO> Categories { get; set; }
    public virtual DbSet<ProductDTO> ProductList { get; set; }
    public virtual DbSet<SaleDTO> Sales { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductDTO>()
            .HasOne(p => p.category)
            .WithMany(c => c.products)
            .HasForeignKey(p => p.categoryId);
    }
}