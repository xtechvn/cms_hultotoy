using DAL;
using Entities.ConfigModels;
using Entities.Models;
using Entities.ViewModels.Hotel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Repositories.IRepositories;
using Repositories.Repositories.BaseRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Repositories.Repositories
{
    public class HotelRepository : BaseRepository, IHotelRepository
    {
        private readonly HotelDAL _HotelDAL;
        private readonly string _UrlStaticImage;
 
        public HotelRepository(IHttpContextAccessor context, IOptions<DataBaseConfig> dataBaseConfig, IOptions<DomainConfig> domainConfig, IUserRepository userRepository, IConfiguration configuration) : base(context, dataBaseConfig, configuration, userRepository)
        {
            _HotelDAL = new HotelDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
            _UrlStaticImage = domainConfig.Value.ImageStatic;
        }

        public int DeleteHotelBankingAccount(int id)
        {
            try
            {
                return _HotelDAL.DeleteHotelBankingAccountById(id);
            }
            catch
            {
                throw;
            }
        }

        public int DeleteHotelContact(int id)
        {
            try
            {
                return _HotelDAL.DeleteHotelContactById(id);
            }
            catch
            {
                throw;
            }
        }

        public int DeleteHotelRoom(int id)
        {
            try
            {
                var room_model = GetHotelRoomById(id);
                if (room_model != null)
                {
                    var is_used = _HotelDAL.CheckExistHotelRoomUsing((int)room_model.HotelId, id);
                    if (is_used < 0) return -2;
                }


                return _HotelDAL.DeleteHotelRoom(id);
            }
            catch
            {
                throw;
            }
        }

        public int DeleteHotelSurcharge(int id)
        {
            try
            {
                return _HotelDAL.DeleteHotelSurchargeById(id);
            }
            catch
            {
                throw;
            }
        }

        public HotelBankingAccount GetHotelBankingAccountById(int Id)
        {
            try
            {
                return _HotelDAL.GetHotelBankingAccountById(Id);
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<HotelBankingAccountGridModel> GetHotelBankingAccountList(int hotel_id)
        {
            try
            {
                var dataTable = _HotelDAL.GetListHotelBankingAccountByHotelId(hotel_id);
                return dataTable.ToList<HotelBankingAccountGridModel>();
            }
            catch
            {
                throw;
            }
        }

        public Hotel GetHotelById(int id)
        {
            try
            {
                return _HotelDAL.GetHotelById(id);
            }
            catch
            {
                throw;
            }
        }

        public HotelContact GetHotelContactById(int id)
        {
            try
            {
                return _HotelDAL.GetHotelContactById(id);
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<HotelContactGridModel> GetHotelContactList(int hotel_id)
        {
            try
            {
                var dataTable = _HotelDAL.GetHotelContactDataTable(hotel_id);
                return dataTable.ToList<HotelContactGridModel>();
            }
            catch
            {
                throw;
            }
        }

        public HotelDetailViewModel GetHotelDetailById(int id)
        {
            try
            {
                var dataTable = _HotelDAL.GetHotelDetailDataTable(id);
                return dataTable.ToList<HotelDetailViewModel>().FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }
        public Hotel GetHotelByHotelID(string hotel_id)
        {
            try
            {
                var hotel = _HotelDAL.GetByHotelId(hotel_id);
                return hotel;
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<HotelGridModel> GetHotelPagingList(HotelFilterModel model)
        {
            try
            {
                var dataTable = _HotelDAL.GetHotelPagingList(model);
                return dataTable.ToList<HotelGridModel>();
            }
            catch
            {
                throw;
            }
        }

        public HotelRoom GetHotelRoomById(int id)
        {
            try
            {
                var dataTable = _HotelDAL.GetDetailHotelRoom(id);
                return dataTable.ToList<HotelRoom>().FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<HotelRoomGridModel> GetHotelRoomList(int hotel_id, int page_index, int page_size)
        {
            try
            {
                var dataTable = _HotelDAL.GetHotelRoomByHotelId(hotel_id, page_index, page_size);
                return dataTable.ToList<HotelRoomGridModel>();
            }
            catch
            {
                throw;
            }
        }

        public HotelSurcharge GetHotelSurchargeById(int id)
        {
            try
            {
                return _HotelDAL.GetHotelSurchargeById(id);
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<HotelSurchargeGridModel> GetHotelSurchargeList(int hotel_id, int page_index, int page_size)
        {
            try
            {
                var dataTable = _HotelDAL.GetHotelSurchargeDataTable(hotel_id, page_index, page_size);
                return dataTable.ToList<HotelSurchargeGridModel>();
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<HotelViewModel> GetSuggestionHotelList(string name, int size)
        {
            try
            {
                var dataTable = _HotelDAL.GetSuggestHotelByName(name, size);
                return dataTable.ToList<HotelViewModel>();
            }
            catch
            {
                throw;
            }
        }

        public int SaveHotel(HotelUpsertViewModel model)
        {
            try
            {
       
                if (!string.IsNullOrEmpty(model.ImageThumb))
                    model.ImageThumb = _UrlStaticImage +  UpLoadHelper.UploadBase64Src(model.ImageThumb, _UrlStaticImage).Result;
                if (model.Id > 0)
                {
                    model.UpdatedBy = _SysUserModel.Id;
                    return _HotelDAL.UpdateHotel(model);
                }
                else
                {
                    model.CreatedBy = _SysUserModel.Id;
                    model.SalerId = _SysUserModel.Id;
                    return _HotelDAL.CreateHotel(model);
                }
            }
            catch
            {
                throw;
            }
        }

        public int UpsertHotelBankingAccount(HotelBankingAccount model)
        {
            try
            {
                if (model.Id > 0)
                {
                    model.UpdatedBy = _SysUserModel.Id;
                    return _HotelDAL.UpdateHotelBankingAccount(model);
                }
                else
                {
                    model.CreatedBy = _SysUserModel.Id;
                    return _HotelDAL.InsertHotelBankingAccount(model);
                }
            }
            catch
            {
                throw;
            }
        }
        public int UpsertHotelBankingAccountByName(HotelBankingAccount model)
        {
            try
            {
                if (model.Id > 0)
                {
                    model.UpdatedBy = _SysUserModel.Id;
                    return _HotelDAL.UpdateHotelBankingAccount(model);
                }
                else
                {
                    var exist_bankaccount = _HotelDAL.GetHotelBankingAccountByName(model.AccountName, (int)model.HotelId);
                    if (exist_bankaccount != null && exist_bankaccount.Id > 0)
                    {
                        model.UpdatedBy = _SysUserModel.Id;
                        return _HotelDAL.UpdateHotelBankingAccount(model);
                    }
                    else
                    {
                        model.CreatedBy = _SysUserModel.Id;
                        return _HotelDAL.InsertHotelBankingAccount(model);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public int UpsertHotelContact(HotelContact model)
        {
            try
            {
                if (model.Id > 0)
                {
                    model.UpdatedBy = _SysUserModel.Id;
                    return _HotelDAL.UpdateHotelContact(model);
                }
                else
                {
                    model.CreatedBy = _SysUserModel.Id;
                    return _HotelDAL.InsertHotelContact(model);
                }
            }
            catch
            {
                throw;
            }
        }

        public int UpsertHotelRoom(HotelRoomUpsertModel model)
        {
            try
            {
                if (!string.IsNullOrEmpty(model.Name)) model.Name = model.Name.Trim();

                var isExistName = _HotelDAL.CheckExistRoomName(model.Id, model.HotelId, model.Name);
                if (isExistName)
                    throw new Exception($"Tên phòng [{model.Name}] đã tồn tại trên hệ thống.");

                if (!string.IsNullOrEmpty(model.Avatar))
                {
                    var avt = UpLoadHelper.UploadBase64Src(model.Avatar, _UrlStaticImage).Result;
                    if (avt != null && avt.Trim() != "")
                    {
                        model.Avatar = avt;
                    }
                }

                if (model.OtherImages != null && model.OtherImages.Count() > 0)
                {
                    var arrImage = new List<string>();
                    foreach (var image in model.OtherImages)
                    {
                        var img = UpLoadHelper.UploadBase64Src(image, _UrlStaticImage).Result;
                        if(img!=null&& img.Trim() != "")
                        {
                            arrImage.Add(img);
                        }
                    }
                    model.RoomAvatar = String.Join(",", arrImage);
                }

                if (model.Id > 0)
                {
                    model.UpdatedBy = _SysUserModel.Id;
                    return _HotelDAL.UpdateHotelRoom(model);
                }
                else
                {
                    model.CreatedBy = _SysUserModel.Id;
                    return _HotelDAL.CreateHotelRoom(model);
                }
            }
            catch
            {
                throw;
            }
        }

        public int UpsertHotelSurcharge(HotelSurcharge model)
        {
            try
            {
                if (model.Id > 0)
                {
                    model.UpdatedBy = _SysUserModel.Id;
                    return _HotelDAL.UpdateHotelSurcharge(model);
                }
                else
                {
                    model.CreatedBy = _SysUserModel.Id;
                    return _HotelDAL.InsertHotelSurcharge(model);
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<int> UpsertHotelUltilities(int hotel_id, string extends)
        {
            try
            {
                var hotel_model = await _HotelDAL.FindAsync(hotel_id);
                if (hotel_model != null)
                {
                    hotel_model.Extends = extends;
                    await _HotelDAL.UpdateAsync(hotel_model);
                    return hotel_id;
                }
                else
                {
                    return -1;
                }
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> UpdateHotelSurchargeNote(string body, int id)
        {
            try
            {
                return await _HotelDAL.UpdateHotelSurchargeNote(body,id);
            }
            catch
            {
                throw;
            }
        }
       
        public List<HotelPricePolicyViewModel> GetHotelRoomPricePolicy(string hotel_id, string room_ids,  string client_types)
        {
            try
            {
                var dataTable = _HotelDAL.GetHotelPricePolicy(hotel_id, room_ids,  client_types);
                return dataTable.ToList<HotelPricePolicyViewModel>();
            }
            catch
            {
                return new List<HotelPricePolicyViewModel>();
            }
        }
      
     
    }
}
