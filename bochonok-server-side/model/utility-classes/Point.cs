namespace bochonok_server_side.model.utility_classes;

public class Point
{
  private int X;
  private int Y;

  public Point(int x, int y)
  {
    X = x;
    Y = y;
  }
  
  public int GetX() => X;
  public int GetY() => Y;

  public override string ToString()
  {
    return "X: " + X + ", Y: " + Y;
  }
}