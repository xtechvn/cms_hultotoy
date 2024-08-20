using Entities.ViewModels.OrderManual;
using Microsoft.AspNetCore.Mvc;
using Repositories.IRepositories;
using System.Security.Claims;
using Utilities;
using Utilities.Contants;
using WEB.Adavigo.CMS.Service;
using WEB.CMS.Customize;

namespace WEB.CMS.Controllers.Order
{
    public class OrderManualController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IAllCodeRepository _allCodeRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IIdentifierServiceRepository _identifierServiceRepository;
        private readonly IAccountClientRepository _accountClientRepository;

        public OrderManualController(IConfiguration configuration, IAllCodeRepository allCodeRepository, IOrderRepository orderRepository, IIdentifierServiceRepository identifierServiceRepository, IAccountClientRepository accountClientRepository)
        {
            _configuration = configuration;
            _allCodeRepository = allCodeRepository;
            _orderRepository = orderRepository;
            _identifierServiceRepository = identifierServiceRepository;
            _accountClientRepository = accountClientRepository;
        }
        [HttpPost]
        public IActionResult CreateOrderManual()
        {
            ViewBag.Branch = _allCodeRepository.GetListByType(AllCodeType.BRANCH_CODE);
            return View();
        }

      
    }
}
