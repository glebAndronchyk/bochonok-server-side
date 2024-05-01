
using bochonok_server_side.model;
using MathNet.Numerics.LinearAlgebra;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.PixelFormats;

namespace bochonok_server_side.Model.Image.abstractions;

public abstract class QRAtomic
{
  public Tuple<int, int> Size;
  
  protected ByteMatrix _bytesMatrix;
  
  protected QRAtomic(Tuple<int, int>? size, int multiplier = 1)
  {
    if (size == null)
    {
      Size = new(5, 5);
    }
    else
    {
      Size = new Tuple<int, int>(size.Item1 * multiplier, size.Item2 * multiplier);
    }
    
    _bytesMatrix = new ByteMatrix(Size.Item1, Size.Item2);
  }
  
  public ByteMatrix GetMatrix()
  {
    return _bytesMatrix;
  }

  public byte[,] GetBytes()
  {
    return _bytesMatrix.GetBytes();
  }

  public Rgba32[] GetRgba32Bytes()
  {
    var bytes = GetBytes();
    List<Rgba32> rgba32Bytes = new ();

    for (int i = 0; i < bytes.GetLength(0); i++)
    {
      for (int j = 0; j < bytes.GetLength(1); j++)
      {
        Rgba32 color = bytes[i,j] == 1 ? Rgba32.ParseHex("#000000") : Rgba32.ParseHex("#ffffff");
        rgba32Bytes.Add(color);
      }
    }

    return rgba32Bytes.ToArray();
  }
}