using DAL;
using Entities.ConfigModels;
using Entities.ViewModels.Comment;
using Microsoft.Extensions.Options;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly CommentDAL _commentDAL;
        public CommentRepository(IOptions<DataBaseConfig> dataBaseConfig)
        {
            _commentDAL = new CommentDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
        }
        public async Task<List<CommentViewModel>> GetAllComment(CommentParamRequest request)
        {
            return await _commentDAL.GetAllComment(request);
        }
    }
}
