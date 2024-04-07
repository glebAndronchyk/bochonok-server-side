using bochonok_server_side.api.AppController;
using bochonok_server_side.interfaces;
using bochonok_server_side.model;
using Microsoft.AspNetCore.Mvc;

namespace bochonok_server_side.api;

[ApiController]
[Route("[controller]")]
public class CategoriesController : AppController<Category, Category>
{
    public CategoriesController(IEntityService<Category> service): base(service)
    { }
}