namespace bochonok_server_side.dto.sale;

public class SaleDTO : ItemWithIdDTO
{
  public double percentage { get; set; }
  public DateTime expires { get; set; }
}