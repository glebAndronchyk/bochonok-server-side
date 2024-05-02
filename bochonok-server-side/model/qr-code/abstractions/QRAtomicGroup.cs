using bochonok_server_side.model;

namespace bochonok_server_side.Model.Image.abstractions;

public abstract class QRAtomicGroup<T> : QRAtomic where T: QRAtomic
{
  protected List<List<T>> _items;

  public QRAtomicGroup(List<List<T>> items, QRSize qrSize) : base(qrSize)
  {
    SetItems(items);
  }
  
  public QRAtomicGroup(QRSize qrSize) : base(qrSize)
  {
    _items = new List<List<T>>();
  }

  public void SetItems(List<List<T>> items)
  {
    _items = items;
    var r = Flatten();
    _bytesMatrix = r;
  }

  // Rework this
  public ByteMatrix Flatten()
  {
    int resultWidth = _items.Sum(row => row.Any() ? row.Max(item => item.Size.Width) : 0);
    int resultHeight = _items.Any() ? _items.Max(row => row.Sum(item => item.Size.Height)) : 0;

    var result = new ByteMatrix(resultWidth, resultHeight);

    int currentX = 0;
    int currentY;

    foreach (List<T> row in _items)
    {
      currentY = 0;

      foreach (T item in row)
      {
        ByteMatrix itemBytes;

        if (item is QRAtomicGroup<T> group)
        {
          itemBytes = group.Flatten();
        }
        else
        {
          itemBytes = item.GetMatrix();
        }

        for (int i = 0; i < item.Size.Width; i++)
        {
          for (int j = 0; j < item.Size.Height; j++)
          {
            result.Set(currentX + i, currentY + j, itemBytes.At(i, j));
          }
        }

        currentY += item.Size.Height;
      }

      currentX += row.Any() ? row.Max(item => item.Size.Width) : 0;
    }

    return result;
  }
}