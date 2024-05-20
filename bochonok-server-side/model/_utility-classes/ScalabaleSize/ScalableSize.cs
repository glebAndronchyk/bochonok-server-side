namespace bochonok_server_side.model.qr_code;

public class ScalableSize
{
  public int Width { get; }
  public int Height { get; }

  private int _multiplier;

  
  public ScalableSize(int width, int height, int multiplier = 1)
  {
    Width = width * multiplier;
    Height = height * multiplier;
    _multiplier = multiplier;
  }
  
  public ScalableSize GetDividedSize()
  {
    return new ScalableSize(Width / _multiplier, Height / _multiplier);
  }
}