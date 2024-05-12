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
  public class ProductsController : BaseController.BaseController
  {
    public ProductsController(DataContext context, IMapper mapper)
      :base(context, mapper)
    { }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SimplifiedProductDTO>>> GetProducts()
    {
      var products = await _context.ProductList.ToListAsync();
      return Ok(_mapper.Map<List<ProductDTO>, List<SimplifiedProductDTO>>(products));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDTO>> GetProduct(string id)
    {
      var product = await _context.ProductList.FindAsync(id);

      if (product == null)
      {
        return NotFound();
      }

      return Ok(product);
    }

    [HttpGet("Category/{categoryId}")]
    public async Task<ActionResult<IEnumerable<SimplifiedProductDTO>>> GetProductsByCategory(string categoryId)
    {
      var products = await _context.ProductList
        .Where(p => p.categoryId == categoryId)
        .ToListAsync();

      return Ok(_mapper.Map<List<ProductDTO>, List<SimplifiedProductDTO>>(products));
    }
    
    [HttpPost]
    public async Task<ActionResult<ProductDTO>> AddProduct(ProductTransferObject productBody)
    {
      var productDto = _mapper.Map<ProductTransferObject, ProductDTO>(productBody);
      _context.ProductList.Add(productDto);
      await _context.SaveChangesAsync();

      return Ok(productDto.categoryId);
    }

    [HttpPut("{id}/Rating")]
    public async Task<IActionResult> ChangeProductRating(string id, [FromBody] double newRating)
    {
      var productDto = await _context.ProductList.FindAsync(id);

      if (productDto == null)
      {
        return NotFound();
      }

      var product = _mapper.Map<ProductDTO, Product>(productDto);
      product.ChangeRating(newRating);

      await _context.ProductList.Where(p => p.id == id)
        .ExecuteUpdateAsync(updates =>
            updates.SetProperty(p => p.rating, product.Rating)
              .SetProperty(p => p.totalRating, product.TotalRating)
              .SetProperty(p => p.totalRated, product.TotalRated)
        );
      await _context.SaveChangesAsync();

      return NoContent();
    }

    // PUT: api/Products/5/Price
    // [HttpPut("{id}/Price")]
    // public async Task<IActionResult> ChangeProductPrice(int id, [FromBody] decimal newPrice)
    // {
    //   var productNoIdDto = await _context.ProductList.FindAsync(id);
    //
    //   if (productNoIdDto == null)
    //   {
    //     return NotFound();
    //   }
    //
    //   var product = _mapper.Map<Product>(productNoIdDto);
    //   product.ChangePrice(newPrice);
    //   var updatedProductDto = _mapper.Map<ProductTransferObject>(product);
    //   await _context.SaveChangesAsync();
    //
    //   return NoContent();
    // }
  }
}