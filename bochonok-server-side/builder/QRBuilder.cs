using bochonok_server_side.Model.Image;
using bochonok_server_side.Model.Image.abstractions;
using bochonok_server_side.Model.Image.interfaces;

namespace bochonok_server_side.builder;

public class QRBuilder
{
  private QRAtomicGroup<QRAtomic> _qrCtx;
  private List<List<QRAtomic>> _items;
  
  public QRBuilder(QRAtomicGroup<QRAtomic> qrCtx)
  {
    _qrCtx = qrCtx;
    _items = _qrCtx.GetAtomicItems();
  }

  public QRBuilder AddPattern(IQRPattern pattern,  int x, int y)
  {
    pattern.Build();
    var patternSize = pattern.GetSize().GetDividedSize();
    
    for (int i = x; i < x + patternSize.Width; i++)
    {
      for (int j = y; j < y + patternSize.Height; j++)
      {
        _items[i][j] = (QRAtomic)pattern.GetAtomicItems()[i - x][j - y].Clone();
      }
    }
    
    return this;
  }

  public QRBuilder AddIterative()
  {
    return this;
  }

  public List<List<QRAtomic>> RetrieveItems()
  {
    return _items;
  }
}