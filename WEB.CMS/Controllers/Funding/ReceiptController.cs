﻿using Entities.Models;
using Entities.ViewModels;
using Entities.ViewModels.Attachment;
using Entities.ViewModels.Funding;
using Entities.ViewModels.HotelBookingCode;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Utilities;
using Utilities.Contants;
using WEB.Adavigo.CMS.Service;
using WEB.Adavigo.CMS.Service.ServiceInterface;
using WEB.CMS.Customize;
using WEB.CMS.Models;

namespace WEB.Adavigo.CMS.Controllers
{
    [CustomAuthorize]

    public class ReceiptController : Controller
    {
        private readonly IAllCodeRepository _allCodeRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        private readonly IClientRepository _clientRepository;
        private readonly IDepositHistoryRepository _depositHistoryRepository;
        private readonly IContractPayRepository _contractPayRepository;
        private IIdentifierServiceRepository identifierServiceRepository;
        private readonly IUserRepository _userRepository;
        private ManagementUser _ManagementUser;
        private readonly IOrderRepository _orderRepository;
        private APIService apiService;
        private readonly IHotelBookingRepositories _hotelBookingRepositories;
        private readonly ITourRepository _tourRepository;
        private readonly IEmailService _emailService;
        private readonly WEB.CMS.Models.AppSettings config;
        private readonly IConfiguration _configuration;
        private readonly IPaymentRequestRepository _paymentRequestRepository;
        private readonly ISupplierRepository _supplierRepository;

        public ReceiptController(IContractPayRepository contractPayRepository, IAllCodeRepository allCodeRepository, IWebHostEnvironment hostEnvironment, IHotelBookingRepositories hotelBookingRepositories, ITourRepository tourRepository,
            IClientRepository clientRepository, IDepositHistoryRepository depositHistoryRepository, IOrderRepository orderRepository, ManagementUser ManagementUser,
             IUserRepository userRepository, IIdentifierServiceRepository _identifierServiceRepository, IPaymentRequestRepository paymentRequestRepository,
             IConfiguration configuration, ISupplierRepository supplierRepository, IEmailService emailService)
        {
            _supplierRepository = supplierRepository;
            _WebHostEnvironment = hostEnvironment;
            _clientRepository = clientRepository;
            _allCodeRepository = allCodeRepository;
            _contractPayRepository = contractPayRepository;
            _depositHistoryRepository = depositHistoryRepository;
            _orderRepository = orderRepository;
            _ManagementUser = ManagementUser;
            _userRepository = userRepository;
            identifierServiceRepository = _identifierServiceRepository;
            apiService = new APIService(configuration, userRepository);
            _hotelBookingRepositories = hotelBookingRepositories;
            _paymentRequestRepository = paymentRequestRepository;
            _tourRepository = tourRepository;
            _emailService = emailService;
            _configuration = configuration;
            config = ReadFile.LoadConfig();
        }

        public IActionResult Index()
        {
            ViewBag.allCode_PAY_TYPE = _allCodeRepository.GetListByType(AllCodeType.PAY_TYPE);
            ViewBag.allCode_CONTRACT_PAY_TYPE = _allCodeRepository.GetListByType(AllCodeType.CONTRACT_PAY_TYPE);
            return View();
        }

        [HttpPost]
        public IActionResult Search(ContractPaySearchModel searchModel, int currentPage = 1, int pageSize = 20)
        {
            var model = new GenericViewModel<ContractPayViewModel>();
            try
            {
                if (searchModel.CreateByIds == null) searchModel.CreateByIds = new List<int>();
                if (!string.IsNullOrEmpty(searchModel.BillNo))
                    searchModel.BillNo = searchModel.BillNo.Trim();
                if (!string.IsNullOrEmpty(searchModel.Content))
                    searchModel.Content = searchModel.Content.Trim();
                var listContractPays = _contractPayRepository.GetListContractPay(searchModel, out long total, currentPage, pageSize);
                model.CurrentPage = currentPage;
                model.ListData = listContractPays;
                model.PageSize = pageSize;
                model.TotalRecord = total;
                model.TotalPage = (int)Math.Ceiling((double)total / pageSize);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("Search - ReceiptController: " + ex);
            }
            return PartialView(model);
        }

