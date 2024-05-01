using bochonok_server_side.Model.Image.abstractions;

namespace bochonok_server_side.Model.Image;

public class QRFinderPattern : QRAtomicGroup<QRAtomic>
{
  private List<List<QRAtomic>> _patternLayout;
  
  public QRFinderPattern() : base(new Tuple<int, int>(7,7), 5)
  {
    // 7 * 5 = 35 => 35x35px;
    var layout = GetLayout();
    SetItems(layout);
    _patternLayout = layout;
  }
  
  // TODO: add pattern class and separate this pattern from QRFInderPatter in order to optimize memory usage
  private List<List<QRAtomic>> GetLayout()
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