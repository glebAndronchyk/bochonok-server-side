namespace bochonok_server_side.Model.Image;

public class CatalogImage: ImageBase
{
    public CatalogImage(byte[] byteArray) : base(byteArray)
    { }
    
    public CatalogImage(string src) : base(src)
    { }
    
    public Shape GetShape()
    {
        return new Shape();
    }

    public ImageBase FillShapeWith()
    {
        return new ImageBase(new byte[]{});
    }
}