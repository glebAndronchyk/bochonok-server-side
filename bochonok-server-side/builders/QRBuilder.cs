using bochonok_server_side.Model.Image;
using bochonok_server_side.Model.Image.abstractions;
using bochonok_server_side.Model.Image.enums;
using bochonok_server_side.Model.Image.interfaces;
using bochonok_server_side.model.utility_classes;
using Point = bochonok_server_side.model.utility_classes.Point;

namespace bochonok_server_side.builders;

// TODO: think how this class can be divided into smaller classes

public class QRBuilder
{
  private QRAtomicGroup<QRAtomic> _qrCtx;
  private List<List<QRAtomic>> _items;
  
  private List<AreaCoordinates> _maskSafeZones = new ();
  
  public QRBuilder(QRAtomicGroup<QRAtomic> qrCtx)
  {
    _qrCtx = qrCtx;
    _items = _qrCtx.GetAtomicItems();
  }

  public QRBuilder AddPattern(IQRPattern pattern, Point start)
  {
    pattern.Build();
    var patternSize = pattern.GetSize().GetDividedSize();
    var x = start.GetX();
    var y = start.GetY();
    var endPoint = new Point(x + patternSize.Width, y + patternSize.Height);
    
    _maskSafeZones.Add(new AreaCoordinates(start, endPoint));
    
    for (int i = x; i < endPoint.GetX(); i++)
    {
      for (int j = y; j < endPoint.GetY(); j++)
      {
        _items[i][j] = (QRAtomic)pattern.GetAtomicItems()[i - x][j - y].Clone();
      }
    }
    
    return this;
  }

  public QRBuilder AddModule(QRModule module, Point pos)
  {
    _items[pos.GetX()][pos.GetY()] = module;

    return this;
  }

  public QRBuilder AddIterative(string bits)
  {
    int x = _qrCtx.Size.Width - 1;
    int y = _qrCtx.Size.Height;
    int bitCounter = 0;
    var direction = EFillDirection.Upwards;

    _items[x][y - 1] = GetNewModule(bits, ref bitCounter);
    
    while (x >= 0)
    {
      y = direction == EFillDirection.Upwards ? y - 1 : y + 1;
      
      if (y is -1 or 25)
      {
        if (x == 0)
        {
          break;
        }

        x -= 2;
        direction = direction == EFillDirection.Upwards ?
            EFillDirection.Downwards :
            EFillDirection.Upwards;
      }
      else
      {
        var yModulePos = GetYModulePositionByDirection(x, y, direction);
        var xModuleAbscissa = x - 1 < 0 ? 0 : x - 1;
        var nextXModule = (QRModule)_items[y][xModuleAbscissa];
        var nextYModule = (QRModule)_items[yModulePos.Item2][yModulePos.Item1];

        var xModuleProcessResult = ProcessNextModule(nextXModule, bits, ref bitCounter, xModuleAbscissa, y);
        if (xModuleProcessResult != null) return xModuleProcessResult;
        
        var yModuleProcessResult = ProcessNextModule(nextYModule, bits, ref bitCounter, yModulePos.Item1, yModulePos.Item2);
        if (yModuleProcessResult != null) return yModuleProcessResult;
      }
    }

    return this;
  }

  public QRBuilder ApplyMask()
  {
    /* This is level 7 mask condition. In order to create normally
     working qr code, you should calculate penalty for every 8 masks
     and then choose one with lowest total penalty. Please refer to 
     https://www.thonky.com/qr-code-tutorial/data-masking
     and
     https://www.thonky.com/qr-code-tutorial/mask-patterns
    */
    Func<Point, bool> maskCondition = p => (((p.GetX() + p.GetY()) % 2) + ((p.GetX() * p.GetY()) % 3))  % 2 == 0;

    for (int i = 0; i < _items.Count; i++)
    {
      for (int j = 0; j < _items.Count; j++)
      {
        bool isSafe = _maskSafeZones.Any(zone =>
        {
          var topLeft = zone.TopLeftCorner;
          var bottomRight = zone.BottomRightCorner;
          var isXSafe = i >= topLeft.GetX() && i <= bottomRight.GetX();
          var isYSafe = j >= topLeft.GetY() && j <= bottomRight.GetY();
            
          return isXSafe && isYSafe;
        });
        
        if (!isSafe)
        {
          bool shouldChangeBit = maskCondition(new Point(i, j));

          if (shouldChangeBit)
          {
            _items[i][j] = ((QRModule)_items[i][j]).ReverseBit();
          }
        }
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
      nextX = nextX > 1 ? nextX - 2 : 0;
    }
    
    return new (nextX, nextY);
  }
}