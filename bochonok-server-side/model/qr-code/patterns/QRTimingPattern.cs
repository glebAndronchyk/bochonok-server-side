using bochonok_server_side.model;
using bochonok_server_side.Model.Image.abstractions;
using bochonok_server_side.Model.Image.interfaces;

namespace bochonok_server_side.Model.Image;

public class QRTimingPattern : QRAtomicGroup<QRAtomic>, IQRPattern
{
  public QRTimingPattern(int modulesAmount) : base(new QRSize(modulesAmount, 1, 5))
  { }

  // TODO: think about this
  public IQRPattern Build()
  {
    var layout = BuildLayout();
    SetItems(layout);

    return this;
  }
  
  public QRSize GetSize()
  {
    return Size;
  }
  
  // TODO: optimize list usage
  public List<List<QRAtomic>> BuildLayout()
  {
    var layout = new List<List<QRAtomic>>();
    var modulesSize = Size.GetDividedSize();
    
    for (int i = 0; i < modulesSize.Width; i++)
    {
      var row = new List<QRAtomic>();
      for (int j = 0; j < modulesSize.Height; j++)
      {
        row.Add(i % 2 == 0 ? new QRModule(1) : new QRModule(0));
      }
      layout.Add(row);
    }
    
    return layout;
  }
}