        public IActionResult AddNew()
        {
            ViewBag.allCode_PAY_TYPE = _allCodeRepository.GetListByType(AllCodeType.PAY_TYPE);
            ViewBag.allCode_CONTRACT_PAY_TYPE = _allCodeRepository.GetListByType(AllCodeType.CONTRACT_PAY_TYPE);
            ViewBag.listBankingAccount = _allCodeRepository.GetBankingAccounts().Where(n => n.SupplierId ==
            (long)config.SUPPLIERID_ADAVIGO).ToList();
            return PartialView();
        }

        public IActionResult AddNewType()
        {
            ViewBag.allCode_PAY_TYPE = _allCodeRepository.GetListByType(AllCodeType.PAY_TYPE);
            ViewBag.allCode_CONTRACT_PAY_TYPE = _allCodeRepository.GetListByType(AllCodeType.CONTRACT_PAY_TYPE);
            ViewBag.listBankingAccount = _allCodeRepository.GetBankingAccounts().Where(n => n.SupplierId ==
            (long)config.SUPPLIERID_ADAVIGO).ToList();
            return PartialView();
        }

        public IActionResult Edit(int payId)
        {
            ViewBag.allCode_PAY_TYPE = _allCodeRepository.GetListByType(AllCodeType.PAY_TYPE);
            ViewBag.allCode_CONTRACT_PAY_TYPE = _allCodeRepository.GetListByType(AllCodeType.CONTRACT_PAY_TYPE);
            ViewBag.listBankingAccount = _allCodeRepository.GetBankingAccounts().Where(n => n.SupplierId ==
            (long)config.SUPPLIERID_ADAVIGO).ToList();
            var contractPay = _contractPayRepository.GetByContractPayId(payId);
            return PartialView(contractPay);
        }

        public IActionResult EditAdmin(int payId)
        {
            ViewBag.allCode_PAY_TYPE = _allCodeRepository.GetListByType(AllCodeType.PAY_TYPE);
            ViewBag.allCode_CONTRACT_PAY_TYPE = _allCodeRepository.GetListByType(AllCodeType.CONTRACT_PAY_TYPE);
            ViewBag.listBankingAccount = _allCodeRepository.GetBankingAccounts().Where(n => n.SupplierId ==
            (long)config.SUPPLIERID_ADAVIGO).ToList();
            var contractPay = _contractPayRepository.GetByContractPayId(payId);
            return PartialView(contractPay);
        }

        public IActionResult EditNewType(int payId)
        {
            ViewBag.allCode_PAY_TYPE = _allCodeRepository.GetListByType(AllCodeType.PAY_TYPE);
            ViewBag.allCode_CONTRACT_PAY_TYPE = _allCodeRepository.GetListByType(AllCodeType.CONTRACT_PAY_TYPE);
            ViewBag.listBankingAccount = _allCodeRepository.GetBankingAccounts().Where(n => n.SupplierId ==
            (long)config.SUPPLIERID_ADAVIGO).ToList();
            var contractPay = _contractPayRepository.GetByPayId(payId);
            return PartialView(contractPay);
        }

        private int Validate(ContractPayViewModel model, out string message)
        {
            var result = 1;
            message = string.Empty;
            if (model.Type == 0)
            {
                message = "Vui lòng chọn loại nghiệp vụ";
                return -1;
            }
            if (model.PayType == 0)
            {
                message = "Vui lòng chọn hình thức";
                return -1;
            }
            if (model.ClientId == 0)
            {
                message = "Vui lòng chọn khách hàng";
                return -1;
            }
            //if (model.Amount == 0)
            //{
            //    message = "Vui lòng nhập số tiền";
            //    return -1;
            //}
            if (model.Type == (int)DepositHistoryConstant.CONTRACT_PAYMENT_TYPE.CHUYEN_KHOAN && model.BankingAccountId == 0)
            {
                message = "Vui lòng chọn tài khoản ngân hàng nhận";
                return -1;
            }
            if (string.IsNullOrEmpty(model.Note))
            {
                message = "Vui lòng nhập nội dung";
                return -1;
            }
            if (!string.IsNullOrEmpty(model.Note) && model.Note.Length > 500)
            {
                message = "Nội dung chỉ được nhập tối đa 500 kí tự";
                return -1;
            }
            if (!string.IsNullOrEmpty(model.Description) && model.Description.Length > 3000)
            {
                message = "Ghi chú chỉ được nhập tối đa 3000 kí tự";
                return -1;
            }

            return result;
        }

