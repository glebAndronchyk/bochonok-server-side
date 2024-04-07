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
        return await _context.Set<TEntity>().ToListAsync();
    }

    public async Task<TEntity> Add(TEntity entity)
    {
        _context.Set<TEntity>().Add(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    public async Task<TEntity> GetById(Guid id)
    {
        TEntity? entity = await _context.Set<TEntity>().FindAsync(id);

        if (entity != null)
        {
            return entity;
        }

        throw new KeyNotFoundException($"No entity with id:{id} found.");
    }
}