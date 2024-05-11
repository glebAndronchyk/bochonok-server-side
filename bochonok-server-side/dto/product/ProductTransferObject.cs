namespace bochonok_server_side.dto.product;

public class ProductTransferObject : DescribedItemTransferObject
{
  public double price { get; set; }
  public string longDescription { get; set; }
  public string categoryId { get; set; }
  public string soldBy { get; set; }
}