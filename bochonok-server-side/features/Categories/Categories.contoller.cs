using bochonok_server_side.controllers;
using bochonok_server_side.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace bochonok_server_side.features.Categories;

[ApiController]
[Route("[controller]")]
public class CategoriesController : AppController<CategoryDTO>
{
    public CategoriesController(IEntityService<CategoryDTO> service): base(service)
    { }
}