using bochonok_server_side.features.Categories;
using Microsoft.EntityFrameworkCore;

namespace bochonok_server_side.database;

public class DataContext: DbContext
{
    public DataContext(DbContextOptions<DataContext> options): base(options)
    { }

    public DbSet<Category> Categories => Set<Category>();
}