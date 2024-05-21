using AutoMapper;
using bochonok_server_side.Controllers.BaseController;
using bochonok_server_side.database;
using bochonok_server_side.dto.category;
using bochonok_server_side.model.encoding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bochonok_server_side.api
{
  [ApiController]
  [Route("api/[controller]")]
  public class CategoriesController : BaseController
  {
    public CategoriesController(DataContext context, IMapper mapper) : base(context, mapper)
    { }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
    {
      var categories = await _context.Categories.ToListAsync();
      var withMIMECategories = categories.Select(c =>
      {
        c.imageB64 = StringEncoder.AddMIMEType(c.imageB64, "image/png");
        return c;
      });
      
      return Ok(withMIMECategories.OrderBy(category => category.isFavorite ? -1 : 1));
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDTO>> AddCategory(CategoryRequestDTO body)
    {
      var categoryDto = _mapper.Map<CategoryRequestDTO, CategoryDTO>(body);
      
      _context.Categories.Add(new CategoryDTO
      {
        description = categoryDto.description,
        title = categoryDto.title,
        isFavorite = categoryDto.isFavorite,
        imageB64 = StringEncoder.GetCleanB64(categoryDto.imageB64),
        id = categoryDto.id,
      });
      await _context.SaveChangesAsync();

      return CreatedAtAction(nameof(GetCategory), new { categoryDto.id }, categoryDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDTO>> GetCategory(string id)
    {
      var category = await _context.Categories.FindAsync(id);

      if (category == null)
      {
        return NotFound();
      }

      category.imageB64 = StringEncoder.AddMIMEType(category.imageB64, "image/png");

      return Ok(category);
    }
  }
}
