namespace bochonok_server_side.model._errors;

public class QRCodeInvalidConfigurationException : QRCodeError
{
    public QRCodeInvalidConfigurationException(string description)
        :base("QRCodeInvalidConfigurationException", description)
    { }
}