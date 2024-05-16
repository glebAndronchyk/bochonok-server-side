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
        
        for (int i = 0; i < Img.Width; i++)
        {
            for (int j = 0; j < Img.Height; j++)
            {
                var pixel = grayscaleImage.Img[i, j];
                
                if (pixel.R < 128)
                {
                    grayscaleImage.Img[i, j] = mask[(i * Img.Width + j) % mask.Length];
                }
            }
        }

        return new ImageBase(grayscaleImage.ToBase64String());
    }
}