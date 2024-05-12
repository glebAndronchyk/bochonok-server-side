using Microsoft.EntityFrameworkCore;

namespace bochonok_server_side.dto;

[PrimaryKey("id")]
public class ItemWithIdDTO
{
  public string id { get; set; }
}