using DeliveryManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace DeliveryManagement.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext Db;
        protected readonly DbSet<T> Set;

        public Repository(ApplicationDbContext db)
        {
            Db = db;
            Set = db.Set<T>();
        }

        public async Task<T?> GetByIdAsync(int id)
            => await Set.FindAsync(id);

        public async Task<List<T>> GetAllAsync()
            => await Set.AsNoTracking().ToListAsync();

        public async Task AddAsync(T entity)
            => await Set.AddAsync(entity);

        public void Update(T entity) => Set.Update(entity);

        public void Remove(T entity) => Set.Remove(entity);

        public async Task SaveChangesAsync()
            => await Db.SaveChangesAsync();
    }
}