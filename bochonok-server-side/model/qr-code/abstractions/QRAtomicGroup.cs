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
    var result = new ByteMatrix(Size.Width, Size.Height);

    int currentX = 0;
    int currentY = 0;

    foreach (List<T> row in _items)
    {
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

        // Update the current position
        currentY += item.Size.Width;
      }

      // Update the current position
      currentX += row[0].Size.Height;
      currentY = 0;
    }

    return result;
  }
}