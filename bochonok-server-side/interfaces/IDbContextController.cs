using bochonok_server_side.database;

namespace bochonok_server_side.interfaces;

public interface IDbContextController
{
    DataContext databaseContext { get; set; }
}