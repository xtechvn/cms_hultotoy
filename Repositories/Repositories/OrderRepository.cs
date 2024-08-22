using DAL;
using DAL.OrderDetail;
using DAL.StoreProcedure;
using Entities.ConfigModels;
using Entities.Models;
using Entities.ViewModels;
using Entities.ViewModels.OrderDetail;
using Microsoft.Extensions.Options;
using Nest;
using PdfSharp;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Repositories.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDAL _OrderDal;
        private readonly ClientDAL _clientDAL;
        private readonly AllCodeDAL allCodeDAL;
        private readonly UserDAL userDAL;
        private readonly OrderDetailDAL _orderDetailDAL;




        public OrderRepository(IOptions<DataBaseConfig> dataBaseConfig)
        {
            _OrderDal = new OrderDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
            allCodeDAL = new AllCodeDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
            userDAL = new UserDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
            _clientDAL = new ClientDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
            _orderDetailDAL = new OrderDetailDAL(dataBaseConfig.Value.SqlServer.ConnectionString);


        }
        public async Task<GenericViewModel<OrderViewModel>> GetList(OrderViewSearchModel searchModel)
        {
            var model = new GenericViewModel<OrderViewModel>();

            try
            {
                DataTable dt = await _OrderDal.GetPagingList(searchModel, ProcedureConstants.GETALLORDER_SEARCH);
                if (dt != null && dt.Rows.Count > 0)
                {
                    var data = dt.ToList<OrderViewModel>();
                    model.ListData = data;
                    model.CurrentPage = searchModel.PageIndex;
                    model.PageSize = searchModel.pageSize;
                    model.TotalRecord = Convert.ToInt32(dt.Rows[0]["TotalRow"]);
                    model.TotalPage = (int)Math.Ceiling((double)model.TotalRecord / model.PageSize);
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetList - OrderRepository: " + ex);
            }
            return model;
        }
        public async Task<OrderDetailViewModel> GetOrderDetailByOrderId(long OrderId)
        {
            try
            {
                return await _OrderDal.GetDetailOrderByOrderId(OrderId);
                
               
            }
            catch(Exception ex)
            {
                LogHelper.InsertLogTelegram("GetOrderDetailByOrderId - OrderRepository: " + ex);
            }
            return null;
        }
        public async Task<TotalCountSumOrder> GetTotalCountSumOrder(OrderViewSearchModel searchModel)
        {
            var model = new TotalCountSumOrder();
            try
            {
                searchModel.PageIndex = -1;
                DataTable dt = await _OrderDal.GetPagingList(searchModel, ProcedureConstants.GET_TOTALCOUNT_ORDER);
                if (dt != null && dt.Rows.Count > 0)
                {


                    model.Profit = dt.Rows[0]["Profit"].Equals(DBNull.Value) ? 0 : Convert.ToDouble(dt.Rows[0]["Profit"]);
                    model.Amount = dt.Rows[0]["Amount"].Equals(DBNull.Value) ? 0 : Convert.ToDouble(dt.Rows[0]["Amount"]);
                    model.Price = dt.Rows[0]["Price"].Equals(DBNull.Value) ? 0 : Convert.ToDouble(dt.Rows[0]["Price"]);
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetTotalCountSumOrder in OrderRepository: " + ex);
            }
            return model;
        }
        public async Task<long> UpdateOrder(Order model)
        {
            try
            {
                return await _OrderDal.UpdateOrder(model);
            }
            catch(Exception ex)
            {
                LogHelper.InsertLogTelegram("GetTotalCountSumOrder in OrderRepository: " + ex);
            }
            return -1;
        }
        public async Task<List<ListOrderDetailViewModel>> GetListOrderDetail(long orderid)
        {
            try
            {
                return await _orderDetailDAL.GetListOrderDetail(orderid);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetTotalCountSumOrder in OrderRepository: " + ex);
            }
            return null;
        }
    }
}
