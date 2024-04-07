using bochonok_server_side.controllers;
using bochonok_server_side.database;
using bochonok_server_side.interfaces;
using bochonok_server_side.services;
using Microsoft.AspNetCore.Mvc;

namespace bochonok_server_side.features.Categories;

[ApiController]
[Route("[controller]")]
public class CategoriesController : AppController<Category>
{
    public CategoriesController(IEntityService<Category> service): base(service)
    { }

    [HttpPost]
    public async Task<ActionResult<List<Category>>> AddCategory(Category category)
    {
        return Ok(await _service.Add(category));
    }

    [HttpGet]
    public async Task<ActionResult<List<Category>>> GetCategories()
    {
        return Ok(await _service.GetAll());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetCategory(Guid id)
    {
        try
        {
            return Ok(await _service.GetById(id));
        }
        catch (Exception e)
        {
            BadRequest(e);
            throw;
        }
    }
}