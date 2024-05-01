using bochonok_server_side.model;
using MathNet.Numerics.LinearAlgebra;

namespace bochonok_server_side.Model.Image.abstractions;

public abstract class QRAtomicGroup<T> : QRAtomic where T: QRAtomic
{
  protected List<List<T>> _items;

  public QRAtomicGroup(List<List<T>> items, Tuple<int, int>? size) : base(size)
  {
    SetItems(items);
  }
  
  public QRAtomicGroup(Tuple<int, int>? size, int multiplier) : base(size, multiplier)
  {
    _items = new List<List<T>>();
  }

  public void OverrideItem(T item)
  { }

  public void SetItems(List<List<T>> items)
  {
    _items = items;
    var r = Flatten();
    _bytesMatrix = r;
  }

  // Rework this
  public ByteMatrix Flatten()
  {
    var result = new ByteMatrix(Size.Item1, Size.Item2);

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

        for (int i = 0; i < item.Size.Item1; i++)
        {
          for (int j = 0; j < item.Size.Item2; j++)
          {
            result.Set(currentX + i, currentY + j, itemBytes.At(i, j));
          }
        }

        // Update the current position
        currentY += item.Size.Item2;
      }

      // Update the current position
      currentX += row[0].Size.Item1;
      currentY = 0;
    }

    return result;
  }
}