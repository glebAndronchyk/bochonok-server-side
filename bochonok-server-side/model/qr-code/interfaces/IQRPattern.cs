using bochonok_server_side.model;
using bochonok_server_side.model.qr_code;
using bochonok_server_side.model.qr_code.abstractions;

namespace bochonok_server_side.Model.Image.interfaces;

public interface IQRPattern
{
  public IQRPattern Build();
  public List<List<QRAtomic>> BuildLayout();
  public ScalableSize GetSize();
  public List<List<QRAtomic>> GetAtomicItems();
}