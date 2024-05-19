using bochonok_server_side.model.qr_code;
using bochonok_server_side.model.qr_code.abstractions;

namespace bochonok_server_side.factories;

public class QRAtomicsFactory
{
  public static QRAtomic CreateQrModule(byte moduleType, ScalableSize? size = null)
  {
    return new QRModule(moduleType, size);
  }

  public static List<QRAtomic> CreateQrLine(int length)
  {
    return Enumerable.Range(0, length).Select(_ => CreateQrModule((byte)0)).ToList();
  }
}