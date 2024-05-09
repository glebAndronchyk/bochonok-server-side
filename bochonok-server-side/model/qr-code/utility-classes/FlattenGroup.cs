using bochonok_server_side.Model.Image.abstractions;

namespace bochonok_server_side.model.utility_classes;

public class FlattenGroup
{
  public ByteMatrix bytesMatrix { get; set; }
  public List<QRAtomic> atomicItems { get; set; }

  public FlattenGroup(int rows, int columns)
  {
    bytesMatrix = new ByteMatrix(rows, columns);
    atomicItems = new List<QRAtomic>();
  }
}