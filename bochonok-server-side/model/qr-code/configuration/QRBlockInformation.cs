using bochonok_server_side.Model.Image.enums;
using bochonok_server_side.model.qr_code.enums;

namespace bochonok_server_side.model.qr_code;

public class QRBlockInformation
{
    public EVersion version { get; set; }
    public EECLevel errorCorrection { get; set; }

    // For additional fields refer to https://www.thonky.com/qr-code-tutorial/error-correction-table
    public int totalCW { get; set; }
    public int ecPerBlock  { get; set; }
    public int numberOfBlocks { get; set; }
    public int dataCodewordsPerBlock { get; set; }
}