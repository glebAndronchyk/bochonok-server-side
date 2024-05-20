using bochonok_server_side.model.qr_code.enums;
using Point = bochonok_server_side.model.utility_classes.Point;

namespace bochonok_server_side.model.qr_code.QrCodeConfiguration;

public partial class QRCodeConfiguration
{
    private void PrepareBlockInfo()
    {
        var blockInfo = _blocksInfo.Find(block => block.version == Version && block.errorCorrection == ErrorCorrection);

        BlockInformation = blockInfo ?? throw new NotImplementedException("No such version/level configuration.");
    }

    private void SetTimingLength()
    {
        TimingLength = Size.Width - 2 * FinderSizeWithSafeZone;
    }

    private void PreparePatternsPosition()
    {
        var v = (int)Version;
        var finderPos = Size.Width - FinderModuleSize;
        var alignmentPos = Size.Width - AlignmentOffset;

        PatternsPosition = new()
        {
            { EPlacement.TopLeft, new Point(0, 0) },
            { EPlacement.BottomLeft, new Point(finderPos, 0) },
            { EPlacement.TopRight, new Point(0, finderPos) },
            { EPlacement.BottomRight,  new Point(alignmentPos, alignmentPos)},
            // Refer to guide for explanation https://www.thonky.com/qr-code-tutorial/module-placement-matrix#dark-module
            { EPlacement.BottomLeftOffset, new Point(4 * (v + 1) + 9, 8)}
        };
    }
}