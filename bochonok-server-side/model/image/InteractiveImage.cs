using bochonok_server_side.model.utility_classes;
using SixLabors.ImageSharp.PixelFormats;

namespace bochonok_server_side.Model.Image;

public class InteractiveImage: ImageBase
{
    public InteractiveImage(string b64) : base(b64)
    { }

    public ImageBase ApplyMask(Rgba32[] mask)
    {
        var grayscaleImage = ToGrayScale();
        var size = (int)Math.Sqrt(mask.Length);
        var mask2d = ArrayHelper.Make2DArray(mask, size, size);
        
        for (int i = 0; i < Img.Width; i++)
        {
            for (int j = 0; j < Img.Height; j++)
            {
                var pixel = grayscaleImage.Img[i, j];
        
                if (pixel is { R: < 250, A: > 0 })
                {
                    grayscaleImage.Img[i, j] = mask2d[i % size, j % size];
                }
            }
        }
        
        return this;
    }
}