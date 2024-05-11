using AutoMapper;
using bochonok_server_side.database;
using bochonok_server_side.dto.category;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bochonok_server_side.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class CategoriesController : BaseController.BaseController
  {
    public CategoriesController(DataContext context, IMapper mapper) : base(context, mapper)
    { }

    // GET: api/Categories
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
    {
      var categories = await _context.Categories.ToListAsync();
      
      return Ok(categories.OrderBy(category => category.isFavorite ? -1 : 1));
    }

    // POST: api/Categories
    [HttpPost]
    public async Task<ActionResult<CategoryDTO>> AddCategory(CategoryTransferObject categoryBody)
    {
      var categoryDto = _mapper.Map<CategoryTransferObject, CategoryDTO>(categoryBody);
      
      _context.Categories.Add(categoryDto);
      await _context.SaveChangesAsync();

      return CreatedAtAction(nameof(GetCategory), new { categoryDto.id }, categoryDto);
    }

    // GET: api/Categories/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDTO>> GetCategory(string id)
    {
      var category = await _context.Categories.FindAsync(id);

      if (category == null)
      {
        return NotFound();
      }

      return Ok(category);
    }
  }
}
