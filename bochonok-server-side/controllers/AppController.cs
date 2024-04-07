﻿using bochonok_server_side.interfaces;
using bochonok_server_side.services;
using Microsoft.AspNetCore.Mvc;

namespace bochonok_server_side.controllers;

public abstract class AppController<TEntity>: ControllerBase where TEntity: class
{
    protected readonly IEntityService<TEntity> _service;
    
    public AppController(IEntityService<TEntity> service)
    {
        _service = service;
    }
    
    [HttpPost]
    public virtual async Task<ActionResult<List<TEntity>>> Add(TEntity entity)
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