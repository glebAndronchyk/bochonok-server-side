namespace bochonok_server_side.Model.Image;

public class QRCode
{
    public ImageBase Image;
    
    private string _encodeString;
    
    
    public QRCode(string encodeString)
    {
        _encodeString = encodeString;
        Encode();
    }

    public ImageBase Encode()
    {
        return new ImageBase(new byte[10]);
    }

    public string Decode()
    {
        return "";
    }
}