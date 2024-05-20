namespace bochonok_server_side.dto.product;

public class ProductRequestDTO : DescribedItemRequestDTO
{
  public double price { get; set; }
  public string longDescription { get; set; }
  public string categoryId { get; set; }
  public string soldBy { get; set; }
}