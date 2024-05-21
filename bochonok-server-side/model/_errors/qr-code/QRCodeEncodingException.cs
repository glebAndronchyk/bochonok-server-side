namespace bochonok_server_side.model._errors;

public class QRCodeEncodingException : QRCodeError
{
    public QRCodeEncodingException(string description)
        :base("QRCodeEncodingException", description)
    {
    }
}