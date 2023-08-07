using LaRosalinaAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LaRosalinaAPI.Repository.IRepository
{
    public interface IGenericRepository<TEntity>
    {
        ICollection<TEntity> Get(Expression<Func<TEntity, object>> orderBy = null);

        TEntity Get(int id);

    }
}
