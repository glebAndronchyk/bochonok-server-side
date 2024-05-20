namespace bochonok_server_side.model.utility_classes.grouping_utilities;

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
