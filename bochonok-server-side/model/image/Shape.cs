namespace bochonok_server_side.Model.Image;

public class Shape
{
    public ImageBase Fill(ImageBase image)
    {
        return new ImageBase(image.GetByteArray());
    }
}