namespace bochonok_server_side.model.utility_classes;

public class AreaCoordinates
{
  public Point TopLeftCorner { get; }
  public Point BottomRightCorner { get; }
  
  public AreaCoordinates(Point topLeftCorner, Point bottomRightCorner)
  {
    TopLeftCorner = topLeftCorner;
    BottomRightCorner = bottomRightCorner;
  }
}
