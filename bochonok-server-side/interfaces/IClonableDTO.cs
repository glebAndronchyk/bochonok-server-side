namespace bochonok_server_side.interfaces;

public interface IClonableDTO<ObjType, DtoType>
{
    public DtoType From(ObjType obj);
}