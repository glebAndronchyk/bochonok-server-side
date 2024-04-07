using System.Runtime.CompilerServices;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace bochonok_server_side.Model.Image;

public class ImageBase
{
    protected readonly byte[] ByteArray;
    protected readonly SixLabors.ImageSharp.Image<Rgba32> Img; 
    
    public ImageBase(string src)
    {
        ByteArray = ByteArrayFromSrc(src);
        Img = SixLabors.ImageSharp.Image.Load<Rgba32>(ByteArray);
    }

    public ImageBase(byte[] byteArray)
    {
        ByteArray = byteArray;
        Img = SixLabors.ImageSharp.Image.Load<Rgba32>(ByteArray);
    }
    
    public static byte[] ByteArrayFromSrc(string src)
    {
        SixLabors.ImageSharp.Image<Rgba32> img = SixLabors.ImageSharp.Image.Load<Rgba32>(src);
        return GetByteArrayFromImage(img);
    }

    public ImageBase ToGrayScale()
    {
        using var grayscaleImage = Img.Clone();
        grayscaleImage.Mutate(x => x.Grayscale());

        return new ImageBase(GetByteArrayFromImage(grayscaleImage));
    }

    public byte[] GetByteArray() => ByteArray;

    private static byte[] GetByteArrayFromImage(SixLabors.ImageSharp.Image<Rgba32> img)
    {
        var byteArray = new byte[img.Width * img.Height * Unsafe.SizeOf<Rgba32>()];
        img.CopyPixelDataTo(byteArray);

        return byteArray;
    }
}