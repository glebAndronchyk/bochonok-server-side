using AutoMapper;
using bochonok_server_side.database;
using bochonok_server_side.dto.sale;
using bochonok_server_side.model.sale;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bochonok_server_side.Controllers;

  [ApiController]
  [Route("controllers/[controller]")]
  public class SalesController : BaseController.BaseController
  {
    public SalesController(DataContext context, IMapper mapper) : base(context, mapper)
    { }

    // GET: controllers/Sales
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SaleDTO>>> GetSales()
    {
      var sales = await _context.Sales.ToListAsync();
      return _mapper.Map<List<SaleDTO>>(sales);
    }

    // POST: controllers/Sales
    [HttpPost]
    public async Task<ActionResult<SaleDTO>> AddSale(SaleDTO saleDto)
    {
      saleDto.id = new Guid().ToString();
      var sale = _mapper.Map<SaleDTO>(saleDto);
      _context.Sales.Add(sale);
      await _context.SaveChangesAsync();

      return CreatedAtAction(nameof(GetSale), new { sale.id }, _mapper.Map<SaleDTO>(sale));
    }

    // GET: controllers/Sales/5
    [HttpGet("{id}")]
    public async Task<ActionResult<SaleDTO>> GetSale(string id)
    {
      var sale = await _context.Sales.FindAsync(id);

      if (sale == null)
      {
        return NotFound();
      }

      return _mapper.Map<SaleDTO>(sale);
    }
  }