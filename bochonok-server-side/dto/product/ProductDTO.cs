namespace bochonok_server_side.dto.product;

public class ProductDTO : DescribedItemDTO
{
    public double price { get; set; }
    public double salePrice { get; set; }
    public string longDescription { get; set; }
    public string categoryId { get; set; }
    public string soldBy { get; set; }
    public double rating { get; set; }
    public double totalRating { get; set; }
    public int totalRated { get; set; }
}