using bochonok_server_side.model;
using bochonok_server_side.Model.Image.interfaces;
using bochonok_server_side.model.qr_code;
using bochonok_server_side.model.qr_code.abstractions;

namespace bochonok_server_side.Model.Image;

public class QRFinderPattern : QRAtomicGroup<QRAtomic>, IQRPattern
{
  public QRFinderPattern() : base(new ScalableSize(7, 7, 5))
  { }

  public IQRPattern Build()
  {
    var layout = BuildLayout();
    SetItems(layout);

    return this;
  }

  public ScalableSize GetSize() => Size;
  
  public List<List<QRAtomic>> BuildLayout()
  {
    return new List<List<QRAtomic>>
    {
      new () { new QRModule(1), new QRModule(1), new QRModule(1), new QRModule(1), new QRModule(1), new QRModule(1), new QRModule(1) },
      new () { new QRModule(1), new QRModule(0), new QRModule(0), new QRModule(0), new QRModule(0), new QRModule(0), new QRModule(1) },
      new () { new QRModule(1), new QRModule(0), new QRModule(1), new QRModule(1), new QRModule(1), new QRModule(0), new QRModule(1) },
      new () { new QRModule(1), new QRModule(0), new QRModule(1), new QRModule(1), new QRModule(1), new QRModule(0), new QRModule(1) },
      new () { new QRModule(1), new QRModule(0), new QRModule(1), new QRModule(1), new QRModule(1), new QRModule(0), new QRModule(1) },
      new () { new QRModule(1), new QRModule(0), new QRModule(0), new QRModule(0), new QRModule(0), new QRModule(0), new QRModule(1) },
      new () { new QRModule(1), new QRModule(1), new QRModule(1), new QRModule(1), new QRModule(1), new QRModule(1), new QRModule(1) }
    };
  }
}