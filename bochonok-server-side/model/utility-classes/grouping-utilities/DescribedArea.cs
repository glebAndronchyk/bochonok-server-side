using bochonok_server_side.model.utility_classes.grouping_utilities;

namespace bochonok_server_side.model.utility_classes;

public class DescribedArea : AreaCoordinates
{
  public string Name { get; private set; }

  public DescribedArea(Point topLeftCorner, Point bottomRightCorner, string name = "") 
    : base(topLeftCorner, bottomRightCorner)
  {
    Name = name;
  }
}