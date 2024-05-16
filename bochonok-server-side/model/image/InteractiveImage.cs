using SixLabors.ImageSharp.PixelFormats;

namespace bochonok_server_side.Model.Image;

public class InteractiveImage: ImageBase
{
    public InteractiveImage(byte[] byteArray) : base(byteArray)
    { }
    
    public InteractiveImage(string src) : base(src)
    { }

    public ImageBase ApplyMask(Rgba32[] mask)
    {
        var grayscaleImage = ToGrayScale();

        for (int i = 0; i < UPPER; i++)
        {
            
        }
    }
}