using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Models;
using Entities.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Repositories.IRepositories;
using Utilities;
using WEB.CMS.Customize;

namespace Web.CMS.Controllers.Account
{
    public class AccountAccessApiController : Controller
    {
        private readonly IWebHostEnvironment _WebHostEnvironment;
        private readonly IUserRepository _UserRepository;
        private readonly IDepartmentRepository _DepartmentRepository;
        private readonly IRoleRepository _RoleRepository;
        private readonly IMFARepository _mFARepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ManagementUser _ManagementUser;
        private readonly IConfiguration _configuration;


        public AccountAccessApiController(IUserRepository userRepository, IRoleRepository roleRepository,
            IWebHostEnvironment hostEnvironment, IMFARepository mFARepository,
            IDepartmentRepository departmentRepository, ManagementUser managementUser, IOrderRepository orderRepository, IConfiguration configuration)
        {
            _UserRepository = userRepository;
            _RoleRepository = roleRepository;
            _WebHostEnvironment = hostEnvironment;
            _mFARepository = mFARepository;
            _DepartmentRepository = departmentRepository;
            _ManagementUser = managementUser;
            _orderRepository = orderRepository;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Search(string userName, string strRoleId, int status = -1, int currentPage = 1, int pageSize = 20)
        {
            var model = new GenericViewModel<UserGridModel>();
            try
            {
                model = _UserRepository.GetPagingList(userName, strRoleId, status, currentPage, pageSize);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("Search - UserController: " + ex);
            }
            return PartialView(model);
        }

        public async Task<IActionResult> AddOrUpdate(int Id, bool IsClone = false)
        {
            try
            {
                var model = new User();
                ViewBag.UserRoleList = null;
                ViewBag.CompanyType = "";
                if (Id != 0)
                {

                    model = await _UserRepository.FindById(Id);
                    if (IsClone)
                    {
                        model = new User
                        {
                            FullName = model.FullName,
                            UserName = model.UserName,
                            Email = model.Email,
                            Address = model.Address,
                            BirthDay = model.BirthDay,
                            Gender = model.Gender,
                            Status = model.Status,
                            Note = model.Note,
                            DepartmentId = model.DepartmentId,
                            Phone = model.Phone,
                        };
                    }
                    var list_role_active = await _UserRepository.GetUserActiveRoleList(model.Id);
                    if (list_role_active != null && list_role_active.Count > 0)
                    {
                        ViewBag.UserRoleList = list_role_active.Select(x => x.Id).ToList();
                    }
                }
                else
                {
                    model.Gender = 1;
                }

                ViewBag.DepartmentList = await _DepartmentRepository.GetAll(String.Empty);
                ViewBag.RoleList = await _RoleRepository.GetAll();
                ViewBag.UserPosition = _UserRepository.GetUserPositions();
                return View(model);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("AddOrUpdate - UserController: " + ex);
                return Content("");
            }

        }

        [HttpPost]
        public async Task<IActionResult> UpSert(UserViewModel model)
        {
            try
            {


                return new JsonResult(new
                {
                    isSuccess = true,
                    message = "Cập nhật thành công"
                });
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UpSert - UserController: " + ex);
                return new JsonResult(new
                {
                    isSuccess = false,
                    message = ex.Message.ToString()
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetDetail(int Id)
        {
            var model = new UserDataViewModel();
            try
            {
                model = await _UserRepository.GetUser(Id);
                var mfa_record = await _mFARepository.get_MFA_DetailByUserID(Id);
                ViewBag.RoleList = await _RoleRepository.GetAll();
                model.UserPositionName = model.UserPositionId != null && model.UserPositionId > 0 ? _UserRepository.GetUserPositionsByID((int)model.UserPositionId).Result.Name : "";
                ViewBag.IsMFAEnabled = (mfa_record != null && mfa_record.UserId == Id);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetDetail - UserController: " + ex);
                ViewBag.IsMFAEnabled = false;
            }
            return PartialView(model);
        }


    }
}
