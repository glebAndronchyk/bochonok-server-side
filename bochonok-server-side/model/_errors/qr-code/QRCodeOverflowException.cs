namespace bochonok_server_side.model._errors;

public class QRCodeOverflowException : QRCodeError
{
    public QRCodeOverflowException(string description)
        :base("QRCodeOverflowException", description)
    { }
}