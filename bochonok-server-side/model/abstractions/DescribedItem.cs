namespace bochonok_server_side.model.abstractions;

public class DescribedItem : ItemWithId
{
    public string title { get; set; } = string.Empty;
    public string description { get; set; } = string.Empty;
    public string imagePath { get; set; } = string.Empty;
}