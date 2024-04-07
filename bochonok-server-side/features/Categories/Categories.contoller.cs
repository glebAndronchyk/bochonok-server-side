using bochonok_server_side.database;
using bochonok_server_side.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace bochonok_server_side.features.Categories;

[ApiController]
[Route("[controller]")]
public class CategoriesController : ControllerBase, IDbContextController
{
    public DataContext databaseContext { get; set; }
    
    public CategoriesController(DataContext ctx)
    {
        databaseContext = ctx;
    }
}