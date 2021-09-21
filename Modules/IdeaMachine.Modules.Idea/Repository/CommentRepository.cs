using IdeaMachine.Common.Database.Context;
using IdeaMachine.Common.Database.Repository;
using IdeaMachine.Modules.Idea.DataTypes.Entity;
using IdeaMachine.Modules.Idea.Repository.Context;
using IdeaMachine.Modules.Idea.Repository.Interface;

namespace IdeaMachine.Modules.Idea.Repository
{
    public class CommentRepository : AbstractCrudRepository<IdeaContext, CommentEntity>, ICommentRepository
    {
	    public CommentRepository(DbContextFactory<IdeaContext> dbContextFactory) 
		    : base(dbContextFactory)
	    {
	    }
    }
}
