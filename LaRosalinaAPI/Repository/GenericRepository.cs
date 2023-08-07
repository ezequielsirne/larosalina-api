using LaRosalinaAPI.Models;
using LaRosalinaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LaRosalinaAPI.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly LaRosalinaDbContext _context;
        private DbSet<TEntity> _dbSet;

        public GenericRepository(LaRosalinaDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        //El método Get() recibe una expresiòn lambda la cual se ignora si es nula y sino se le pasa al método OrderBy
        public ICollection<TEntity> Get(Expression<Func<TEntity, object>> orderBy = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (orderBy != null)
            {
                query = query.OrderBy(orderBy);
            }

            return query.ToList();
        }

        public TEntity Get(int id) => _dbSet.Find(id);
    }
}
