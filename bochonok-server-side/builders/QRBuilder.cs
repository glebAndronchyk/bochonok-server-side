using bochonok_server_side.factories;
using bochonok_server_side.Model.Image.enums;
using bochonok_server_side.Model.Image.interfaces;
using bochonok_server_side.model.qr_code;
using bochonok_server_side.model.qr_code.abstractions;
using bochonok_server_side.model.qr_code.enums;
using bochonok_server_side.model.utility_classes;
using Point = bochonok_server_side.model.utility_classes.Point;

namespace bochonok_server_side.builders;

// TODO: think how this class can be divided into smaller classes

public class QRBuilder
{
  private QRCodeConfiguration _cfg;
  private List<List<QRAtomic>> _items;
  
  private List<DescribedArea> _maskSafeZones = new ();
  
  public QRBuilder(QRCodeConfiguration cfg, List<List<QRAtomic>> items)
  {
    _cfg = cfg;
    _items = items;
  }

  public QRBuilder AddPattern(IQRPattern pattern, Point start, string name)
  {
    pattern.Build();
    var patternSize = pattern.GetSize().GetDividedSize();
    var x = start.X;
    var y = start.Y;
    var endPoint = new Point(x + patternSize.Width, y + patternSize.Height);
    
    _maskSafeZones.Add(new DescribedArea(start, endPoint, name));
    
    for (int i = x; i < endPoint.X; i++)
    {
      for (int j = y; j < endPoint.Y; j++)
      {
        _items[i][j] = (QRAtomic)pattern.GetAtomicItems()[i - x][j - y].Clone();
      }
    }
    
    return this;
  }

  public QRBuilder AddTiming()
  {
    string timingBits = string.Join("", 
      Enumerable.Range(0, _cfg.TimingLength)
        .Select((_, i) => i % 2 == 0 ? "1" : "0")
    );
    
    PlaceBitsInSequence(timingBits,
      _cfg.TimingPositions[EAxisPlacement.Vertical],
      _cfg.TimingPositions[EAxisPlacement.Horizontal],
      new (1, 1)
    );

    return this;
  }

  public QRBuilder AddFindersSafeZone()
  {
    var finderAreas = _maskSafeZones.Where(zone => zone.Name == "finder").ToList();
    var finderModuleSize = QRCodeConfiguration.FinderModuleSize;
    var amountOfBits = finderModuleSize + 1;
    var size = _cfg.Size.Width;
    var rightOffset = size - finderModuleSize;
    var safeZoneBits = String.Join("", Enumerable.Range(0, amountOfBits).Select(_ => "0"));

    foreach (var area in finderAreas)
    {
      Point verticalStart = new (0, 0);
      Point horizontalStart = new (0, 0);
      var topLeftCoord = area.TopLeftCorner;

      if (topLeftCoord.Y == 0)
      {
        verticalStart = new Point(0, finderModuleSize);
        horizontalStart = new Point(finderModuleSize, 0);
      }
      else if (topLeftCoord.Y == rightOffset)
      {
        verticalStart = new Point(0, topLeftCoord.Y - 1);
        horizontalStart = new Point(topLeftCoord.X + finderModuleSize, topLeftCoord.Y - 1);
      }

      if (topLeftCoord.X > 0 && topLeftCoord.X < size - 1)
      {
        verticalStart = new Point(topLeftCoord.X - 1, topLeftCoord.Y + finderModuleSize);
        horizontalStart = new Point(topLeftCoord.X - 1, 0);
      }

      PlaceBitsInSequence(safeZoneBits,
        verticalStart,
        horizontalStart,
        new Point(1, 1),
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

    _items[pos.X][pos.Y] = module;

    return this;
  }

  public QRBuilder AddIterative(string bits)
  {
    int x = _items.Count - 1;
    int y = _items.Count;
    var direction = EFillDirection.Upwards;
    var bitsContainer = new BitsContainer(bits, true);
    var verticalTimingPosition = 8;
    var defaultXSkip = 2;
    var timingXSkip = 3;
    
    _items[x][y - 1] = GetNewModule(bitsContainer);
    
    while (x >= 0)
    {
      y = direction == EFillDirection.Upwards ? y - 1 : y + 1;
      
      if (y == -1 || y == _cfg.Size.Width)
      {
        if (x == 0)
        {
          break;
        }

        x -= x == verticalTimingPosition ? timingXSkip : defaultXSkip;
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
    var finderSizeWithCOnfiguration = QRCodeConfiguration.FinderSizeWithSafeZone;
    
    PlaceBitsInSequence(zeroToSix,
      new Point(_items.Count - 1, 8),
      new Point(finderSizeWithCOnfiguration, 0),
      new Point(-1, 1)
      );
    PlaceBitsInSequence(
      sixToFourteenth,
      new Point(finderSizeWithCOnfiguration, finderSizeWithCOnfiguration),
      new Point(finderSizeWithCOnfiguration, _items.Count - finderSizeWithCOnfiguration),
      new Point(-1, 1)
    );

    return this;
  }

  private void PlaceBitsInSequence(string zeroToSix, Point verticalStart, Point horizontalStart, Point increasers, bool ignoreModuleSkip = false)
  {
    var verticalCopy = new Point(verticalStart);
    var horizontalCopy = new Point(horizontalStart);
    var bitsContainer = new BitsContainer(zeroToSix, false);
    
    while (bitsContainer.HasNext())
    {
      var xCoord = verticalCopy.X;
      var yCoord = horizontalCopy.Y;
      
      var nextVerticalModule = (QRModule)_items[xCoord][verticalCopy.Y];
      var nextHorizontalModule = (QRModule)_items[horizontalCopy.X][yCoord];

      if (IsBlackOrWhite(nextVerticalModule.Type) && !ignoreModuleSkip)
      {
        xCoord += increasers.X;
      }
      
      if (IsBlackOrWhite(nextHorizontalModule.Type) && !ignoreModuleSkip)
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
          var isXSafe = i >= topLeft.X && i <= bottomRight.X;
          var isYSafe = j >= topLeft.Y && j <= bottomRight.Y;
            
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
    if (module.Type == EModuleType.Red)
    {
      if (!bitsContainer.HasNext())
      {
        return false;
      }
      
      _items[pos.Y][pos.X] = GetNewModule(bitsContainer);
    }

    return true;
  }
  
  private QRModule GetNewModule(BitsContainer bitsContainer)
  {
    var currentBit = bitsContainer.GetCurrent();
    bitsContainer.TryIncrementCounter();
    
    return new QRModule((EModuleType)byte.Parse(currentBit));
  }
  
  private Point GetYModulePositionByDirection(int x, int y, EFillDirection direction)
  {
    var nextY = direction == EFillDirection.Upwards ? y - 1 : y + 1;
    var nextX = x;
    
    if (nextY == -1 || nextY == _cfg.Size.Width)
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

  private bool IsBlackOrWhite(EModuleType module) => module is EModuleType.Black or EModuleType.White;
}