using bochonok_server_side.model;
using bochonok_server_side.Model.Image;
using bochonok_server_side.Model.Image.abstractions;

namespace bochonok_server_side.factories;

public class QRAtomicsFactory
{
  public static QRAtomic CreateQrModule(byte moduleType, QRSize? size = null)
  {
    return new QRModule(moduleType, size);
  }

  public static List<QRAtomic> CreateQrLine(int length)
  {
    return Enumerable.Range(0, length).Select(_ => CreateQrModule((byte)0)).ToList();
  }
}