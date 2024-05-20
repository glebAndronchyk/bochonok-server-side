using bochonok_server_side.model.encoding;
using bochonok_server_side.Model.Image.enums;
using bochonok_server_side.model.qr_code;
using bochonok_server_side.model.qr_code.QrCodeConfiguration;
using Newtonsoft.Json.Linq;
using ZXing.Common.ReedSolomon;

namespace bochonok_server_side.Model.Image;

public class QRDataEncoder
{
  private const string _bin236 = "11101100";
  private const string _bin17 = "00010001";
  
  public static string EncodeCodewords(string str, QRBlockInformation information, QREncodingMeta encoding)
  {
    if (str.Length > information.totalCW)
    {
      throw new ArgumentException("Input string is too long");
    }

    var maxCodewords = information.totalCW;
    byte maxBits = encoding.maxBits.GetByVersion(information.version);
    string result = "";
    
    AddModeIndicator(ref result, encoding.binaryRepresentation);
    AddCharacterIndicator(ref result, str, maxBits);
    EncodeEntryString(ref result, str);
    AddTerminatorBits(ref result, maxCodewords, maxBits);
    PadToMultipleOfEight(ref result, maxBits);
    AddPadBytes(ref result, maxCodewords, maxBits);
    AddErrorCorrectionBytes(ref result, maxBits, information.ecPerBlock);
    AddPadding(ref result, QRCodeConfiguration.ErrorCorrectionPadding[information.version]);
    
    return result;
  }

  private static void EncodeEntryString(ref string data, string inputString) => data += String.Join("", StringEncoder.ByteModeEncode(inputString).ToArray());
  private static void AddCharacterIndicator(ref string data, string str, byte maxBits)
  {
    string modeIndicator = Pad(Convert.ToString(str.Length, 2), maxBits);
    data += modeIndicator;
  }

  private static void AddModeIndicator(ref string data, string binaryRepresentation) => data += binaryRepresentation;
  
  private static string Pad(string binData, byte maxCapacity)
  {
    if (binData.Length == maxCapacity)
    {
      return binData;
    }

    return binData.PadLeft(maxCapacity, '0'); 
  }
  
  private static void PadToMultipleOfEight(ref string data, byte maxBits)
  {
    int remainder = data.Length % maxBits;
    if (remainder != 0)
    {
      int paddingLength = 8 - remainder;
      data += new string('0', paddingLength);
    }
  }
  
  private static void AddTerminatorBits(ref string data, int requiredBits, int maxBits)
  {
    int dataLength = data.Length;
    int deficit = requiredBits * maxBits - dataLength;
    const int terminatorLength = 4;
    
    if (deficit >= terminatorLength)
    {
      data += "0000";
    }
    else if (deficit > 0)
    {
      data += new string('0', deficit);
    }
  }

  private static void AddPadBytes(ref string data, int maxCapacity, int maxBits)
  {
    int dataLength = data.Length;
    int deficit = (maxCapacity * maxBits - dataLength) / maxBits;
    
    if (deficit > 0)
    {
      for (int i = 0; i < deficit; i++)
      {
        bool isEven = i % 2 == 0;
        
        data += isEven ? _bin236 : _bin17;
      }
    }
  }

  private static void AddPadding(ref string data, int paddingBits)
  {
    data += String.Join("", Enumerable.Range(0, paddingBits).Select(_ => "0").ToArray());
  }

  private static void AddErrorCorrectionBytes(ref string data, byte maxBits, int ecc)
  {
    // i'm too stupid to implement this by myself :)
    
    ReedSolomonEncoder encoder = new ReedSolomonEncoder(GenericGF.QR_CODE_FIELD_256);
    var intData = StringEncoder.SplitInParts(data, maxBits)
      .Select(v => Convert.ToInt32(v, 2))
      .Concat(
        Enumerable.Range(0, ecc)
          .Select(_ => 0))
      .ToArray();
    
    encoder.encode(intData, ecc);
    
    data = String.Join("", intData.Select(v => Pad(Convert.ToString(v, 2), maxBits)));
  }
}