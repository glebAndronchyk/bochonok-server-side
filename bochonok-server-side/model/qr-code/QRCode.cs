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
    
    public QRCode(string encodeString): base(new ScalableSize(25, 25))
    {
        _encodeString = encodeString;
        _items = Fill();
    }

    public List<List<QRAtomic>> Fill()
    {
        return Enumerable.Range(0, Size.Width)
            .Select(_ => Enumerable.Range(0, Size.Height)
                .Select(_ => QRAtomicsFactory.CreateQrModule(2))
                .ToList())
            .ToList();
    }

    public QRCode Build()
    {
        var  builder = new QRBuilder(this);
        var  encodedString = Encode();
        
        // TODO: format info encoding - refer to https://www.thonky.com/qr-code-tutorial/format-version-tables
        var buildedQr = 
            builder
            .AddPattern(new QRFinderPattern(), QRCodeConfiguration.FinderPatternsPosition[EPlacement.TopLeft], "finder")
            .AddPattern(new QRFinderPattern(), new Point(Size.Width - 7, 0), "finder")
            .AddPattern(new QRFinderPattern(), new Point(0, Size.Height - 7), "finder")
            .AddPattern(new QRAlignmentPattern(), new Point(Size.Width - 9, Size.Height - 9), "alignment")
            .AddModule(new QRModule(1), new Point(4 * 2 + 9, 8), true)
            .AddFindersSafeZone()
            .AddTiming(9)
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