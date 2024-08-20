using Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface IOrderRepository
    {
        Task<GenericViewModel<OrderViewModel>> GetList(OrderViewSearchModel searchModel);
        Task<OrderDetailViewModel> GetOrderDetailByOrderId(long OrderId);
        Task<TotalCountSumOrder> GetTotalCountSumOrder(OrderViewSearchModel searchModel);
    }
}
