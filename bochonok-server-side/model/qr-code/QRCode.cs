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
    private QRAtomicsFactory _factory = new ();
    
    // TODO: add dynamic value for size
    public QRCode(string encodeString): base(new QRSize(25, 25))
    {
        _encodeString = encodeString;
        _items = GetFilled();
    }

    public List<List<QRAtomic>> GetFilled()
    {
        return Enumerable.Range(0, Size.Width)
            .Select(_ => Enumerable.Range(0, Size.Height)
                .Select(_ => _factory.CreateQrModule(2))
                .ToList())
            .ToList();
    }

    public void Build()
    {
        var  builder = new QRBuilder(this);
        var  encodedString = Encode();

        // TODO: check finder size without hardcoding
        var buildedQR = builder
            .AddPattern(new QRFinderPattern(), new Point(0, 0))
            .AddPattern(new QRFinderPattern(), new Point(Size.Width - 7, 0))
            .AddPattern(new QRFinderPattern(), new Point(0, Size.Height - 7))
            .AddPattern(new QRAlignmentPattern(), new Point(Size.Width - 9, Size.Height - 9))
            .AddIterative(encodedString)
            // .ApplyMask()
            .RetrieveItems();
        // .AddPattern(new QRTimingPattern())
        // .AddPattern(new QRTimingPattern())
        // .AddIterative()
        // .AddIterative()
        SetItems(buildedQR);
    }

    private string Encode() => QRDataEncoder.EncodeCodewords(_encodeString, EEncodingMode.BYTE, EVersion.V2);
}