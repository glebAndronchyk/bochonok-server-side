using bochonok_server_side.dto.category;
using bochonok_server_side.dto.product;
using Microsoft.EntityFrameworkCore;

namespace bochonok_server_side.database;

public class DataContext: DbContext
{
    public DataContext(DbContextOptions<DataContext> options): base(options)
    { }

    public DbSet<CategoryDTO> Categories => Set<CategoryDTO>();
    public DbSet<ProductDTO> ProductList => Set<ProductDTO>();
}