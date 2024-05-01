
using bochonok_server_side.model;
using SixLabors.ImageSharp.PixelFormats;

namespace bochonok_server_side.Model.Image.abstractions;

public abstract class QRAtomic
{
  public QRSize Size;
  
  protected ByteMatrix _bytesMatrix;
  
  protected QRAtomic(QRSize? qrSize)
  {
    if (qrSize == null)
    {
      Size = new(5, 5);
    }
    
    _bytesMatrix = new ByteMatrix(Size!.Width, Size.Height);
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