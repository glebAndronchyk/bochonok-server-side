using AutoMapper;
using bochonok_server_side.database;
using bochonok_server_side.dto.category;
using bochonok_server_side.model.encoding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bochonok_server_side.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class QRCodeController : BaseController.BaseController
  {
    public QRCodeController(DataContext context, IMapper mapper) : base(context, mapper)
    { }

    [HttpGet]
  }
}
