namespace bochonok_server_side.Controllers.BaseController;

using AutoMapper;
using bochonok_server_side.database;
using Microsoft.AspNetCore.Mvc;

public abstract class BaseController : ControllerBase
{
  protected readonly DataContext _context;
  protected readonly IMapper _mapper;

  protected BaseController(DataContext context, IMapper mapper)
  {
    _context = context;
    _mapper = mapper;
  }
}