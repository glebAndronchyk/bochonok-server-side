namespace bochonok_server_side.model.utility_classes;

// TODO: add start/end methods
public class BitsContainer
{
  private string _bits;
  private int _bitCounter;
  private bool _autoIncrement;
  
  public BitsContainer(string bits, bool autoIncrement)
  {
    _bits = bits;
    _bitCounter = 0;
    _autoIncrement = autoIncrement;
  }
  
  public bool HasNext()
  {
    return _bitCounter < _bits.Length;
  }

  public int GetCurrentCounter()
  {
    return _bitCounter;
  }

  public string GetCurrent()
  {
    return _bits[_bitCounter].ToString();
  }
  
  public void TryIncrementCounter(bool ignoreAutoIncrement = false)
  {
    if (_autoIncrement || ignoreAutoIncrement)
    {
      _bitCounter++;
    }
  }
}