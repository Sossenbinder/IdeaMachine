using System.Collections.Generic;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Utils.IPC;
using IdeaMachine.Modules.Idea.DataTypes.Model;

namespace IdeaMachine.Modules.Idea.Service.Interface
{
    public interface ICommentService
    {
	    Task<ServiceResponse<List<CommentModel>>> GetComments(int ideaId);
    }
}
