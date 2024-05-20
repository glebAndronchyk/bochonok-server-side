using bochonok_server_side.Model.Image.enums;
using bochonok_server_side.model.qr_code.enums;
using Point = bochonok_server_side.model.utility_classes.Point;

namespace bochonok_server_side.model.qr_code;

public class QRCodeConfiguration
{
    public const int FinderModuleSize = 7;
    public const int FinderSizeWithSafeZone = FinderModuleSize + 1;
    public const int AlignmentOffset = 9;
    
    public readonly Dictionary<EAxisPlacement, Point> TimingPositions = new ()
    {
        { EAxisPlacement.Vertical, new Point(FinderSizeWithSafeZone, 6)},
        { EAxisPlacement.Horizontal, new Point(6, FinderSizeWithSafeZone)},
    };
    public int TimingLength { get; private set; }
    public Dictionary<EPlacement, Point> PatternsPosition { get; private set; }
    public ScalableSize Size { get; private set; }

    private List<ScalableSize> _sizes = new ()
    {
        new ScalableSize(10, 10),
        new ScalableSize(25, 25),
    };
    
    public QRCodeConfiguration(EVersion version, EECLevel ecLevel, EMaskPattern pattern)
    {
        Size = _sizes[(int)version];
        Build(version, ecLevel, pattern);
    }

    private void Build(EVersion version, EECLevel ecLevel, EMaskPattern pattern)
    {
        PreparePatternsPosition(version);
        SetTimingLength();
    }

    private void SetTimingLength()
    {
        TimingLength = Size.Width - 2 * FinderSizeWithSafeZone;
    }

    private void PreparePatternsPosition(EVersion version)
    {
        var v = (int)version;
        var finderPos = Size.Width - FinderModuleSize;
        var alignmentPos = Size.Width - AlignmentOffset;

        PatternsPosition = new()
        {
            { EPlacement.TopLeft, new Point(0, 0) },
            { EPlacement.BottomLeft, new Point(finderPos, 0) },
            { EPlacement.TopRight, new Point(0, finderPos) },
            { EPlacement.BottomRight,  new Point(alignmentPos, alignmentPos)},
            // Refer to for explanation https://www.thonky.com/qr-code-tutorial/module-placement-matrix#dark-module
            { EPlacement.BottomLeftOffset, new Point(4 * (v + 1) + 9, 8)}
        };
    }
}