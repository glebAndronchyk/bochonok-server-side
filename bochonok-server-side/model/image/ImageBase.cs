using bochonok_server_side.model.encoding;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;

namespace bochonok_server_side.Model.Image;

public class ImageBase
{
    public readonly Image<Rgba32> Img;
    
    public ImageBase(string b64)
    {
        Img = SixLabors.ImageSharp.Image.Load<Rgba32>(
            Convert.FromBase64String(StringEncoder.GetCleanB64(b64))
            );
    }

    public ImageBase ToGrayScale()
    {
        Img.Mutate(x => x.Grayscale());
        
        return this;
    }
    
    public string ToBase64String() => Img.ToBase64String(PngFormat.Instance);
}