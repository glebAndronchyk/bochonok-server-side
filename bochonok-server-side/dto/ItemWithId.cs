using Microsoft.EntityFrameworkCore;

namespace bochonok_server_side.model.abstractions;

[PrimaryKey(nameof(id))]
public class ItemWithId
{
    public Guid id { get; set; } = Guid.NewGuid();
}