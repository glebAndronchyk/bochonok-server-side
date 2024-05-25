namespace bochonok_server_side.model._errors;

public class QRCodeError : Exception
{
    public QRCodeError(string title, string description)
        :base($"QRCodeError: {title} - ${description}")
    { }
}