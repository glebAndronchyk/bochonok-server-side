using bochonok_server_side.model.qr_code;
using bochonok_server_side.model.qr_code.abstractions;
using bochonok_server_side.model.qr_code.enums;

namespace bochonok_server_side.factories;

public class QRAtomicsFactory
{
  public static QRAtomic CreateQrModule(EModuleType moduleType, ScalableSize? size = null)
  {
    return new QRModule(moduleType, size);
  }

  public static List<QRAtomic> CreateQrLine(int length)
  {
    return Enumerable.Range(0, length).Select(_ => CreateQrModule(EModuleType.White)).ToList();
  }
}