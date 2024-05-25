using AutoMapper;
using bochonok_server_side.Controllers.BaseController;
using bochonok_server_side.database;
using bochonok_server_side.dto.qr_code;
using bochonok_server_side.model._errors;
using bochonok_server_side.model.encoding;
using bochonok_server_side.Model.Image;
using bochonok_server_side.model.qr_code;
using Microsoft.AspNetCore.Mvc;

namespace bochonok_server_side.api
{
  [ApiController]
  [Route("api/[controller]")]
  public class QRCodeController : BaseController
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
            Console.WriteLine(e);
            var msg = e is QRCodeError ? e.Message : "Unhandled QRCode exception.";
            
            return BadRequest(msg);
        }
    }
  }
}
