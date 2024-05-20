using bochonok_server_side.model.qr_code.abstractions;
using bochonok_server_side.model.utility_classes.Mat;

namespace bochonok_server_side.model.utility_classes.grouping_utilities;

public class FlattenGroup
{
  public Mat<byte> bytesMatrix { get; set; }
  public List<QRAtomic> atomicItems { get; set; }

  public FlattenGroup(int rows, int columns)
  {
    bytesMatrix = new Mat<byte>(rows, columns, 0);
    atomicItems = new List<QRAtomic>();
  }
}