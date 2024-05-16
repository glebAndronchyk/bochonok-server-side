using System.Runtime.CompilerServices;
using SixLabors.ImageSharp.PixelFormats;

namespace bochonok_server_side.Model.Image;

public class ImageBase
{
    public readonly Image<Rgba32> Img;
    protected readonly byte[] ByteArray;
    
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
        Image<Rgba32> img = SixLabors.ImageSharp.Image.Load<Rgba32>(src);
        return GetByteArrayFromImage(img);
    }

    public ImageBase ToGrayScale()
    {
        using var grayscaleImage = Img.Clone();
        grayscaleImage.Mutate(x => x.Grayscale());

        return new ImageBase(GetByteArrayFromImage(grayscaleImage));
    }

    public byte[] GetByteArray() => ByteArray;

    public string GetB64() => Convert.ToBase64String(ByteArray);

    private static byte[] GetByteArrayFromImage(Image<Rgba32> img)
    {
        var byteArray = new byte[img.Width * img.Height * Unsafe.SizeOf<Rgba32>()];
        img.CopyPixelDataTo(byteArray);

        return byteArray;
    }
}