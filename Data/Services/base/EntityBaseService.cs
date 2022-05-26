using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using someOnlineStore.Data.Services.ServiceInterfaces;

namespace someOnlineStore.Data.Services.ServicesImpl
{
    public class EntityBaseService<T> : IEntityBaseService<T> where T : class, IEntityBase, new()
    {
        private readonly ApplicationDbContext _context;

        public EntityBaseService(ApplicationDbContext context)
        {
            _context=context;
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            EntityEntry entry = _context.Entry(await _context.Set<T>().FirstOrDefaultAsync(temp => temp.Id.Equals(id)));
            entry.State= EntityState.Deleted;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(temp => temp.Id.Equals(id));
        }

        public async Task UpdateAsync(int id, T entity)
        {
            EntityEntry<T> entry = _context.Entry(entity);
            entry.State=EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
