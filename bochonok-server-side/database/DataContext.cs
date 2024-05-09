using bochonok_server_side.dto.category;
using bochonok_server_side.dto.product;
using bochonok_server_side.dto.sale;
using Microsoft.EntityFrameworkCore;

namespace bochonok_server_side.database;

public class DataContext: DbContext
{
    public DataContext(DbContextOptions<DataContext> options): base(options)
    { }

    public DbSet<CategoryDTO> Categories => Set<CategoryDTO>();
    public DbSet<ProductDTO> ProductList => Set<ProductDTO>();
    public DbSet<SaleDTO> Sales => Set<SaleDTO>();
}