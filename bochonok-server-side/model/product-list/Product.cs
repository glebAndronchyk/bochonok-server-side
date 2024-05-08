namespace bochonok_server_side.model.product_list;

public class Product
{
    public string Id { get; private set; }
    // public List<SaleDTO> sales
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string ImageB64 { get; private set; }
    public double Price { get; private set; }
    public string LongDescription { get; private set; }
    public string CategoryId { get; private set; }
    public string SoldBy { get; private set; }
    public double Rating { get; private set; } = 0;
    public int TotalRated { get; private set; } = 0;

    public Product(string id, string name, string description, string imageB64, string longDescription, string categoryId)
    {
        Id = id;
        Name = name;
        Description = description;
        ImageB64 = imageB64;
        LongDescription = longDescription;
        CategoryId = categoryId;
    }

    public Product(Product p)
    {
        Id = new Guid().ToString();
        Name = p.Name;
        Description = p.Description;
        ImageB64 = p.ImageB64;
        Price = p.Price;
        LongDescription = p.LongDescription;
        CategoryId = p.CategoryId;
        Rating = p.Rating;
    }

    public void ChangeRating(double ratedValue)
    {
        TotalRated++;
        Rating = (Rating + ratedValue) / TotalRated;
    }
    
    // public void ChangePrice()

    // public void ApplySale()
}