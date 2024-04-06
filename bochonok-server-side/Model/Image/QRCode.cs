namespace bochonok_server_side.Model.Image;

public class QRCode: ImageBase
{
    public QRCode(byte[] byteArray) : base(byteArray)
    {
        Encode();
    }

    public QRCode(string src) : base(src)
    {
        Encode();
    }

    public void Encode()
    {
    }

    public string Decode()
    {
        return "";
    }
}