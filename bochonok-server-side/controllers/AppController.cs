using bochonok_server_side.interfaces;
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
}