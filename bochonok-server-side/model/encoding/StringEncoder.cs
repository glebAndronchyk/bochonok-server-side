using System.Text;

namespace bochonok_server_side.model.encoding;

public class StringEncoder
{
  public static List<string> ByteModeEncode(string input)
  {
    List<string> encodedStrings = new List<string>();

    byte[] bytes = Encoding.UTF8.GetBytes(input);

    foreach (byte b in bytes)
    {
      string binaryString = Convert.ToString(b, 2).PadLeft(8, '0');
      encodedStrings.Add(binaryString);
    }

    return encodedStrings;
  }
}