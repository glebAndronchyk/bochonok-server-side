using bochonok_server_side.model.qr_code;

namespace bochonok_server_side.model.utility_classes.Mat;

public class Mat<T>
{
    private T[,] _matrix;

    public delegate T FactoryMethod();

    public Mat(int rows, int columns, T val)
    {
        _matrix = new T[rows, columns];
        Fill(val);
    }

    public Mat(int rows, int columns, FactoryMethod factoryMethod)
    {
        _matrix = new T[rows, columns];
        Fill(factoryMethod);
    }

    public Mat(T[,] matrix)
    {
        _matrix = (T[,])matrix.Clone();
    }

    public T[,] GetMatrix()
    {
        return _matrix;
    }

    public void AddItem(int row, int column, T value)
    {
        if (_matrix is T[,])
        {
            _matrix[row, column] = AddValues(_matrix[row, column], value);
        }
        else
        {
            throw new InvalidOperationException("Operation not supported for this type.");
        }
    }

    public void Set(int row, int column, T value)
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

    public ScalableSize GetSize()
    {
        return new ScalableSize(_matrix.GetLength(0), _matrix.GetLength(1));
    }

    public T At(int row, int column)
    {
        return _matrix[row, column];
    }

    public void Add(T value)
    {
        if (_matrix is T[,])
        {
            Iterate((i, j) =>
            {
                _matrix[i, j] = AddValues(_matrix[i, j], value);
            });
        }
        else
        {
            throw new InvalidOperationException("Operation not supported for this type.");
        }
    }

    private void Fill(T value)
    {
        Iterate((i, j) => _matrix[i, j] = value);
    }

    private void Fill(FactoryMethod factory)
    {
        Iterate((i, j) => _matrix[i, j] = factory());
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

    private static T AddValues(T a, T b)
    {
        dynamic x = a;
        dynamic y = b;
        return (T)(x + y);
    }
}