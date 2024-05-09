namespace bochonok_server_side.model.utility_classes;

public class Point
{
  public int X { get; set; }

  public int Y { get; set; }


  public Point(int x, int y)
  {
    X = x;
    Y = y;
  }

  public Point(Point pCopy)
  {
    X = pCopy.X;
    Y = pCopy.Y;
  }

  // TODO: deprecated
  public int GetX() => X;
  public int GetY() => Y;
  
  public override string ToString()
  {
    return "X: " + X + ", Y: " + Y;
  }
}