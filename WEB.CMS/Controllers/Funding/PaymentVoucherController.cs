﻿using Entities.ViewModels;
using Entities.ViewModels.Funding;
using Entities.ViewModels.SupplierConfig;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
using WEB.CMS.Customize;
using WEB.CMS.Models;

namespace WEB.Adavigo.CMS.Controllers.Funding
{
    [CustomAuthorize]

    public class PaymentVoucherController : Controller
    {
        private readonly IAllCodeRepository _allCodeRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        private readonly IPaymentRequestRepository _paymentRequestRepository;
        private readonly IPaymentVoucherRepository _paymentVoucherRepository;
        private readonly string _UrlStaticImage;
        private readonly WEB.CMS.Models.AppSettings config;

        public PaymentVoucherController(IAllCodeRepository allCodeRepository, IWebHostEnvironment hostEnvironment,
           IPaymentRequestRepository paymentRequestRepository, IPaymentVoucherRepository paymentVoucherRepository)
        {
            _WebHostEnvironment = hostEnvironment;
            _allCodeRepository = allCodeRepository;
            _paymentRequestRepository = paymentRequestRepository;
            _paymentVoucherRepository = paymentVoucherRepository;
            config = ReadFile.LoadConfig();
        }

        public IActionResult Index()
        {
            ViewBag.allCode_PAY_TYPE = _allCodeRepository.GetListByType(AllCodeType.PAY_TYPE);
            ViewBag.allCode_PAYMENT_VOUCHER_TYPE = _allCodeRepository.GetListByType(AllCodeType.PAYMENT_VOUCHER_TYPE);
            ViewBag.listBankingAccountAdavigo = _allCodeRepository.GetBankingAccounts().Where(n => n.SupplierId ==
           (long)config.SUPPLIERID_ADAVIGO).ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Search(PaymentVoucherViewModel searchModel, int currentPage = 1, int pageSize = 20)
        {
            var model = new GenericViewModel<PaymentVoucherViewModel>();
            try
            {
                if (searchModel.CreateByIds == null) searchModel.CreateByIds = new List<int>();
                if (!string.IsNullOrEmpty(searchModel.PaymentCode))
                    searchModel.PaymentCode = searchModel.PaymentCode.Trim();
                if (!string.IsNullOrEmpty(searchModel.Content))
                    searchModel.Content = searchModel.Content.Trim();
                var listPaymentVouchers = _paymentVoucherRepository.GetPaymentVouchers(searchModel, out long total, currentPage, pageSize);
                model.CurrentPage = currentPage;
                model.ListData = listPaymentVouchers;
                model.PageSize = pageSize;
                model.TotalRecord = total;
                model.TotalPage = (int)Math.Ceiling((double)total / pageSize);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("Search - PaymentVoucherController: " + ex);
            }
            return PartialView(model);
        }

        public IActionResult AddNew()
        {
            ViewBag.allCode_PAY_TYPE = _allCodeRepository.GetListByType(AllCodeType.PAY_TYPE);
            ViewBag.allCode_PAYMENT_VOUCHER_TYPE = _allCodeRepository.GetListByType(AllCodeType.PAYMENT_VOUCHER_TYPE);
            ViewBag.listBankingAccount = _allCodeRepository.GetBankingAccounts();
            var userLogin = 0;
            if (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                userLogin = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            ViewBag.UserId = userLogin;
            ViewBag.Type = (int)AttachmentType.Payment_Voucher;
            ViewBag.listBankingAccountAdavigo = _allCodeRepository.GetBankingAccounts().Where(n => n.SupplierId ==
           (long)config.SUPPLIERID_ADAVIGO).ToList();
            return PartialView();
        }

        public IActionResult Edit(int paymentVoucherId)
        {
            ViewBag.allCode_PAY_TYPE = _allCodeRepository.GetListByType(AllCodeType.PAY_TYPE);
            ViewBag.allCode_PAYMENT_VOUCHER_TYPE = _allCodeRepository.GetListByType(AllCodeType.PAYMENT_VOUCHER_TYPE);
            ViewBag.listBankingAccount = _allCodeRepository.GetBankingAccounts();
            var detail = _paymentVoucherRepository.GetDetail(paymentVoucherId);
            var userLogin = 0;
            if (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                userLogin = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            ViewBag.UserId = userLogin;
            ViewBag.Type = (int)AttachmentType.Payment_Voucher;
            ViewBag.listBankingAccountAdavigo = _allCodeRepository.GetBankingAccounts().Where(n => n.SupplierId ==
         (long)config.SUPPLIERID_ADAVIGO).ToList();
            return PartialView(detail);
        }

        private int Validate(PaymentVoucherViewModel model, out string message)
        {
            var result = 1;
            message = string.Empty;
            if (model.Type == 0)
            {
                message = "Vui lòng chọn loại nghiệp vụ";
                return -1;
            }
            if (model.PaymentType == 0)
            {
                message = "Vui lòng chọn hình thức";
                return -1;
            }
            if (model.SupplierId == 0 && model.Type != (int)PAYMENT_VOUCHER_TYPE.HOAN_TRA_KHACH_HANG)
            {
                message = "Vui lòng chọn nhà cung cấp";
                return -1;
            }
            if (model.ClientId == 0 && model.Type == (int)PAYMENT_VOUCHER_TYPE.HOAN_TRA_KHACH_HANG)
            {
                message = "Vui lòng chọn khách hàng";
                return -1;
            }
            if (model.PaymentType == (int)DepositHistoryConstant.CONTRACT_PAYMENT_TYPE.CHUYEN_KHOAN
                && model.BankingAccountId == 0)
            {
                message = "Vui lòng chọn tài khoản ngân hàng nhận";
                return -1;
            }
            if (model.SourceAccount == 0)
            {
                message = "Vui lòng nhập tài khoản nguồn xuất tiền";
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
            if (model.PaymentRequestDetails == null || model.PaymentRequestDetails.Count == 0)
            {
                message = "Vui lòng chọn phiếu yêu cầu chi";
                return -1;
            }
            return result;
        }

        [HttpPost]
        public async Task<IActionResult> AddNewJson(PaymentVoucherViewModel model, string jsonData)
        {
            try
            {
                var userLogin = 0;
                if (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
                {
                    userLogin = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                }

                model.CreatedBy = userLogin;
                var validate = Validate(model, out string messages);
                if (validate < 0)
                {
                    return Ok(new
                    {
                        isSuccess = false,
                        message = messages
                    });
                }
                var client = new HttpClient();
                var apiPrefix = ReadFile.LoadConfig().API_URL + ReadFile.LoadConfig().API_GET_BILL_NO;
                var key_token_api = ReadFile.LoadConfig().KEY_TOKEN_API_MANUAL;
                HttpClient httpClient = new HttpClient();
                JObject jsonObject = new JObject(
                   new JProperty("code_type", ((int)GET_CODE_MODULE.PHIEU_CHI).ToString())
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
                model.PaymentCode = output.code;
                var result = _paymentVoucherRepository.CreatePaymentVoucher(model, out string msg);
                if (result == -3)
                    return Ok(new
                    {
                        isSuccess = false,
                        message = "Các phiếu yêu cầu chi " + msg + " đã được tạo phiếu chi. Vui lòng kiểm tra lại."
                    });
                if (result == -2)
                    return Ok(new
                    {
                        isSuccess = false,
                        message = "Mã phiếu chi " + model.PaymentCode + " đã tồn tại trên hệ thống . Vui lòng kiểm tra lại."
                    });
                if (result < 1)
                    return Ok(new
                    {
                        isSuccess = false,
                        message = "Thêm phiếu chi thất bại",
                        id = -1
                    });
                return Ok(new
                {
                    isSuccess = true,
                    message = "Thêm phiếu chi thành công",
                    id = result
                });
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("AddNewJson - PaymentVoucherController: " + ex);
                return Ok(new
                {
                    isSuccess = false,
                    message = "Thêm phiếu chi thất bại"
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(PaymentVoucherViewModel model, string jsonData)
        {
            try
            {
                var userLogin = 0;
                if (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
                {
                    userLogin = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                }
                //PaymentVoucherViewModel model = JsonConvert.DeserializeObject<PaymentVoucherViewModel>(jsonData);
                //if (imagefile != null)
                //{
                //    string _FileName = Guid.NewGuid() + Path.GetExtension(imagefile.FileName);
                //    string _UploadFolder = @"uploads/images";
                //    string _UploadDirectory = Path.Combine(_WebHostEnvironment.WebRootPath, _UploadFolder);

                //    if (!Directory.Exists(_UploadDirectory))
                //    {
                //        Directory.CreateDirectory(_UploadDirectory);
                //    }

                //    string filePath = Path.Combine(_UploadDirectory, _FileName);

                //    if (!System.IO.File.Exists(filePath))
                //    {
                //        using (var fileStream = new FileStream(filePath, FileMode.Create))
                //        {
                //            await imagefile.CopyToAsync(fileStream);
                //        }
                //    }
                //    model.AttachFiles = "/" + _UploadFolder + "/" + _FileName;
                //}
                model.UpdatedBy = userLogin;
                var validate = Validate(model, out string messages);
                if (validate < 0)
                {
                    return Ok(new
                    {
                        isSuccess = false,
                        message = messages
                    });
                }
                var result = _paymentVoucherRepository.UpdatePaymentVoucher(model);
                if (result < 1)
                    return Ok(new
                    {
                        isSuccess = false,
                        message = "Cập nhật phiếu chi thất bại"
                    });
                return Ok(new
                {
                    isSuccess = true,
                    message = "Cập nhật phiếu chi thành công"
                });
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("Update - PaymentVoucherController: " + ex);
                return Ok(new
                {
                    isSuccess = false,
                    message = "Cập nhật phiếu chi thất bại"
                });
            }
        }

        public IActionResult Detail(int paymentVoucherId)
        {
            var model = _paymentVoucherRepository.GetDetail(paymentVoucherId);
            return View(model);
        }

        [HttpPost]
        public IActionResult ExportExcel(PaymentVoucherViewModel searchModel)
        {
            try
            {
                string _FileName = "PaymentVoucher_" + Guid.NewGuid() + ".xlsx";
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
                if (!string.IsNullOrEmpty(searchModel.PaymentCode))
                    searchModel.PaymentCode = searchModel.PaymentCode.Trim();
                if (!string.IsNullOrEmpty(searchModel.Content))
                    searchModel.Content = searchModel.Content.Trim();

                string FilePath = Path.Combine(_UploadDirectory, _FileName);
                var rsPath = _paymentVoucherRepository.ExportPaymentVouchers(searchModel, FilePath);

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
        public async Task<IActionResult> GetRequestBySupplierId(int supplierId, string text = null, int paymentVoucherId = 0)
        {
            try
            {
                if (supplierId == 0)
                    return Ok(new
                    {
                        isSuccess = true,
                        message = "Thành công",
                        data = new List<PaymentRequestViewModel>()
                    });
                var listOrder = _paymentRequestRepository.GetBySupplierId(supplierId, paymentVoucherId);
                if (!string.IsNullOrEmpty(text))
                    listOrder = listOrder.Where(n => n.PaymentCode.Contains(text.Trim())).ToList();

                return Ok(new
                {
                    isSuccess = true,
                    message = "Thành công",
                    data = listOrder
                });
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetBySupplierId - PaymentVoucherController: " + ex);
                return Ok(new
                {
                    isSuccess = false,
                    message = "Thất bại",
                    data = new List<Entities.Models.Order>()
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetRequestByClientId(int clientId, string text = null, int paymentVoucherId = 0)
        {
            try
            {
                if (clientId == 0)
                    return Ok(new
                    {
                        isSuccess = true,
                        message = "Thành công",
                        data = new List<PaymentRequestViewModel>()
                    });
                var listOrder = _paymentRequestRepository.GetByClientId(clientId, paymentVoucherId);
                return Ok(new
                {
                    isSuccess = true,
                    message = "Thành công",
                    data = listOrder
                });
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetByClientId - PaymentVoucherController: " + ex);
                return Ok(new
                {
                    isSuccess = false,
                    message = "Thất bại",
                    data = new List<Entities.Models.Order>()
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
                        data = new List<PaymentRequestViewModel>()
                    });
                var listFilter = JsonConvert.DeserializeObject<List<PaymentRequestViewModel>>(jsonData);
                if (!string.IsNullOrEmpty(text))
                    listFilter = listFilter.Where(n => n.PaymentCode.Contains(text.Trim())).ToList();
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
                    data = new List<Entities.Models.Order>()
                });
            }
        }

    }
}
