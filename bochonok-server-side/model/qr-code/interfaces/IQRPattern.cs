using bochonok_server_side.model;
using bochonok_server_side.Model.Image.abstractions;

namespace bochonok_server_side.Model.Image.interfaces;

public interface IQRPattern
{
  public IQRPattern Build();
  public List<List<QRAtomic>> BuildLayout();
  public QRSize GetSize();
  public List<List<QRAtomic>> GetAtomicItems();
}