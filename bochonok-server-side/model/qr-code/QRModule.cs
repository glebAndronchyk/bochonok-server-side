using bochonok_server_side.Model.Image.abstractions;

namespace bochonok_server_side.Model.Image;

public class QRModule : QRAtomic
{
  public byte Type;
  
  // TODO: add different color support and implement decorator pattern for easier use.
  public QRModule(byte moduleType, Tuple<int, int>? size = null) : base(size)
  {
    if (moduleType > 2)
    {
      throw new NotImplementedException("Can't use this number for locating module type.");
    }
    
    Type = moduleType;
    _bytesMatrix.Add(moduleType); 
  }
}
