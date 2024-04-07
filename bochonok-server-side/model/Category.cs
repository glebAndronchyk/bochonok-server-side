using Microsoft.EntityFrameworkCore;

namespace bochonok_server_side.model;

[PrimaryKey(nameof(id))]
public class Category
{
    private Guid id { get; set; } = Guid.NewGuid();
    public string title { get; set; } = string.Empty;
    public string description { get; set; } = string.Empty;
    public string image { get; set; } = string.Empty;
}