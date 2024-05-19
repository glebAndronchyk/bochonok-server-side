using bochonok_server_side.model.qr_code.abstractions;

namespace bochonok_server_side.model.qr_code;

public class QRModule : QRAtomic
{
  public byte Type;
  
  // TODO: add different color support and implement decorator pattern for easier use.
  public QRModule(byte moduleType, ScalableSize? size = null) : base(size)
  {
    if (moduleType > 3)
    {
      throw new NotImplementedException("Can't use this number for locating module type.");
    }
    
    Type = moduleType;
    _bytesMatrix.Add(moduleType); 
  }

  public QRModule ReverseBit()
  {
    if (Type >= 2)
    {
      throw new Exception("Bit wasn't set for module.");
    }

    return new QRModule(Convert.ToByte(!Convert.ToBoolean(Type)));
  }
}
