using bochonok_server_side.controllers;
using bochonok_server_side.interfaces;
using bochonok_server_side.model;
using Microsoft.AspNetCore.Mvc;

namespace bochonok_server_side.features.Catalog;

[ApiController]
[Route("[controller]")]
public class CatalogController : AppController<CatalogItem, CatalogItem>
{
    public CatalogController(IEntityService<CatalogItem> service): base(service)
    { }
}