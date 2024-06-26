using System.Text;
using System.Text.RegularExpressions;

namespace bochonok_server_side.model.encoding;

public class StringEncoder
{
  public static List<string> ByteModeEncode(string input)
  {
    List<string> encodedStrings = new List<string>();

    byte[] bytes = Encoding.GetEncoding("UTF-8").GetBytes(input);

    foreach (byte b in bytes)
    {
      string binaryString = Convert.ToString(b, 2).PadLeft(8, '0');
      encodedStrings.Add(binaryString);
    }

    return encodedStrings;
  }

  public static string GetCleanB64(string initial)
  {
    string pattern = @"data:[\w\/\+]+;base64,(.*)";
        
    var match = Regex.Match(initial, pattern);

    if (match.Success)
    {
      return match.Groups[1].Value;
    }
  
    return initial;
  }

  public static string GenerateRandom(int length)
  {
    StringBuilder sb = new StringBuilder();
    int numGuidsToConcat = (length - 1) / 32 + 1;
    for(int i = 1; i <= numGuidsToConcat; i++)
    {
      sb.Append(Guid.NewGuid().ToString("N"));
    }

    return sb.ToString(0, length);
  }

  public static IEnumerable<String> SplitInParts(string s, int partLength) {
    if (s == null)
      throw new ArgumentNullException(nameof(s));
    if (partLength <= 0)
      throw new ArgumentException("Part length has to be positive.", nameof(partLength));

    for (var i = 0; i < s.Length; i += partLength)
      yield return s.Substring(i, Math.Min(partLength, s.Length - i));
  }

  
  public static string AddMIMEType(string base64Data, string mime)
  {
    // Combine data type and base64 data into a single string
    return $"data:{mime};base64,{base64Data}";
  }
}