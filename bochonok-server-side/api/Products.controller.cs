using AutoMapper;
using bochonok_server_side.database;
using bochonok_server_side.dto.product;
using bochonok_server_side.model.product_list;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace bochonok_server_side.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class ProductsController : ControllerBase
  {
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public ProductsController(DataContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    // GET: api/Products
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
    {
      var products = await _context.ProductList.ToListAsync();
      return (_mapper.Map<List<ProductDTO>>(products));
    }

    // GET: api/Products/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDTO>> GetProduct(int id)
    {
      var product = await _context.ProductList.FindAsync(id);

      if (product == null)
      {
        return NotFound();
      }

      return (_mapper.Map<ProductDTO>(product));
    }

    // GET: api/Products/Category/5
    [HttpGet("Category/{categoryId}")]
    public async Task<ActionResult<IEnumerable<SimplifiedProductDTO>>> GetProductsByCategory(string categoryId)
    {
      var products = await _context.ProductList
        .Where(p => p.categoryId == categoryId)
        .ToListAsync();

      return Ok(_mapper.Map<List<SimplifiedProductDTO>>(products));
    }
    
    [HttpPost]
    public async Task<ActionResult<ProductDTO>> AddProduct(ProductDTO productDto)
    {
      productDto.id = new Guid().ToString();
      _context.ProductList.Add(productDto);
      await _context.SaveChangesAsync();

      return Ok();
    }

    // PUT: api/Products/5/Rating
    [HttpPut("{id}/Rating")]
    public async Task<IActionResult> ChangeProductRating(int id, [FromBody] double newRating)
    {
      var productDto = await _context.ProductList.FindAsync(id);

      if (productDto == null)
      {
        return NotFound();
      }

      var product = _mapper.Map<Product>(productDto);
      product.ChangeRating(newRating);
      var updatedProductDto = _mapper.Map<ProductDTO>(product);

      _context.ProductList.Remove(productDto);
      _context.ProductList.Add(updatedProductDto);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    // PUT: api/Products/5/Price
    // [HttpPut("{id}/Price")]
    // public async Task<IActionResult> ChangeProductPrice(int id, [FromBody] decimal newPrice)
    // {
    //   var productDto = await _context.ProductList.FindAsync(id);
    //
    //   if (productDto == null)
    //   {
    //     return NotFound();
    //   }
    //
    //   var product = _mapper.Map<Product>(productDto);
    //   product.ChangePrice(newPrice);
    //   var updatedProductDto = _mapper.Map<ProductDTO>(product);
    //   await _context.SaveChangesAsync();
    //
    //   return NoContent();
    // }
  }
}