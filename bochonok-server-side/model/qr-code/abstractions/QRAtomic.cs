using bochonok_server_side.model.utility_classes.Mat;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;

namespace bochonok_server_side.model.qr_code.abstractions;

public abstract class QRAtomic : ICloneable
{
  public ScalableSize Size
  {
    get => _size;
    set
    {
      _size = value;
      _bytesMatrix = new(value.Width, value.Height, 0);
    }
  }
  
  private ScalableSize _size; // Backing field for the property

  protected Mat<byte> _bytesMatrix;
  
  protected QRAtomic(ScalableSize? scalableSize)
  {
    Size = scalableSize ?? new(5, 5);
  }
  
  public Mat<byte> GetMatrix()
  {
    return _bytesMatrix;
  }

  public byte[,] GetBytes()
  {
    return _bytesMatrix.GetMatrix();
  }

  public string ToBase64String()
  {
    var rgba = GetRgba32Bytes();
    var size = (int)Math.Sqrt(rgba.Length);
    using (var image = Image.LoadPixelData<Rgba32>(rgba, size, size))
    {
      return image.ToBase64String(PngFormat.Instance);
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