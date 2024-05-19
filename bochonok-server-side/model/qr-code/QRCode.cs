using bochonok_server_side.builders;
using bochonok_server_side.factories;
using bochonok_server_side.model;
using bochonok_server_side.Model.Image.abstractions;
using bochonok_server_side.Model.Image.enums;
using Point = bochonok_server_side.model.utility_classes.Point;

namespace bochonok_server_side.Model.Image;

public class QRCode : QRAtomicGroup<QRAtomic>
{
    public ImageBase Image;
    
    private string _encodeString;
    
    // TODO: add dynamic value for size
    // TODO: add qr version selection
    public QRCode(string encodeString): base(new QRSize(25, 25))
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
        
        // TODO: check finder size without hardcoding
        // TODO: fix axis
        // TODO: format info encoding - refer to https://www.thonky.com/qr-code-tutorial/format-version-tables
        var buildedQR = 
            builder
            .AddPattern(new QRFinderPattern(), new Point(0, 0), "finder")
            .AddPattern(new QRFinderPattern(), new Point(Size.Width - 7, 0), "finder")
            .AddPattern(new QRFinderPattern(), new Point(0, Size.Height - 7), "finder")
            .AddFindersSafeZone()
            .AddPattern(new QRAlignmentPattern(), new Point(Size.Width - 9, Size.Height - 9), "alignment")
            .AddTiming(9)
            .AddFormatInfo("111011111000100")
            .AddModule(new QRModule(1), new Point(4 * 2 + 9, 8), true)
            .AddIterative(encodedString)
            .ApplyMask()
            .AddQrSafeZone()
            .RetrieveItems();
        SetItems(buildedQR);

        return this;
    }

    private string Encode() => QRDataEncoder.EncodeCodewords(_encodeString, EEncodingMode.BYTE, EVersion.V2);
}