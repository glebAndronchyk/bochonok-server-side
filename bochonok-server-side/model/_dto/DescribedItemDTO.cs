namespace bochonok_server_side.dto;

public class DescribedItemDTO : ItemWithIdDTO
{
  public string title { get; set; }
  public string description { get; set; }
  public string imageB64 { get; set; }
}