using bochonok_server_side.Model.Image.enums;
using bochonok_server_side.model.qr_code.enums;
using Point = bochonok_server_side.model.utility_classes.Point;

namespace bochonok_server_side.model.qr_code.QrCodeConfiguration;

public partial class QRCodeConfiguration
{
    public readonly EVersion Version;
    public readonly EECLevel ErrorCorrection;
    public readonly EEncodingMode Encoding;
    
    public QRBlockInformation BlockInformation;
    public int TimingLength { get; private set; }
    public Dictionary<EPlacement, Point> PatternsPosition { get; private set; }
    public ScalableSize Size { get; private set; }
    public QREncodingMeta EncodingMeta { get; private set; }
}