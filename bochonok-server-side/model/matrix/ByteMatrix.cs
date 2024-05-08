namespace bochonok_server_side.model
{
  public class ByteMatrix
  {
    private byte[,] _matrix;

    public ByteMatrix(int rows, int columns)
    {
      _matrix = new byte[rows, columns];
      Fill(0);
    }
    
    public ByteMatrix(byte[,] matrix)
    {
      _matrix = (byte[,])matrix.Clone();
    }
    
    public byte[,] GetBytes()
    {
      return _matrix;
    }

    public void AddItem(int row, int column, byte value)
    {
      _matrix[row, column] += value;
    }
    
    public void Set(int row, int column, byte value)
    {
      if (row >= _matrix.GetLength(0))
      {
        throw new IndexOutOfRangeException("Row index is out of range");
      } 
      
      if (column >= _matrix.GetLength(1))
      {
        throw new IndexOutOfRangeException("Column index is out of range");
      }

      _matrix[row, column] = value;
    }
    
    public QRSize GetSize()
    {
      return new QRSize(_matrix.GetLength(0), _matrix.GetLength(1));
    }
    
    public byte At(int row, int column)
    {
      return _matrix[row, column];
    }

    public void Add(byte value)
    {
      Iterate((i, j) => _matrix[i, j] += value);
    }

    private void Fill(byte value)
    {
      Iterate((i, j) => _matrix[i, j] = value);
    }
    
    private void Iterate(Action<int, int> action)
    {
      for (int i = 0; i < _matrix.GetLength(0); i++)
      {
        for (int j = 0; j < _matrix.GetLength(1); j++)
        {
          action(i, j);
        }
      }
    }
  }
}