using System.Text.Json.Serialization;
using bochonok_server_side.dto.product;

namespace bochonok_server_side.dto.category;

public class CategoryDTO : DescribedItemDTO
{
    public bool isFavorite { get; set; }
    [JsonIgnore]
    public ICollection<ProductDTO> products { get; set; }
}