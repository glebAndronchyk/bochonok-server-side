namespace bochonok_server_side.interfaces;

public interface IEntityService<TEntity>
{
    Task<List<TEntity>> GetAll();
    Task<TEntity> Add<TEntityDTO>(TEntityDTO entity);
    Task<TEntity> GetById(Guid id);
}