using bochonok_server_side.model._errors;
using bochonok_server_side.model.qr_code.abstractions;
using bochonok_server_side.model.qr_code.enums;

namespace bochonok_server_side.model.qr_code;

public class QRModule : QRAtomic
{
  public EModuleType Type;
  
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
      throw new QRCodeOverflowException("Bit wasn't set for module.");
    }

    return new QRModule(Type == EModuleType.Black ? EModuleType.White : EModuleType.Black);
  }
}
