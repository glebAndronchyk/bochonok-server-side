using bochonok_server_side.model.encoding;
using bochonok_server_side.Model.Image.enums;
using Newtonsoft.Json.Linq;

namespace bochonok_server_side.Model.Image;

public class QRDataEncoder
{
  private static readonly string _bin236 = "11101100";
  private static readonly string _bin17 = "00010001";
  
  public static string EncodeCodewords(string str, EEncodingMode mode, EVersion version)
  {
    // TODO: unhardcode
    // this is hardcoded value - refer here to check every possible https://www.thonky.com/qr-code-tutorial/error-correction-table
    int maxCodewords = 34;
    
    if (str.Length > maxCodewords)
    {
      throw new ArgumentException("Input string is too long");
    }
    
    string result = "";
    string bitVersionPresenter = $"{(int)version}-{(Convert.ToString((int)mode, 2))}";
    byte maxBits = (byte)JObject.Parse(File.ReadAllText("data-bits.json"))
      .SelectToken($"$.data-bits.elements[?(@.name == '{bitVersionPresenter}')]")!
      ["bits"]!;
    
    AddModeIndicator(ref result, mode);
    AddCharacterIndicator(ref result, str, maxBits);
    EncodeEntryString(ref result, str);
    AddTerminatorBits(ref result, maxCodewords);
    PadToMultipleOfEight(ref result);
    AddPadBytes(ref result, maxCodewords);
    
    return result;
  }

  private static void EncodeEntryString(ref string data, string inputString)
  {
    data += String.Join("", StringEncoder.ByteModeEncode(inputString).ToArray());
  }

  private static void AddCharacterIndicator(ref string data, string str, byte maxBits)
  {
    string modeIndicator = Pad(Convert.ToString(str.Length, 2), maxBits);
    data += modeIndicator;
  }
  
  private static void AddModeIndicator(ref string data, EEncodingMode mode)
  {
    string modeIndicator = Pad(Convert.ToString((int)mode, 2), 4);
    data += modeIndicator;
  }
  
  private static string Pad(string binData, byte maxCapacity)
  {
    if (binData.Length == maxCapacity)
    {
      return binData;
    }

    return binData.PadLeft(maxCapacity, '0'); 
  }
  
  private static void PadToMultipleOfEight(ref string data)
  {
    int remainder = data.Length % 8;
    if (remainder != 0)
    {
      int paddingLength = 8 - remainder;
      data += new string('0', paddingLength);
    }
  }
  
  private static void AddTerminatorBits(ref string data, int requiredBits)
  {
    int dataLength = data.Length;
    int deficit = requiredBits * 8 - dataLength;
    if (deficit >= 4)
    {
      data += "0000";
    }
    else if (deficit > 0)
    {
      // Add only the number of 0s needed to reach the required number of bits
      data += new string('0', deficit);
    }
  }

  private static void AddPadBytes(ref string data, int maxCapacity)
  {
    int dataLength = data.Length;
    int deficit = (maxCapacity * 8 - dataLength) / 8;
    
    if (deficit > 0)
    {
      for (int i = 0; i < deficit; i++)
      {
        if (i % 2 == 0)
        {
          data += _bin236;
        }
        else
        {
          data += _bin17;
        }
      }
    }
  }
}