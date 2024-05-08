using bochonok_server_side.model.enums;

namespace bochonok_server_side.model;

public class QRSize
{
  public int Width { get; }
  public int Height { get; }

  private int _multiplier;

  
  public QRSize(int width, int height, int multiplier = 1)
  {
    Width = width * multiplier;
    Height = height * multiplier;
    _multiplier = multiplier;
  }
  
  public QRSize GetDividedSize()
  {
    return new QRSize(Width / _multiplier, Height / _multiplier, 1);
  }
}