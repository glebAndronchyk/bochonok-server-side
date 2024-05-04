using bochonok_server_side.Model.Image;
using bochonok_server_side.Model.Image.abstractions;
using bochonok_server_side.Model.Image.enums;
using bochonok_server_side.Model.Image.interfaces;

namespace bochonok_server_side.builder;

// TODO: think how this class can be divided into smaller classes

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

  public QRBuilder AddIterative(string bits)
  {
    int x = _qrCtx.Size.Width - 1;
    int y = _qrCtx.Size.Height;
    int bitCounter = 0;
    var direction = EFillDirection.Upwards;

    _items[x][y - 1] = GetNewModule(bits, ref bitCounter);
    
    while (x > 2)
    {
      y = direction == EFillDirection.Upwards ? y - 1 : y + 1;
      
      if (y is -1 or 25)
      {
        x -= 2;
        direction = direction == EFillDirection.Upwards ?
            EFillDirection.Downwards :
            EFillDirection.Upwards;
      }
      else
      {
        var yModulePos = GetYModulePositionByDirection(x, y, direction);
        // TODO: create point class
        var nextXModule = (QRModule)_items[y][x - 1];
        var nextYModule = (QRModule)_items[yModulePos.Item2][yModulePos.Item1];

        var xModuleProcessResult = ProcessNextModule(nextXModule, bits, ref bitCounter, x - 1, y);
        if (xModuleProcessResult != null) return xModuleProcessResult;
        
        var yModuleProcessResult = ProcessNextModule(nextYModule, bits, ref bitCounter, yModulePos.Item1, yModulePos.Item2);
        if (yModuleProcessResult != null) return yModuleProcessResult;
      }
    }

    return this;
  }

  public List<List<QRAtomic>> RetrieveItems()
  {
    return _items;
  }
  
  private QRBuilder? ProcessNextModule(QRModule module, string bits, ref int bitCounter, int x, int y)
  {
    if (module.Type == 2)
    {
      if (bitCounter >= bits.Length)
      {
        return this;
      }
      
      _items[y][x] = GetNewModule(bits, ref bitCounter);
    }
    
    return null;
  }
  
  private QRModule GetNewModule(string bits, ref int bitCounter)
  {
    return new QRModule(byte.Parse(bits[bitCounter++].ToString()));
  }
  
  private Tuple<int, int> GetYModulePositionByDirection(int x, int y, EFillDirection direction)
  {
    var nextY = direction == EFillDirection.Upwards ? y - 1 : y + 1;
    var nextX = x;
    
    if (nextY is -1 or 25)
    {
      nextY = nextY > 0 ? nextY - 1 : nextY + 1;
      nextX -= 2;
    }
    
    return new (nextX, nextY);
  }
}