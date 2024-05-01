namespace bochonok_server_side.model;

public class QRSize
{
  public int Width { get; set; }
  public int Height { get; set; }
  
  public QRSize(int width, int height, int multiplier = 1)
  {
    Width = width * multiplier;
    Height = height * multiplier;
  }
}