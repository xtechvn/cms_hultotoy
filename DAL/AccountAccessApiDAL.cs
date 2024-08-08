using DAL.Generic;
using DAL.StoreProcedure;
using Entities.Models;
using Entities.ViewModels.AccountAccessAPI;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using Utilities.Contants;

namespace DAL
{
    public class AccountAccessApiDAL : GenericService<AccountAccessApi>
    {
        private static DbWorker _DbWorker;
        public AccountAccessApiDAL(string connection) : base(connection)
        {
            _DbWorker = new DbWorker(connection);
        }

        public async Task<int> InsertAccountAccessAPI(AccountAccessApiSubmitModel model) 
        {
            try 
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@UserName",model.UserName != null ? model.UserName : DBNull.Value),
                    new SqlParameter("@Password",model.Password != null ? PresentationUtils.Encrypt(model.Password) : DBNull.Value),
                    new SqlParameter("@Status",model.Status != null ? model.Status : DBNull.Value),
                    new SqlParameter("@CreateDate",DateTime.Now),
                    new SqlParameter("@UpdateLast",DateTime.Now),
                    new SqlParameter("@Description",model.Description != null ? model.Description : DBNull.Value)
                };
                return _DbWorker.ExecuteNonQuery(StoreProcedureConstant.sp_InsertAccountAccessAPI,sqlParameters);
            }
            catch (Exception ex) 
            {
                LogHelper.InsertLogTelegram("InsertAccountAccessAPI - AccountAccessApiDAL: " + ex);
                return -1;
            }
        }

        public async Task<int> UpdateAccountAccessAPI(AccountAccessApiSubmitModel model)
        {
            try
            {
                AccountAccessApi obj = null;
                
                using (var _DbContext = new EntityDataContext(_connection))
                {

                    obj = await _DbContext.AccountAccessApis.FindAsync(model.Id);
                }

                if (obj == null) 
                {
                    LogHelper.InsertLogTelegram("UpdateAccountAccessAPI - AccountAccessApiDAL: Not Found Object");
                    return -1;
                }

                string password = model.Password!= null ? PresentationUtils.Encrypt(model.Password) : obj.Password;

                SqlParameter[] sqlParameters = new SqlParameter[]
                 {
                    new SqlParameter("@Id", model.Id),
                    new SqlParameter("@UserName",model.UserName != null ? model.UserName : DBNull.Value),
                    new SqlParameter("@Password",password),
                    new SqlParameter("@Status",model.Status != null ? model.Status : DBNull.Value),
                    new SqlParameter("@CreateDate",DateTime.Now),
                    new SqlParameter("@UpdateLast",DateTime.Now),
                    new SqlParameter("@Description",model.Description != null ? model.Description : DBNull.Value)
                 };
                return _DbWorker.ExecuteNonQuery(StoreProcedureConstant.sp_UpdateAccountAccessAPI, sqlParameters);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UpdateAccountAccessAPI - AccountAccessApiDAL: " + ex);
                return -1;
            }
        }

        public async Task<int> ResetPassword(int id)
        {
            try
            {
                AccountAccessApi obj = null;

                using (var _DbContext = new EntityDataContext(_connection))
                {

                    obj = await _DbContext.AccountAccessApis.FindAsync(id);
                }

                if (obj == null)
                {
                    LogHelper.InsertLogTelegram("ResetPassword - AccountAccessApiDAL: Not Found Object");
                    return -1;
                }

                string password = PresentationUtils.Encrypt("abc123");

                SqlParameter[] sqlParameters = new SqlParameter[]
                 {
                    new SqlParameter("@Id", obj.Id),
                    new SqlParameter("@UserName",obj.UserName != null ? obj.UserName : DBNull.Value),
                    new SqlParameter("@Password",password),
                    new SqlParameter("@Status",obj.Status != null ? obj.Status : DBNull.Value),
                    new SqlParameter("@CreateDate",DateTime.Now),
                    new SqlParameter("@UpdateLast",DateTime.Now),
                    new SqlParameter("@Description",obj.Description)
                 };
                return _DbWorker.ExecuteNonQuery(StoreProcedureConstant.sp_UpdateAccountAccessAPI, sqlParameters);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("ResetPassword - AccountAccessApiDAL: " + ex);
                return -1;
            }
        }
    }
}
