using bochonok_server_side.factories;
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
  
  private List<DescribedArea> _maskSafeZones = new ();
  
  public QRBuilder(QRAtomicGroup<QRAtomic> qrCtx)
  {
    _qrCtx = qrCtx;
    _items = _qrCtx.GetAtomicItems();
  }

  public QRBuilder AddPattern(IQRPattern pattern, Point start, string name)
  {
    pattern.Build();
    var patternSize = pattern.GetSize().GetDividedSize();
    var x = start.GetX();
    var y = start.GetY();
    var endPoint = new Point(x + patternSize.Width, y + patternSize.Height);
    
    _maskSafeZones.Add(new DescribedArea(start, endPoint, name));
    
    for (int i = x; i < endPoint.GetX(); i++)
    {
      for (int j = y; j < endPoint.GetY(); j++)
      {
        _items[i][j] = (QRAtomic)pattern.GetAtomicItems()[i - x][j - y].Clone();
      }
    }
    
    return this;
  }

  public QRBuilder AddTiming(int length)
  {
    Point verticalStart = new (8, 6);
    Point horizontalStart = new (6, 8);

    string timingBits = string.Join("", Enumerable.Range(0, length).Select((_, i) => i % 2 == 0 ? "1" : "0"));
    
    PlaceBitsInSequence(timingBits,
      verticalStart,
      horizontalStart,
      new (1, 1)
    );

    return this;
  }

  public QRBuilder AddFindersSafeZone()
  {
    var finderAreas = _maskSafeZones.Where(zone => zone.Name == "finder").ToList();
    var safeZoneBits = String.Join("", Enumerable.Range(0, 8).Select(_ => "0"));
    
    foreach (var area in finderAreas)
    {
      Point verticalStart = new (0, 0);
      Point horizontalStart = new (0, 0);
      var topLeftCoord = area.TopLeftCorner;
      
      switch (topLeftCoord.Y)
      {
        case 0:
          verticalStart = new (0, 7);
          horizontalStart = new (7, 0);
          break;
        case 18:
          verticalStart = new (0, topLeftCoord.Y - 1);
          horizontalStart = new (topLeftCoord.X + 7, topLeftCoord.Y - 1);
          break;
      }

      if (topLeftCoord.X is > 0 and < 24)
      {
        verticalStart = new (topLeftCoord.X - 1, topLeftCoord.Y + 7);
        horizontalStart = new (topLeftCoord.X - 1, 0);
      }
      
      PlaceBitsInSequence(safeZoneBits,
        verticalStart,
        horizontalStart,
        new (1, 1),
        true
      );
    }

    return this;
  }

  public QRBuilder AddModule(QRModule module, Point pos, bool protectFromMask = false)
  {
    if (protectFromMask)
    {
      _maskSafeZones.Add(new DescribedArea(pos, pos));
    }

    _items[pos.GetX()][pos.GetY()] = module;

    return this;
  }

  public QRBuilder AddIterative(string bits)
  {
    int x = _qrCtx.Size.Width - 1;
    int y = _qrCtx.Size.Height;
    var direction = EFillDirection.Upwards;
    var bitsContainer = new BitsContainer(bits, true);
    var verticalTimingPosition = 8;
    
    _items[x][y - 1] = GetNewModule(bitsContainer);
    
    while (x >= 0)
    {
      y = direction == EFillDirection.Upwards ? y - 1 : y + 1;
      
      if (y is -1 or 25)
      {
        if (x == 0)
        {
          break;
        }

        x -= x == verticalTimingPosition ? 3 : 2;
        direction = direction == EFillDirection.Upwards ?
            EFillDirection.Downwards :
            EFillDirection.Upwards;
      }
      else
      {
        var yModulePos = GetYModulePositionByDirection(x, y, direction);
        var xModuleAbscissa = x - 1 < 0 ? 0 : x - 1;
        var nextXModule = (QRModule)_items[y][xModuleAbscissa];
        var nextYModule = (QRModule)_items[yModulePos.Y][yModulePos.X];

        var xModuleProcessResult = ProcessNextModule(nextXModule, bitsContainer, new Point(xModuleAbscissa, y));
        if (!xModuleProcessResult) return this;
        
        var yModuleProcessResult = ProcessNextModule(nextYModule, bitsContainer, new Point(yModulePos.X, yModulePos.Y));
        if (!yModuleProcessResult) return this;
      }
    }

    return this;
  }

  public QRBuilder AddFormatInfo(string formatInfo)
  {
    var zeroToSix = formatInfo.Substring(0, 7);
    var sixToFourteenth = formatInfo.Substring(6, 8);
    
    
    PlaceBitsInSequence(zeroToSix,
      new Point(_qrCtx.Size.Height - 1, 8),
      new Point(8, 0),
      new Point(-1, 1)
      );
    PlaceBitsInSequence(
      sixToFourteenth,
      new Point(8, 8),
      new Point(8, _qrCtx.Size.Height - 8),
      new Point(-1, 1)
    );

    return this;
  }

  private void PlaceBitsInSequence(string zeroToSix, Point verticalStart, Point horizontalStart, Point increasers, bool ignoreModuleSkip = false)
  {
    // TODO: point and size same signature
    var verticalCopy = new Point(verticalStart);
    var horizontalCopy = new Point(horizontalStart);
    var bitsContainer = new BitsContainer(zeroToSix, false);
    
    while (bitsContainer.HasNext())
    {
      var xCoord = verticalCopy.X;
      var yCoord = horizontalCopy.Y;
      
      var nextVerticalModule = (QRModule)_items[xCoord][verticalCopy.Y];
      var nextHorizontalModule = (QRModule)_items[horizontalCopy.X][yCoord];

      if (nextVerticalModule.Type is 0 or 1 && !ignoreModuleSkip)
      {
        xCoord += increasers.X;
      }
      
      if (nextHorizontalModule.Type is 0 or 1 && !ignoreModuleSkip)
      {
        yCoord += increasers.Y;
      }
      
      var verticalSafeZonePoint = new Point(xCoord, verticalCopy.Y);
      var horizontalSafeZonePoint = new Point(horizontalCopy.X, yCoord);
      _maskSafeZones.Add(new DescribedArea(verticalSafeZonePoint, verticalSafeZonePoint));
      _maskSafeZones.Add(new DescribedArea(horizontalSafeZonePoint, horizontalSafeZonePoint));

      _items[xCoord][verticalCopy.Y] = GetNewModule(bitsContainer);
      _items[horizontalCopy.X][yCoord] = GetNewModule(bitsContainer);
      
      verticalCopy.X += increasers.X;
      horizontalCopy.Y += increasers.Y;
      bitsContainer.TryIncrementCounter(true);
    }
  }

  public QRBuilder ApplyMask()
  {
    /* This is level 0 mask condition. In order to create normally
     working qr code, you should calculate penalty for every 8 masks
     and then choose one with lowest total penalty. Please refer to 
     https://www.thonky.com/qr-code-tutorial/data-masking
     and
     https://www.thonky.com/qr-code-tutorial/mask-patterns
    */
    Func<Point, bool> maskCondition = p => (p.X + p.Y)  % 2 == 0;

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
  
  private bool ProcessNextModule(QRModule module, BitsContainer bitsContainer, Point pos)
  {
    if (module.Type == 2)
    {
      if (!bitsContainer.HasNext())
      {
        return false;
      }
      
      _items[pos.GetY()][pos.GetX()] = GetNewModule(bitsContainer);
    }

    return true;
  }
  
  private QRModule GetNewModule(BitsContainer bitsContainer)
  {
    var currentBit = bitsContainer.GetCurrent();
    bitsContainer.TryIncrementCounter();
    
    return new QRModule(byte.Parse(currentBit));
  }
  
  private Point GetYModulePositionByDirection(int x, int y, EFillDirection direction)
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

  public QRBuilder AddQrSafeZone()
  {
    var safeAreaLength = 2;
    
    foreach (var row in _items)
    {
      for (int i = 0; i < safeAreaLength; i++)
      {
        row.Insert(0, QRAtomicsFactory.CreateQrModule(0));
        row.Add(QRAtomicsFactory.CreateQrModule(0));
      }
    }
    
    for (int i = 0; i < safeAreaLength; i++)
    {
      _items.Insert(0, QRAtomicsFactory.CreateQrLine(_items.Count));
      _items.Add(QRAtomicsFactory.CreateQrLine(_items.Count));
    }
    
    return this;
  }
}