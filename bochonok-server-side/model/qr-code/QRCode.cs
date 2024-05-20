using bochonok_server_side.builders;
using bochonok_server_side.factories;
using bochonok_server_side.Model.Image;
using bochonok_server_side.Model.Image.enums;
using bochonok_server_side.model.qr_code.abstractions;
using bochonok_server_side.model.qr_code.enums;
using bochonok_server_side.model.qr_code.QrCodeConfiguration;

namespace bochonok_server_side.model.qr_code;

public class QRCode : QRAtomicGroup<QRAtomic>
{
    public ImageBase Image;
    
    private string _encodeString;
    private QRCodeConfiguration _cfg;
    
    public QRCode(string encodeString)
    {
        _cfg = new QRCodeConfiguration(EVersion.V2, EECLevel.L, EEncodingMode.BYTE);
        Size = _cfg.Size;
        _encodeString = encodeString;
        _items = Fill();
    }

    public List<List<QRAtomic>> Fill()
    {
        return Enumerable.Range(0, Size.Width)
            .Select(_ => Enumerable.Range(0, Size.Height)
                .Select(_ => QRAtomicsFactory.CreateQrModule(EModuleType.Red))
                .ToList())
            .ToList();
    }

    public QRCode Build()
    {
        var  builder = new QRBuilder(_cfg, GetAtomicItems());
        var  encodedString = Encode();
        
        var buildedQR = 
            builder
            .AddPattern(new QRFinderPattern(), _cfg.PatternsPosition[EPlacement.TopLeft], "finder")
            .AddPattern(new QRFinderPattern(),  _cfg.PatternsPosition[EPlacement.BottomLeft], "finder")
            .AddPattern(new QRFinderPattern(),  _cfg.PatternsPosition[EPlacement.TopRight], "finder")
            .AddPattern(new QRAlignmentPattern(), _cfg.PatternsPosition[EPlacement.BottomRight], "alignment")
            .AddModule(new QRModule(EModuleType.Black), _cfg.PatternsPosition[EPlacement.BottomLeftOffset], true)
            .AddFindersSafeZone()
            .AddTiming()
            // This part of code is incorrect. Firstly i should determine best mask then i should proceed with format info
            // so let it be as it is.... for now... maybe one day :D.
            // about format info - refer to https://www.thonky.com/qr-code-tutorial/format-version-tables
            .AddFormatInfo("111011111000100")
            .AddIterative(encodedString)
            .ApplyMask()
            //
            .AddQrQuiteZone()
            .RetrieveItems();
        SetItems(buildedQR);

        return this;
    }

    private string Encode()
    {
        return QRDataEncoder.EncodeCodewords(_encodeString, _cfg.BlockInformation, _cfg.EncodingMeta);
    }
}