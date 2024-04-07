using bochonok_server_side.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace bochonok_server_side.api.AppController;

public abstract class AppController<TEntity,TEntityDTO>: ControllerBase where TEntity: class where TEntityDTO: class
{
    protected readonly IEntityService<TEntity> _service;
    
    public AppController(IEntityService<TEntity> service)
    {
        _service = service;
    }
    
    [HttpPost]
    public virtual async Task<ActionResult<List<TEntity>>> Add(TEntityDTO entity)
    {
        return Ok(await _service.Add(entity));
    }

    [HttpGet]
    public virtual async Task<ActionResult<List<TEntity>>> GetAll()
    {
        return Ok(await _service.GetAll());
    }

    [HttpGet("{id}")]
    public virtual async Task<ActionResult<TEntity>> GetById(Guid id)
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