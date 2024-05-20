using bochonok_server_side.Model.Image.interfaces;
using bochonok_server_side.model.qr_code;
using bochonok_server_side.model.qr_code.abstractions;
using bochonok_server_side.model.qr_code.enums;

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
  
  // OMG this is so bad :(
  
  public List<List<QRAtomic>> BuildLayout()
  {
    return new List<List<QRAtomic>>
    {
      new () { new QRModule(EModuleType.Black), new QRModule(EModuleType.Black), new QRModule(EModuleType.Black), new QRModule(EModuleType.Black), new QRModule(EModuleType.Black), new QRModule(EModuleType.Black), new QRModule(EModuleType.Black) },
      new () { new QRModule(EModuleType.Black), new QRModule(EModuleType.White), new QRModule(EModuleType.White), new QRModule(EModuleType.White), new QRModule(EModuleType.White), new QRModule(EModuleType.White), new QRModule(EModuleType.Black) },
      new () { new QRModule(EModuleType.Black), new QRModule(EModuleType.White), new QRModule(EModuleType.Black), new QRModule(EModuleType.Black), new QRModule(EModuleType.Black), new QRModule(EModuleType.White), new QRModule(EModuleType.Black) },
      new () { new QRModule(EModuleType.Black), new QRModule(EModuleType.White), new QRModule(EModuleType.Black), new QRModule(EModuleType.Black), new QRModule(EModuleType.Black), new QRModule(EModuleType.White), new QRModule(EModuleType.Black) },
      new () { new QRModule(EModuleType.Black), new QRModule(EModuleType.White), new QRModule(EModuleType.Black), new QRModule(EModuleType.Black), new QRModule(EModuleType.Black), new QRModule(EModuleType.White), new QRModule(EModuleType.Black) },
      new () { new QRModule(EModuleType.Black), new QRModule(EModuleType.White), new QRModule(EModuleType.White), new QRModule(EModuleType.White), new QRModule(EModuleType.White), new QRModule(EModuleType.White), new QRModule(EModuleType.Black) },
      new () { new QRModule(EModuleType.Black), new QRModule(EModuleType.Black), new QRModule(EModuleType.Black), new QRModule(EModuleType.Black), new QRModule(EModuleType.Black), new QRModule(EModuleType.Black), new QRModule(EModuleType.Black) }
    };
  }
}