using Microsoft.AspNetCore.Mvc;

namespace bochonok_server_side.api;

[ApiController]
[Route("[controller]")]
public class QRCodeController : ControllerBase
{
    [HttpGet(Name = "GetQRCode")]
    public int Get()
    {
        return 10;
    }
}