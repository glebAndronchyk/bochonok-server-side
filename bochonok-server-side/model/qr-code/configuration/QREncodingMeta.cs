namespace bochonok_server_side.model.qr_code;

public class QREncodingMeta
{
    public delegate List<string> EncodingMethodType(string data);
    
    public string binaryRepresentation { get; set; }
    public MaxBitsRepresentation maxBits { get; set; }
    public EncodingMethodType encodingMethod { get; set; }
}