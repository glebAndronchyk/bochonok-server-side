using AutoMapper;
using bochonok_server_side.dto;
using bochonok_server_side.dto.category;
using bochonok_server_side.dto.product;
using bochonok_server_side.dto.sale;
using bochonok_server_side.model.product_list;
using bochonok_server_side.model.sale;

namespace bochonok_server_side.mapper;

public class MappingInjector
{
  public static void InjectMappings(IServiceCollection services)
  {
    var config = new MapperConfiguration(cfg =>
    {
      cfg.CreateMap<ProductDTO, Product>().ReverseMap();
      cfg.CreateMap<DescribedItemDTO, Product>().ReverseMap();
      cfg.CreateMap<SimplifiedProductDTO, Product>().ReverseMap();
      cfg.CreateMap<SimplifiedProductDTO, ProductDTO>().ReverseMap();
      cfg.CreateMap<Sale, SaleDTO>().ReverseMap();
      MapWithId(cfg.CreateMap<CategoryRequestDTO, CategoryDTO>());
      MapWithId(cfg.CreateMap<DescribedItemRequestDTO, CategoryDTO>());
      MapWithId(MapForMembers(cfg.CreateMap<ProductRequestDTO, ProductDTO>(),
        new() { "salePrice", "rating", "totalRating", "totalRated" }, 0));
    });

    IMapper mapper = config.CreateMapper();
    services.AddSingleton(mapper);
  }

  private static IMappingExpression<TSource, TDestination> MapForMembers<TSource, TDestination, TValue>
    (IMappingExpression<TSource, TDestination> opt, List<string> destKeys, TValue val)
  {
    foreach (var key in destKeys)
    {
      opt.ForMember("salePrice", expression => expression.MapFrom(s => val));
    }

    return opt;
  }

  private static IMappingExpression<TSource, TDestination> MapWithId<TSource, TDestination>
    (IMappingExpression<TSource, TDestination> opt)
  {
    var idProperty = typeof(TDestination).GetProperty("id");

    if (idProperty != null)
    {
      return opt.ForMember(
        "id",
        opt => opt.MapFrom(
          s => Guid.NewGuid().ToString())
        );
    }
    else
    {
      throw new ArgumentException("TDestination does not have a writable string property named 'id'.");
    }
  }
}