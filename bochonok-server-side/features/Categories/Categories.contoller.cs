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
}