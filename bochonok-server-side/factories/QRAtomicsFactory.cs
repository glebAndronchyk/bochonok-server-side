using bochonok_server_side.model;
using bochonok_server_side.Model.Image;
using bochonok_server_side.Model.Image.abstractions;

namespace bochonok_server_side.factories;

public class QRAtomicsFactory
{
  public QRAtomicsFactory()
  { }
  
  public QRAtomic CreateQrModule(byte moduleType, QRSize? size = null)
  {
    return new QRModule(moduleType, size);
  }
}