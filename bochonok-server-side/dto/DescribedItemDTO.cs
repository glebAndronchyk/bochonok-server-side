namespace bochonok_server_side.dto;

public class DescribedItemDTO : ItemWithIdDTO
{
  public string title { get; set; }
  public string desciption { get; set; }
  public string imagePath { get; set; }
}