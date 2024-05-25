using bochonok_server_side.model.encoding;
using bochonok_server_side.Model.Image.enums;
using bochonok_server_side.model.qr_code.enums;
using Point = bochonok_server_side.model.utility_classes.Point;

namespace bochonok_server_side.model.qr_code.QrCodeConfiguration;

// This is the most stupid shit i've ever seen.

public partial class QRCodeConfiguration
{
    public static readonly Dictionary<EEncodingMode, QREncodingMeta> Encodings = new()
    {
        {
            EEncodingMode.BYTE, new QREncodingMeta
            {
                binaryRepresentation = "0100",
                maxBits = new MaxBitsRepresentation(8, 16, 16),
                encodingMethod = StringEncoder.ByteModeEncode
            }
        }
    };

    public static readonly Dictionary<EVersion, int> ErrorCorrectionPadding = new()
    {
        { EVersion.V2, 7 }
    };

    public readonly Dictionary<EAxisPlacement, Point> TimingPositions = new ()
    {
        { EAxisPlacement.Vertical, new Point(FinderSizeWithSafeZone, 6)},
        { EAxisPlacement.Horizontal, new Point(6, FinderSizeWithSafeZone)},
    };
    
    private readonly List<QRBlockInformation> _blocksInfo = new()
    {
        new()
        {
            version = EVersion.V2,
            errorCorrection = EECLevel.L,
            totalCW = 34,
            ecPerBlock = 10,
            numberOfBlocks = 1,
            dataCodewordsPerBlock = 34,
        }
    };
    private readonly List<ScalableSize> _sizes = new ()
    {
        new ScalableSize(10, 10),
        new ScalableSize(25, 25),
    };
}