        [HttpPost]
        public async Task<IActionResult> AddNewJson(IFormFile imagefile, string jsonData)
        {
            try
            {
                var userLogin = 0;
                if (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
                {
                    userLogin = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                }
                ContractPayViewModel model = JsonConvert.DeserializeObject<ContractPayViewModel>(jsonData);
                model.CreatedBy = userLogin;
                if (model.ContractPayDetails == null)
                {
                    model.ContractPayDetails = new List<ContractPayDetailViewModel>();
                }
                var validate = Validate(model, out string messages);
                if (validate < 0)
                {
                    return Ok(new
                    {
                        isSuccess = false,
                        message = messages
                    });
                }
                if (imagefile != null)
                {
                    string _FileName = Guid.NewGuid() + Path.GetExtension(imagefile.FileName);
                    string _UploadFolder = @"uploads/images";
                    string _UploadDirectory = Path.Combine(_WebHostEnvironment.WebRootPath, _UploadFolder);

                    if (!Directory.Exists(_UploadDirectory))
                    {
                        Directory.CreateDirectory(_UploadDirectory);
                    }

                    string filePath = Path.Combine(_UploadDirectory, _FileName);

                    if (!System.IO.File.Exists(filePath))
                    {
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await imagefile.CopyToAsync(fileStream);
                        }
                    }
                    model.AttatchmentFile = "/" + _UploadFolder + "/" + _FileName;
                }
                //model.BillNo = await identifierServiceRepository.buildContractPay();
                var client = new HttpClient();
                var apiPrefix = ReadFile.LoadConfig().API_URL + ReadFile.LoadConfig().API_GET_BILL_NO;
                var key_token_api = ReadFile.LoadConfig().KEY_TOKEN_API_MANUAL;
                HttpClient httpClient = new HttpClient();
                JObject jsonObject = new JObject(
                   new JProperty("code_type", ((int)GET_CODE_MODULE.PHIEU_THU).ToString())
                );
                var j_param = new Dictionary<string, object>
                 {
                     { "key",jsonObject}
                 };
                var data_product = JsonConvert.SerializeObject(j_param);
                var token = CommonHelper.Encode(data_product, key_token_api);
                var content = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("token", token) });
                var response = await httpClient.PostAsync(apiPrefix, content);
                var resultAPI = await response.Content.ReadAsStringAsync();
                var output = JsonConvert.DeserializeObject<OutputAPI>(resultAPI);
                model.BillNo = output.code;
                var contractPayId = _contractPayRepository.CreateContractPay(model);
                if (contractPayId == -2)
                    return Ok(new
                    {
                        isSuccess = false,
                        message = "Mã phiếu thu " + model.BillNo + " đã tồn tại trên hệ thống. Vui lòng kiểm tra lại."
                    });
                if (contractPayId < 1)
                    return Ok(new
                    {
                        isSuccess = false,
                        message = "Thêm phiếu thu thất bại"
                    });
                long _UserId = 0;
                if (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
                {
                    _UserId = Convert.ToInt64(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                }
                var current_user = _ManagementUser.GetCurrentUser();
                foreach (var item in model.ContractPayDetails)
                {
                    string link = "/Receipt/Detail?contractPayId=" + contractPayId;
                    apiService.SendMessage(_UserId.ToString(), ((int)ModuleType.PHIEU_THU).ToString(),
                       ((int)ActionType.TAO_MOI_PHIEU_THU).ToString(), item.OrderCode, link, current_user == null ? "0" : current_user.Role);

                    var data = await _orderRepository.GetAllServiceByOrderId(item.OrderId);
                    if (data != null)
                        foreach (var item2 in data)
                        {
                            if (item2.Type.Equals("Tour"))
                            {
                                apiService.SendMessage(_UserId.ToString(), ((int)ModuleType.DON_HANG).ToString(), ((int)ActionType.DUYET_DICH_VU).ToString(), item2.OrderNo, link, current_user.Role, item2.ServiceCode);
                            }
                            if (item2.Type.Equals("Khách sạn"))
                            {
                                apiService.SendMessage(_UserId.ToString(), ((int)ModuleType.DON_HANG).ToString(), ((int)ActionType.DUYET_DICH_VU).ToString(), item2.OrderNo, link, current_user.Role, item2.ServiceCode);
                            }
                            if (item2.Type.Equals("Vé máy bay"))
                            {
                                apiService.SendMessage(_UserId.ToString(), ((int)ModuleType.DON_HANG).ToString(), ((int)ActionType.DUYET_DICH_VU).ToString(), item2.OrderNo, link, current_user.Role, item2.ServiceCode);
                            }
                        }
                    var modelEmail = new SendEmailViewModel();
                    var attach_file = new List<AttachfileViewModel>();
                    modelEmail.Orderid = item.OrderId;
                    var ContractPayByOrderId = await _contractPayRepository.GetContractPayByOrderId(item.OrderId);
                    modelEmail.ServiceType = (int)EmailType.SaleDH;
                    var order = await _orderRepository.GetOrderByID(item.OrderId);
                    if (ContractPayByOrderId != null && ContractPayByOrderId.Count <= 1 && order.OrderStatus == (int)OrderStatus.WAITING_FOR_OPERATOR)
                    {
                        _emailService.SendEmail(modelEmail, attach_file);
                    }
                }
                return Ok(new
                {
                    isSuccess = true,
                    message = "Thêm phiếu thu thành công"
                });
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("AddNewJson - ReceiptController: " + ex);
                return Ok(new
                {
                    isSuccess = false,
                    message = "Thêm phiếu thu thất bại. Đã có lỗi xảy ra. Vui lòng liên hệ với quản trị viên"
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(IFormFile imagefile, string jsonData)
        {
            try
            {
                var userLogin = 0;
                if (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
                {
                    userLogin = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                }
                ContractPayViewModel model = JsonConvert.DeserializeObject<ContractPayViewModel>(jsonData);
                model.UpdatedBy = userLogin;
                if (model.ContractPayDetails == null)
                {
                    model.ContractPayDetails = new List<ContractPayDetailViewModel>();
                }
                var validate = Validate(model, out string messages);
                if (validate < 0)
                {
                    return Ok(new
                    {
                        isSuccess = false,
                        message = messages
                    });
                }
                if (imagefile != null)
                {
                    string _FileName = Guid.NewGuid() + Path.GetExtension(imagefile.FileName);
                    string _UploadFolder = @"uploads/images";
                    string _UploadDirectory = Path.Combine(_WebHostEnvironment.WebRootPath, _UploadFolder);

                    if (!Directory.Exists(_UploadDirectory))
                    {
                        Directory.CreateDirectory(_UploadDirectory);
                    }

                    string filePath = Path.Combine(_UploadDirectory, _FileName);

                    if (!System.IO.File.Exists(filePath))
                    {
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await imagefile.CopyToAsync(fileStream);
                        }
                    }
                    model.AttatchmentFile = "/" + _UploadFolder + "/" + _FileName;
                }
                var result = _contractPayRepository.UpdateContractPay(model);
                if (result < 1)
                    return Ok(new
                    {
                        isSuccess = false,
                        message = "Cập nhật phiếu thu thất bại"
                    });
                foreach (var item in model.ContractPayDetails)
                {
                    var modelEmail = new SendEmailViewModel();
                    var attach_file = new List<AttachfileViewModel>();
                    modelEmail.Orderid = item.OrderId;
                    var ContractPayByOrderId = await _contractPayRepository.GetContractPayByOrderId(item.OrderId);
                    modelEmail.ServiceType = (int)EmailType.SaleDH;
                    if (ContractPayByOrderId != null && ContractPayByOrderId.Count <= 1)
                    {
                        _emailService.SendEmail(modelEmail, attach_file);
                    }
                }

                return Ok(new
                {
                    isSuccess = true,
                    message = "Cập nhật phiếu thu thành công"
                });
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("Update - ReceiptController: " + ex);
                return Ok(new
                {
                    isSuccess = false,
                    message = "Cập nhật phiếu thu thất bại"
                });
            }
        }

        public async Task<IActionResult> Detail(int contractPayId)
        {
            ContractPayViewModel contractPay = _contractPayRepository.GetByPayId(contractPayId);
            if (contractPay.RelateData == null) contractPay.RelateData = new List<ContractPayDetailViewModel>();
            var current_user = _ManagementUser.GetCurrentUser();
            ViewBag.CHINH_SUA_PHIEU_THU = 0;
            if (current_user != null && !string.IsNullOrEmpty(current_user.Role))
            {
                var listRole = current_user.Role.Split(',').Select(n => Convert.ToInt64(n)).ToList();
                var checkRolePermission = await _userRepository.CheckRolePermissionByUserAndRole(current_user.Id, listRole,
                    (int)SortOrder.SUA, (int)MenuId.PHIEU_THU);
                ViewBag.CHINH_SUA_PHIEU_THU = checkRolePermission ? 1 : 0;
            }

            bool isAdmin = _userRepository.IsAdmin(current_user.Id);
            ViewBag.isAdmin = isAdmin;
            return View(contractPay);
        }

        public async Task<IActionResult> DetailNew(int contractPayId)
        {
            ContractPayViewModel contractPay = _contractPayRepository.GetByPayId(contractPayId);
            if (contractPay.RelateData == null) contractPay.RelateData = new List<ContractPayDetailViewModel>();
            var current_user = _ManagementUser.GetCurrentUser();
            ViewBag.CHINH_SUA_PHIEU_THU = 0;
            if (current_user != null && !string.IsNullOrEmpty(current_user.Role))
            {
                var listRole = current_user.Role.Split(',').Select(n => Convert.ToInt64(n)).ToList();
                var checkRolePermission = await _userRepository.CheckRolePermissionByUserAndRole(current_user.Id, listRole,
                    (int)SortOrder.SUA, (int)MenuId.PHIEU_THU);
                ViewBag.CHINH_SUA_PHIEU_THU = checkRolePermission ? 1 : 0;
            }
            bool isAdmin = _userRepository.IsAdmin(current_user.Id);
            ViewBag.isAdmin = isAdmin;
            return View(contractPay);
        }

        [HttpPost]
        public IActionResult ExportExcel(ContractPaySearchModel searchModel)
        {
            try
            {
                string _FileName = "ContractPay_" + Guid.NewGuid() + ".xlsx";
                string _UploadFolder = @"Template/Export";
                string _UploadDirectory = Path.Combine(_WebHostEnvironment.WebRootPath, _UploadFolder);

                if (!Directory.Exists(_UploadDirectory))
                {
                    Directory.CreateDirectory(_UploadDirectory);
                }
                //delete all file in folder before export
                try
                {
                    System.IO.DirectoryInfo di = new DirectoryInfo(_UploadDirectory);
                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                }
                catch
                {
                }
                if (searchModel.CreateByIds == null) searchModel.CreateByIds = new List<int>();
                if (!string.IsNullOrEmpty(searchModel.BillNo))
                    searchModel.BillNo = searchModel.BillNo.Trim();
                if (!string.IsNullOrEmpty(searchModel.Content))
                    searchModel.Content = searchModel.Content.Trim();

                string FilePath = Path.Combine(_UploadDirectory, _FileName);
                var rsPath = _contractPayRepository.ExportDeposit(searchModel, FilePath);

                if (!string.IsNullOrEmpty(rsPath))
                {
                    return new JsonResult(new
                    {
                        isSuccess = true,
                        message = "Xuất dữ liệu thành công",
                        path = "/" + _UploadFolder + "/" + _FileName
                    });
                }
                else
                {
                    return new JsonResult(new
                    {
                        isSuccess = false,
                        message = "Xuất dữ liệu thất bại"
                    });
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    isSuccess = false,
                    message = ex.Message.ToString()
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetOrderListByClientId(int clientId, int payId = 0)
        {
            try
            {
                var listOrder = _orderRepository.GetByClientId(clientId, payId);
                return Ok(new
                {
                    isSuccess = true,
                    message = "Thành công",
                    data = listOrder
                });
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetOrderListByClientId - ReceiptController: " + ex);
                return Ok(new
                {
                    isSuccess = false,
                    message = "Thất bại",
                    data = new List<Entities.Models.Order>()
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetDepositListByClientId(int clientId, int payId = 0)
        {
            try
            {
                var listDepositHistory = _depositHistoryRepository.GetByClientId(clientId, payId);
                return Ok(new
                {
                    isSuccess = true,
                    message = "Thành công",
                    data = listDepositHistory
                });
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetDepositListByClientId - ReceiptController: " + ex);
                return Ok(new
                {
                    isSuccess = false,
                    message = "Thất bại",
                    data = new List<DepositHistory>()
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetServiceListBySupplierId(int supplierId, int contractPayId = 0, int serviceId = 0)
        {
            try
            {
                var listOrder = _contractPayRepository.GetContractPayServiceListBySupplierId(supplierId, contractPayId, serviceId);
                return Ok(new
                {
                    isSuccess = true,
                    message = "Thành công",
                    data = listOrder
                });
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetServiceListBySupplierId - ReceiptController: " + ex + ". Đã có lỗi xảy ra");
                return Ok(new
                {
                    isSuccess = false,
                    message = "Thất bại" + ". Đã có lỗi xảy ra",
                    data = new List<Entities.Models.Order>()
                });
            }
        }

        public async Task<string> GetUserCreateSuggest(string name)
        {
            try
            {
                var accountClients = await _clientRepository.GetUserCreateSuggest(name);
                var suggestionlist = accountClients.Select(s => new
                {
                    id = s.Id,
                    name = s.UserName,
                }).ToList();
                return JsonConvert.SerializeObject(suggestionlist);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetUserCreateSuggest - ReceiptController: " + ex);
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetListBankAccountAdavigo()
        {
            try
            {
                var supplierAdavigoId = _supplierRepository.GetByIDOrName(0, "adavigo");
                var listBankAccount = _allCodeRepository.GetBankingAccountsBySupplierId(supplierAdavigoId);
                return Ok(new
                {
                    isSuccess = true,
                    message = "Thành công",
                    data = listBankAccount
                });
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetListBankAccountAdavigo - ReceiptController: " + ex);
                return Ok(new
                {
                    isSuccess = false,
                    message = "Thất bại",
                    data = new List<BankingAccount>()
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetListServiceFilter(string jsonData, string text = null)
        {
            try
            {
                if (string.IsNullOrEmpty(jsonData))
                    return Ok(new
                    {
                        isSuccess = true,
                        message = "Thành công",
                        data = new List<PaymentRequestViewModel>()
                    });
                var listFilter = JsonConvert.DeserializeObject<List<PaymentRequestViewModel>>(jsonData);
                if (!string.IsNullOrEmpty(text))
                    listFilter = listFilter.Where(n => n.ServiceCode.Contains(text.Trim())).ToList();
                return Ok(new
                {
                    isSuccess = true,
                    message = "Thành công",
                    data = listFilter
                });
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetListServiceFilter - PaymentVoucherController: " + ex);
                return Ok(new
                {
                    isSuccess = false,
                    message = "Thất bại",
                    data = new List<OrderViewModel>()
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetOrderDetail(string orderCode)
        {
            try
            {
                if (string.IsNullOrEmpty(orderCode))
                    return Ok(new
                    {
                        isSuccess = true,
                        message = "Thành công",
                        data = new OrderViewModel()
                    });
                var orderViewModel = new OrderViewModel();
                var orderDetail = _orderRepository.GetOrderByOrderNo(orderCode).Result;
                if (orderDetail == null)
                    return Ok(new
                    {
                        isSuccess = false,
                        message = "Đơn hàng không tồn tại trên hệ thống. Vui lòng kiểm tra lại",
                    });
                orderDetail.CopyProperties(orderViewModel);
                orderViewModel.OrderCode = orderDetail.OrderNo;
                orderViewModel.OrderId = orderDetail.OrderId.ToString();
                orderViewModel.ClientId = orderDetail.ClientId;
                orderViewModel.StartDate = orderDetail.StartDate != null ? orderDetail.StartDate.Value.ToString("dd/MM/yyyy") : "";
                orderViewModel.EndDate = orderDetail.EndDate != null ? orderDetail.EndDate.Value.ToString("dd/MM/yyyy") : "";
                var client = _clientRepository.GetClientDetailByClientId(orderDetail.ClientId.Value).Result;
                orderViewModel.ClientName = client.ClientName;
                return Ok(new
                {
                    isSuccess = true,
                    message = "Thành công",
                    data = orderViewModel
                });
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetListFilter - PaymentVoucherController: " + ex);
                return Ok(new
                {
                    isSuccess = false,
                    message = "Thất bại",
                    data = new List<OrderViewModel>()
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetServiceDetail(string serviceCode)
        {
            try
            {
                if (string.IsNullOrEmpty(serviceCode))
                    return Ok(new
                    {
                        isSuccess = true,
                        message = "Thành công",
                        data = new PaymentRequestViewModel()
                    });
                var requestViewModel = new PaymentRequestViewModel();
                var serviceDetail = _contractPayRepository.GetServiceDetail(serviceCode);
                if (serviceDetail == null)
                    return Ok(new
                    {
                        isSuccess = false,
                        message = "Dịch vụ không tồn tại trên hệ thống. Vui lòng kiểm tra lại",
                    });
                serviceDetail.CopyProperties(requestViewModel);
                //orderViewModel.OrderCode = orderDetail.OrderNo;
                //orderViewModel.OrderId = orderDetail.OrderId.ToString();
                //orderViewModel.ClientId = orderDetail.ClientId;
                //orderViewModel.StartDate = orderDetail.StartDate != null ? orderDetail.StartDate.Value.ToString("dd/MM/yyyy") : "";
                //orderViewModel.EndDate = orderDetail.EndDate != null ? orderDetail.EndDate.Value.ToString("dd/MM/yyyy") : "";
                //var client = _clientRepository.GetClientDetailByClientId(orderDetail.ClientId.Value).Result;
                //orderViewModel.ClientName = client.ClientName;
                return Ok(new
                {
                    isSuccess = true,
                    message = "Thành công",
                    data = requestViewModel
                });
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetListFilter - PaymentVoucherController: " + ex);
                return Ok(new
                {
                    isSuccess = false,
                    message = "Thất bại",
                    data = new List<OrderViewModel>()
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetListFilter(string jsonData, string text = null)
        {
            try
            {
                if (string.IsNullOrEmpty(jsonData))
                    return Ok(new
                    {
                        isSuccess = true,
                        message = "Thành công",
                        data = new List<OrderViewModel>()
                    });
                var listFilter = JsonConvert.DeserializeObject<List<OrderViewModel>>(jsonData);
                if (!string.IsNullOrEmpty(text))
                    listFilter = listFilter.Where(n => n.OrderCode.Contains(text.Trim())).ToList();
                return Ok(new
                {
                    isSuccess = true,
                    message = "Thành công",
                    data = listFilter
                });
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetListFilter - PaymentVoucherController: " + ex);
                return Ok(new
                {
                    isSuccess = false,
                    message = "Thất bại",
                    data = new List<OrderViewModel>()
                });
            }
        }
    }
}
