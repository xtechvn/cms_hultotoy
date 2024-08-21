﻿using DAL.Generic;
using DAL.StoreProcedure;
using Entities.Models;
using Entities.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using Utilities.Contants;

namespace DAL
{
    public class OrderDAL : GenericService<Order>
    {
        private static DbWorker _DbWorker;
        public OrderDAL(string connection) : base(connection)
        {
            _DbWorker = new DbWorker(connection);
        }
        public async Task<DataTable> GetPagingList(OrderViewSearchModel searchModel, string proc)
        {
            try
            {

                SqlParameter[] objParam = new SqlParameter[24];


                objParam[0] = (CheckDate(searchModel.CreateTime) == DateTime.MinValue) ? new SqlParameter("@CreateTime", DBNull.Value) : new SqlParameter("@CreateTime", CheckDate(searchModel.CreateTime));
                objParam[1] = (CheckDate(searchModel.ToDateTime) == DateTime.MinValue) ? new SqlParameter("@ToDateTime", DBNull.Value) : new SqlParameter("@ToDateTime", CheckDate(searchModel.ToDateTime).AddDays(1));
                objParam[2] = new SqlParameter("@SysTemType", searchModel.SysTemType);
                objParam[3] = new SqlParameter("@OrderNo", searchModel.OrderNo);
                objParam[4] = new SqlParameter("@Note", searchModel.Note);
                objParam[5] = new SqlParameter("@ServiceType", searchModel.ServiceType == null ? "" : string.Join(",", searchModel.ServiceType));
                objParam[6] = new SqlParameter("@UtmSource", searchModel.UtmSource == null ? "" : searchModel.UtmSource);
                objParam[7] = new SqlParameter("@status", searchModel.Status == null ? "" : string.Join(",", searchModel.Status));
                objParam[8] = new SqlParameter("@CreateName", searchModel.CreateName);
                if (searchModel.Sale == null)
                {
                    objParam[9] = new SqlParameter("@Sale", DBNull.Value);

                }
                else
                {
                    objParam[9] = new SqlParameter("@Sale", searchModel.Sale);

                }
                objParam[10] = new SqlParameter("@SaleGroup", searchModel.SaleGroup);
                objParam[11] = new SqlParameter("@PageIndex", searchModel.PageIndex);
                objParam[12] = new SqlParameter("@PageSize", searchModel.pageSize);
                objParam[13] = new SqlParameter("@StatusTab", searchModel.StatusTab);
                objParam[14] = new SqlParameter("@ClientId", searchModel.ClientId);
                objParam[15] = new SqlParameter("@SalerPermission", searchModel.SalerPermission);
                objParam[16] = new SqlParameter("@OperatorId", searchModel.OperatorId);
                if (searchModel.StartDateFrom == null)
                {
                    objParam[17] = new SqlParameter("@StartDateFrom", DBNull.Value);

                }
                else
                {
                    objParam[17] = new SqlParameter("@StartDateFrom", searchModel.StartDateFrom);

                }
                if (searchModel.StartDateTo == null)
                {
                    objParam[18] = new SqlParameter("@StartDateTo", DBNull.Value);

                }
                else
                {
                    objParam[18] = new SqlParameter("@StartDateTo", searchModel.StartDateTo);

                }
                if (searchModel.EndDateFrom == null)
                {
                    objParam[19] = new SqlParameter("@EndDateFrom", DBNull.Value);

                }
                else
                {
                    objParam[19] = new SqlParameter("@EndDateFrom", searchModel.EndDateFrom);

                }
                if (searchModel.EndDateTo == null)
                {
                    objParam[20] = new SqlParameter("@EndDateTo", DBNull.Value);

                }
                else
                {
                    objParam[20] = new SqlParameter("@EndDateTo", searchModel.EndDateTo);

                }
                if (searchModel.PermisionType == null)
                {
                    objParam[21] = new SqlParameter("@PermisionType", DBNull.Value);

                }
                else
                {
                    objParam[21] = new SqlParameter("@PermisionType", searchModel.PermisionType);

                }
                if (searchModel.PaymentStatus == null)
                {
                    objParam[22] = new SqlParameter("@PaymentStatus", DBNull.Value);

                }
                else
                {
                    objParam[22] = new SqlParameter("@PaymentStatus", searchModel.PaymentStatus);

                }

                objParam[23] = new SqlParameter("@OrderId", searchModel.BoongKingCode);


                return _DbWorker.GetDataTable(proc, objParam);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetPagingList - OrderDal: " + ex);
            }
            return null;
        }
        private DateTime CheckDate(string dateTime)
        {
            DateTime _date = DateTime.MinValue;
            if (!string.IsNullOrEmpty(dateTime))
            {
                _date = DateTime.ParseExact(dateTime, "d/M/yyyy", CultureInfo.InvariantCulture);
            }

            return _date != DateTime.MinValue ? _date : DateTime.MinValue;
        }
        public async Task<OrderDetailViewModel> GetDetailOrderByOrderId(long OrderId)
        {
            try
            {

                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@OrderId", OrderId);

                DataTable dt = _DbWorker.GetDataTable(ProcedureConstants.SP_GetDetailOrderByOrderId, objParam);
                if (dt != null && dt.Rows.Count > 0)
                {
                    var data = dt.ToList<OrderDetailViewModel>();
                    return data[0];
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetDetailOrderByOrderId - OrderDal: " + ex);
            }
            return null;
        }
        public DataTable GetListOrderByClientId(long clienId, string proc, int status = 0)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[3];
                objParam[0] = new SqlParameter("@ClientId", clienId);
                objParam[1] = new SqlParameter("@IsFinishPayment", DBNull.Value);
                if (status == 0)
                    objParam[2] = new SqlParameter("@OrderStatus", DBNull.Value);
                else
                    objParam[2] = new SqlParameter("@OrderStatus", status);

                return _DbWorker.GetDataTable(proc, objParam);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetListOrderByClientId - OrderDal: " + ex);
            }
            return null;
        }
        public List<Order> GetByOrderIds(List<long> orderIds)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {

                    return _DbContext.Orders.AsNoTracking().Where(s => orderIds.Contains(s.OrderId)).ToList();
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetByOrderIds - OrderDal: " + ex);
                return new List<Order>();
            }
        }
        public async Task<int> UpdateOrderStatus(long OrderId, long Status, long UpdatedBy, long UserVerify)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[4];
                objParam[0] = new SqlParameter("@OrderId", OrderId);
                objParam[1] = new SqlParameter("@Status", Status);
                objParam[2] = new SqlParameter("@UpdatedBy", UpdatedBy);
                objParam[3] = UserVerify == 0 ? new SqlParameter("@UserVerify", DBNull.Value) : new SqlParameter("@UserVerify", UserVerify);

                return _DbWorker.ExecuteNonQuery(StoreProcedureConstant.SP_UpdateOrderStatus, objParam);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetDetailOrderServiceByOrderId - OrderDal: " + ex);
            }
            return 0;
        }
        public async Task<long> UpdateOrder(Order model)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[47];
                objParam[0] = new SqlParameter("@OrderId", model.OrderId);
                objParam[1] = model.OrderNo == null ? new SqlParameter("@OrderNo", DBNull.Value) : new SqlParameter("@OrderNo", model.OrderNo);
                objParam[2] = model.ServiceType == null ? new SqlParameter("@ServiceType", DBNull.Value) : new SqlParameter("@ServiceType", model.ServiceType);             
                objParam[3] =  new SqlParameter("@CreateTime", DBNull.Value);             
                objParam[4] = model.Amount == null ? new SqlParameter("@Amount", DBNull.Value) : new SqlParameter("@Amount", model.Amount);             
                objParam[5] = model.PaymentStatus == null ? new SqlParameter("@PaymentStatus", DBNull.Value) : new SqlParameter("@PaymentStatus", model.PaymentStatus);             
                objParam[6] = model.ClientId == null ? new SqlParameter("@ClientId", DBNull.Value) : new SqlParameter("@ClientId", model.ClientId);             
                objParam[7] = model.ContactClientId == null ? new SqlParameter("@ContactClientId", DBNull.Value) : new SqlParameter("@ContactClientId", model.ContactClientId);             
                objParam[8] = model.OrderStatus == null ? new SqlParameter("@OrderStatus", DBNull.Value) : new SqlParameter("@OrderStatus", model.OrderStatus);             
                objParam[9] =  new SqlParameter("@SmsContent", model.SmsContent);             
                objParam[10] = model.PaymentType == null ? new SqlParameter("@PaymentType", DBNull.Value) : new SqlParameter("@PaymentType", model.PaymentType);             
                objParam[11] = new SqlParameter("@BankCode", model.BankCode);             
                objParam[12] = new SqlParameter("@PaymentDate", DBNull.Value);             
                objParam[13] = model.PaymentNo == null ? new SqlParameter("@PaymentNo", DBNull.Value) : new SqlParameter("@PaymentNo", model.PaymentNo);             
                objParam[14] = new SqlParameter("@ColorCode", DBNull.Value);             
                objParam[15] = model.Discount == null ? new SqlParameter("@Discount", DBNull.Value) : new SqlParameter("@Discount", model.Discount);             
                objParam[16] = model.Profit == null ? new SqlParameter("@Profit", DBNull.Value) : new SqlParameter("@Profit", model.Profit);             
                objParam[17] = new SqlParameter("@ExpriryDate", model.ExpriryDate);             
                objParam[18] = new SqlParameter("@StartDate", model.StartDate);             
                objParam[19] = new SqlParameter("@EndDate", model.EndDate);             
                objParam[20] = model.ProductService == null ? new SqlParameter("@ProductService", DBNull.Value) : new SqlParameter("@ProductService", model.ProductService);             
                objParam[21] = model.Note == null ? new SqlParameter("@Note", DBNull.Value) : new SqlParameter("@Note", model.Note);             
                objParam[22] = model.UtmSource == null ? new SqlParameter("@UtmSource", DBNull.Value) : new SqlParameter("@UtmSource", model.UtmSource);             
                objParam[23] = new SqlParameter("@UpdateLast", model.UpdateLast);             
                objParam[24] = model.SalerId==0? new SqlParameter("@SalerId", DBNull.Value) : new SqlParameter("@SalerId", model.SalerId);             
                objParam[25] = model.SalerGroupId == null ? new SqlParameter("@SalerGroupId", DBNull.Value) : new SqlParameter("@SalerGroupId", model.SalerGroupId);             
                objParam[26] = model.UserUpdateId == null ? new SqlParameter("@UserUpdateId", DBNull.Value) : new SqlParameter("@UserUpdateId", model.UserUpdateId);             
                objParam[27] = model.SystemType == null ? new SqlParameter("@SystemType", DBNull.Value) : new SqlParameter("@SystemType", model.SystemType);             
                objParam[28] = model.AccountClientId == null ? new SqlParameter("@AccountClientId", DBNull.Value) : new SqlParameter("@AccountClientId", model.AccountClientId);             
                objParam[29] = model.Description == null ? new SqlParameter("@Description", DBNull.Value) : new SqlParameter("@Description", model.Description);             
                objParam[30] = model.BranchCode == null ? new SqlParameter("@BranchCode", DBNull.Value) : new SqlParameter("@BranchCode", model.BranchCode);             
                objParam[31] = model.BookingInfo == null ? new SqlParameter("@BookingInfo", DBNull.Value) : new SqlParameter("@BookingInfo", model.BookingInfo);             
                objParam[32] = model.Label == null ? new SqlParameter("@Label", DBNull.Value) : new SqlParameter("@Label", model.Label);             
                objParam[33] = model.IsFinishPayment == null ? new SqlParameter("@IsFinishPayment", DBNull.Value) : new SqlParameter("@IsFinishPayment", model.IsFinishPayment);             
                objParam[34] = model.PercentDecrease == null ? new SqlParameter("@PercentDecrease", DBNull.Value) : new SqlParameter("@PercentDecrease", model.PercentDecrease);             
                objParam[35] = model.VoucherId == null ? new SqlParameter("@VoucherId", DBNull.Value) : new SqlParameter("@VoucherId", model.VoucherId);             
                objParam[36] = model.Price == null ? new SqlParameter("@Price", DBNull.Value) : new SqlParameter("@Price", model.Price);             
                objParam[37] = model.SupplierId == null ? new SqlParameter("@SupplierId", DBNull.Value) : new SqlParameter("@SupplierId", model.SupplierId);             
                objParam[38] = model.DepartmentId == null ? new SqlParameter("@DepartmentId", DBNull.Value) : new SqlParameter("@DepartmentId", model.DepartmentId);             
                objParam[39] = model.OperatorId == null ? new SqlParameter("@OperatorId", DBNull.Value) : new SqlParameter("@OperatorId", model.OperatorId);             
                objParam[40] = model.UserVerify == null ? new SqlParameter("@UserVerify", DBNull.Value) : new SqlParameter("@UserVerify", model.UserVerify);             
                objParam[41] = model.VerifyDate == null ? new SqlParameter("@VerifyDate", DBNull.Value) : new SqlParameter("@VerifyDate", model.VerifyDate);             
                objParam[42] = model.DebtStatus == null ? new SqlParameter("@DebtStatus", DBNull.Value) : new SqlParameter("@DebtStatus", model.DebtStatus);             
                objParam[43] = model.DebtNote == null ? new SqlParameter("@DebtNote", DBNull.Value) : new SqlParameter("@DebtNote", model.DebtNote);             
                objParam[44] = model.Commission == null ? new SqlParameter("@Commission", DBNull.Value) : new SqlParameter("@Commission", model.Commission);             
                objParam[45] = model.UtmMedium == null ? new SqlParameter("@UtmMedium", DBNull.Value) : new SqlParameter("@UtmMedium", model.UtmMedium);             
                objParam[46] = model.Refund == null ? new SqlParameter("@Refund", DBNull.Value) : new SqlParameter("@Refund", model.Refund);             

                return _DbWorker.ExecuteNonQuery(StoreProcedureConstant.Sp_UpdateOrder, objParam);

            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UpdateOrderSaler - OrderDal: " + ex);
                return -2;
            }
        }
    }
}
