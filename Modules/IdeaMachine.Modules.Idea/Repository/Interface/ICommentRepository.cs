using System.Collections.Generic;
using System.Threading.Tasks;
using IdeaMachine.Common.Database.Repository.Interface;
using IdeaMachine.Modules.Idea.DataTypes.Entity;

namespace IdeaMachine.Modules.Idea.Repository.Interface
{
    public interface ICommentRepository : ICrudRepository<CommentEntity>
    {
	    Task<List<CommentEntity>> GetComments(int ideaId);
    }
}
