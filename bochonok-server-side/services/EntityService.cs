using bochonok_server_side.database;
using bochonok_server_side.interfaces;
using Microsoft.EntityFrameworkCore;

namespace bochonok_server_side.services;

public class EntityService<TEntity>: IEntityService<TEntity> where TEntity: class
{
    private readonly DataContext _context;
    
    public EntityService(DataContext context)
    {
        _context = context;
    }

    public async Task<List<TEntity>> GetAll()
    {
        return await GetDBSet().ToListAsync();
    }

    public async Task<TEntity> Add<TEntityDTO>(TEntityDTO entity)
    {
        var castedEntity = entity as TEntity;
        
        GetDBSet().Add(castedEntity);
        await _context.SaveChangesAsync();

        return castedEntity;
    }

    public async Task<TEntity> GetById(Guid id)
    {
        TEntity? entity = await GetDBSet().FindAsync(id);

        if (entity != null)
        {
            return entity;
        }

        throw new KeyNotFoundException($"No entity with id:{id} found.");
    }

    private DbSet<TEntity> GetDBSet()
    {
        return _context.Set<TEntity>();
    }
}