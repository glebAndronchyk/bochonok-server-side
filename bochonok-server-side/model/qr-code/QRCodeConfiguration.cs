using bochonok_server_side.model.qr_code.enums;
using Point = bochonok_server_side.model.utility_classes.Point;

namespace bochonok_server_side.model.qr_code;


public class QRCodeConfiguration
{
    public QRCodeConfiguration(int width, int h)
    {
        
    }

    public static readonly Dictionary<EPlacement, Point> FinderPatternsPosition = new()
    {
        { EPlacement.TopLeft, new Point(0, 0) },
        // { EPlacement.BottomLeft, new Point(Size.Width - 7, 0) }
    };
}