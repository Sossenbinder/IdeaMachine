using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IdeaMachine.Common.Database.Repository.Interface
{
    public interface ICrudRepository<TEntity>
    {
	    Task<bool> Add(TEntity entity);

	    Task<List<TEntity>> Get(Expression<Func<TEntity, bool>> predicate);
    }
}
