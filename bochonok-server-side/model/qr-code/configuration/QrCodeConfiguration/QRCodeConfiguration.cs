using bochonok_server_side.Model.Image.enums;
using bochonok_server_side.model.qr_code.enums;

namespace bochonok_server_side.model.qr_code.QrCodeConfiguration;

public partial class QRCodeConfiguration
{
    // At least i tried to make it better.
    // please kill me xD
    public QRCodeConfiguration(EVersion version, EECLevel ecLevel, EEncodingMode mode)
    {
        Version = version;
        ErrorCorrection = ecLevel;
        Encoding = mode;
        EncodingMeta = Encodings[Encoding];
        Size = _sizes[(int)version];
        PrepareBlockInfo();
        PreparePatternsPosition();
        SetTimingLength();
    }
}