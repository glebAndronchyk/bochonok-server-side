
using bochonok_server_side.model;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;

namespace bochonok_server_side.Model.Image.abstractions;

public abstract class QRAtomic : ICloneable
{
  public QRSize Size;
  
  protected ByteMatrix _bytesMatrix;
  
  protected QRAtomic(QRSize? qrSize)
  {
    if (qrSize == null)
    {
      Size = new(5, 5);
    }
    else
    {
      Size = qrSize;
    }
    
    _bytesMatrix = new ByteMatrix(Size.Width, Size.Height);
  }
  
  public ByteMatrix GetMatrix()
  {
    return _bytesMatrix;
  }

  public byte[,] GetBytes()
  {
    return _bytesMatrix.GetBytes();
  }

  public string ToBase64String()
  {
    var rgba = GetRgba32Bytes();
    var size = (int)Math.Sqrt(rgba.Length);
    using (var image = SixLabors.ImageSharp.Image.LoadPixelData<Rgba32>(rgba, size, size))
    {
      return image.ToBase64String(PngFormat.Instance);
    }
  }

  public byte[] GetFlattenBytes()
  {
    var rgba = GetRgba32Bytes();
    var size = (int)Math.Sqrt(rgba.Length);
    using (var image = SixLabors.ImageSharp.Image.LoadPixelData<Rgba32>(rgba, size, size))
    {
      var bytes = new byte[image.Height * image.Width * image.PixelType.BitsPerPixel / 8];
      image.CopyPixelDataTo(bytes);
      
      return bytes;
    }
  }

  public Rgba32[] GetRgba32Bytes()
  {
    var bytes = GetBytes();
    List<Rgba32> rgba32Bytes = new ();

    for (int i = 0; i < bytes.GetLength(0); i++)
    {
      for (int j = 0; j < bytes.GetLength(1); j++)
      {
        Rgba32 color;

        switch (bytes[i, j])
        {
          case 0:
            color = Rgba32.ParseHex("#ffffff");
            break;
          case 1:
            color = Rgba32.ParseHex("#000000");
            break;
          default:
            color = Rgba32.ParseHex("#f11f1f");
            break;
        }
        
        rgba32Bytes.Add(color);
      }
    }

    return rgba32Bytes.ToArray();
  }

  public object Clone()
  {
    return MemberwiseClone();
  }
}