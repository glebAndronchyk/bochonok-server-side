using bochonok_server_side.model;
using bochonok_server_side.model.utility_classes;

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
    _bytesMatrix = r.bytesMatrix;
  }
  
  public List<List<T>> GetAtomicItems()
  {
    return _items;
  }

  // Rework this
  public FlattenGroup Flatten()
  {
    int resultWidth = _items.Sum(row => row.Any() ? row.Max(item => item.Size.Width) : 0);
    int resultHeight = _items.Any() ? _items.Max(row => row.Sum(item => item.Size.Height)) : 0;

    var result = new FlattenGroup(resultWidth, resultHeight);

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
          itemBytes = group.Flatten().bytesMatrix;
        }
        else
        {
          itemBytes = item.GetMatrix();
        }

        for (int i = 0; i < item.Size.Width; i++)
        {
          for (int j = 0; j < item.Size.Height; j++)
          {
            result.atomicItems.Add(item);
            result.bytesMatrix.Set(currentX + i, currentY + j, itemBytes.At(i, j));
          }
        }

        currentY += item.Size.Height;
      }

      currentX += row.Any() ? row.Max(item => item.Size.Width) : 0;
    }

    return result;
  }
}