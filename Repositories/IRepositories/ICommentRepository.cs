using Entities.ViewModels.Comment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface ICommentRepository
    {
        Task<List<CommentViewModel>> GetAllComment(CommentParamRequest request);
    }
}
