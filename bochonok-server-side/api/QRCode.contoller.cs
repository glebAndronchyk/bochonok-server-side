using AutoMapper;
using bochonok_server_side.database;
using bochonok_server_side.dto.qr_code;
using bochonok_server_side.model.encoding;
using bochonok_server_side.Model.Image;
using Microsoft.AspNetCore.Mvc;

namespace bochonok_server_side.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class QRCodeController : BaseController.BaseController
  {
    public QRCodeController(DataContext context, IMapper mapper) : base(context, mapper)
    { }

    [HttpPost]
    public async Task<ActionResult<IEnumerable<QRCodeDTO>>> GetQRCodes([FromBody] QRCodeRequestDTO body)
    {
      try
      {
        var category = _context.Categories.Find(body.categoryId);
        var qr = new QRCode(body.url).Build();
        var qrB64 = qr.ToBase64String();
        var maskedCatalogImageB64 = 
          new InteractiveImage(StringEncoder.GetCleanB64(category.imageB64))
          .ApplyMask(qr.GetRgba32Bytes())
          .ToBase64String();
        
        return Ok(new QRCodeDTO
        {
          defaultQR = qrB64,
          categoryQR = maskedCatalogImageB64
        });
      }
      catch (Exception e)
      {
        return NotFound(e.Message);
      }
    }
  }
}
