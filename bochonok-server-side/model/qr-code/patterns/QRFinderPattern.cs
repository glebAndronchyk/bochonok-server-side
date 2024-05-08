using bochonok_server_side.model;
using bochonok_server_side.Model.Image.abstractions;
using bochonok_server_side.Model.Image.interfaces;

namespace bochonok_server_side.Model.Image;

public class QRFinderPattern : QRAtomicGroup<QRAtomic>, IQRPattern
{
  public QRFinderPattern() : base(new QRSize(7, 7, 5))
  { }

  public IQRPattern Build()
  {
    // 7 * 5 = 35 => 35x35px;
    var layout = BuildLayout();
    SetItems(layout);

    return this;
  }
  
  public QRSize GetSize()
  {
    return Size;
  }
  
  // TODO: add pattern class and separate this pattern from QRFInderPatter in order to optimize memory usage
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