using bochonok_server_side.builders;
using bochonok_server_side.factories;
using bochonok_server_side.Model.Image;
using bochonok_server_side.Model.Image.enums;
using bochonok_server_side.model.qr_code.abstractions;
using bochonok_server_side.model.qr_code.enums;
using Point = bochonok_server_side.model.utility_classes.Point;

namespace bochonok_server_side.model.qr_code;

public class QRCode : QRAtomicGroup<QRAtomic>
{
    public ImageBase Image;
    
    private string _encodeString;
    private QRCodeConfiguration _cfg;

    
    public QRCode(string encodeString): base(new ScalableSize(25, 25))
    {
        _cfg = new QRCodeConfiguration(EVersion.V2, EECLevel.L, EMaskPattern.L0);   
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
        
        // TODO: format info encoding - refer to https://www.thonky.com/qr-code-tutorial/format-version-tables
        var buildedQr = 
            builder
            .AddPattern(new QRFinderPattern(), _cfg.PatternsPosition[EPlacement.TopLeft], "finder")
            .AddPattern(new QRFinderPattern(),  _cfg.PatternsPosition[EPlacement.BottomLeft], "finder")
            .AddPattern(new QRFinderPattern(),  _cfg.PatternsPosition[EPlacement.TopRight], "finder")
            .AddPattern(new QRAlignmentPattern(), _cfg.PatternsPosition[EPlacement.BottomRight], "alignment")
            .AddModule(new QRModule(EModuleType.Black), _cfg.PatternsPosition[EPlacement.BottomLeftOffset], true)
            .AddFindersSafeZone()
            .AddTiming()
            .AddFormatInfo("111011111000100")
            .AddIterative(encodedString)
            .ApplyMask()
            .AddQrSafeZone()
            .RetrieveItems();
        SetItems(buildedQr);

        return this;
    }

    private string Encode() => QRDataEncoder.EncodeCodewords(_encodeString, EEncodingMode.BYTE, EVersion.V2);
}