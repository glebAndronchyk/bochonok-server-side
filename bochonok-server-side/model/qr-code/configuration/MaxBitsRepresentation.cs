using bochonok_server_side.Model.Image.enums;

namespace bochonok_server_side.model.qr_code;

public class MaxBitsRepresentation
{
    private byte _firstPart;
    private byte _secondPart;
    private byte _thirdPart;
    
    public MaxBitsRepresentation(byte firstPart, byte secondPart, byte thirdPart)
    {
        _firstPart = firstPart;
        _secondPart = secondPart;
        _thirdPart = thirdPart;
    }

    public byte GetByVersion(EVersion version)
    {
        int versionNumber = (int)version + 1;
        
        return versionNumber switch
        {
            >= 1 and <= 9 => _firstPart,
            >= 10 and <= 26 => _secondPart,
            >= 27 and <= 40 => _thirdPart,
            _ => throw new ArgumentOutOfRangeException(nameof(version), "Version must be between 1 and 40.")
        };
    }
}