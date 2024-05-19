using bochonok_server_side.model.utility_classes.grouping_utilities;
using bochonok_server_side.model.utility_classes.Mat;

namespace bochonok_server_side.model.qr_code.abstractions;

public abstract class QRAtomicGroup<T> : QRAtomic where T: QRAtomic
{
  protected List<List<T>> _items;

  public QRAtomicGroup(ScalableSize scalableSize) : base(scalableSize)
  {
    _items = new List<List<T>>();
  }

  public void SetItems(List<List<T>> items)
  {
    _items = items;
    var flattenItems = Flatten();
    _bytesMatrix = flattenItems.bytesMatrix;
  }
  
  public List<List<T>> GetAtomicItems()
  {
    return _items;
  }

  public FlattenGroup Flatten()
  {
    int resultWidth = _items.Sum(row => row.Any() ? row.Max(item => item.Size.Width) : 0);
    int resultHeight = _items.Any() ? _items.Max(row => row.Sum(item => item.Size.Height)) : 0;

    var result = new FlattenGroup(resultWidth, resultHeight);

    int currentX = 0;

    foreach (List<T> row in _items)
    {
      var currentY = 0;

      foreach (T item in row)
      {
        Mat<byte> itemBytes;

        itemBytes = item is QRAtomicGroup<T> group ? group.Flatten().bytesMatrix : item.GetMatrix();

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