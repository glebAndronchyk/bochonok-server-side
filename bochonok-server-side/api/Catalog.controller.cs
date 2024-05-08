using bochonok_server_side.api.AppController;
using bochonok_server_side.interfaces;
using bochonok_server_side.model;
using Microsoft.AspNetCore.Mvc;

namespace bochonok_server_side.api;

[ApiController]
[Route("[controller]")]
public class CatalogController : AppController<CatalogItem, CatalogItem>
{
    public CatalogController(IEntityService<CatalogItem> service): base(service)
    { }
}