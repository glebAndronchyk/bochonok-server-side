namespace bochonok_server_side.dto.product;

public class SimplifiedProductDTO : DescribedItemDTO
{
  public double price { get; set; }
  public double salePrice { get; set; }
  public string longDescription { get; set; }
  public string categoryId { get; set; }
  public double rating { get; set; }
}