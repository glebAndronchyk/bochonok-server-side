using AutoMapper;
using bochonok_server_side.dto;
using bochonok_server_side.dto.product;
using bochonok_server_side.model.product_list;

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
    });

    IMapper mapper = config.CreateMapper();
    services.AddSingleton(mapper);
  }
}