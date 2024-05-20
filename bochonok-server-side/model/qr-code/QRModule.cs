using bochonok_server_side.model.qr_code.abstractions;
using bochonok_server_side.model.qr_code.enums;

namespace bochonok_server_side.model.qr_code;

public class QRModule : QRAtomic
{
  public EModuleType Type;
  
  // TODO: add different color support and implement decorator pattern for easier use.
  public QRModule(EModuleType moduleType, ScalableSize? size = null) : base(size)
  {
    var byteModule = (byte)moduleType;
    Type = moduleType;
    _bytesMatrix.Add(byteModule); 
  }

  public QRModule ReverseBit()
  {
    if (Type == EModuleType.Red)
    {
      throw new Exception("Bit wasn't set for module.");
    }

    return new QRModule(Type == EModuleType.Black ? EModuleType.White : EModuleType.Black);
  }
}
