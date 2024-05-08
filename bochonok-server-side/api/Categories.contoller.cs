using bochonok_server_side.api.AppController;
using bochonok_server_side.interfaces;
using bochonok_server_side.model;
using bochonok_server_side.Model.Image;
using Microsoft.AspNetCore.Mvc;

namespace bochonok_server_side.api;

[ApiController]
[Route("[controller]")]
public class CategoriesController : AppController<Category, Category>
{
    public CategoriesController(IEntityService<Category> service): base(service)
    { }

    [HttpPost("{id}")]
    public async Task<ActionResult<string>> GetQrCode(string id, string encodeString)
    {
        Category category = await _service.GetById(new Guid(id));
        QRCode qr = new QRCode(encodeString);
        
        ImageBase shapeWithQr = new CatalogImage(category.imagePath).GetShape().Fill(qr.Image);
        
        shapeWithQr.Save();

        return Ok(shapeWithQr.GetB64());
    }
}