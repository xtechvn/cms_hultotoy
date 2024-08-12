using Caching.Elasticsearch;
using Caching.RedisWorker;
using Entities.ConfigModels;
using Entities.Models;
using Entities.ViewModels;
using Entities.ViewModels.Product;
using Entities.ViewModels.Products;
using Entities.ViewModels.Products.V2;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Repositories.IRepositories;
using System.Configuration;
using System.Globalization;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Utilities;
using Utilities.Contants;
using WEB.CMS.Customize;
using WEB.CMS.Models.Product;
using WEB.CMS.Models.Product.V2;
using LogHelper = Utilities.LogHelper;

namespace WEB.CMS.Controllers
{
    [CustomAuthorize]
    public class ProductController : Controller
    {
        private readonly ProductDetailMongoAccess _productV2DetailMongoAccess;

        public ProductController(IConfiguration configuration)
        {
            _productV2DetailMongoAccess = new ProductDetailMongoAccess(configuration);

        }
        public IActionResult Index()
        {
           
            return View();
        }
        public IActionResult Detail(string product_id="")
        {
            ViewBag.ProductId = product_id;
            return View();
        }
        public async Task<IActionResult> ProductListing(string keyword = "", int group_id = -1, int page_index = 1, int page_size = 10)
        {
            try
            {
                if (page_size <= 0) page_size = 10;
                if (page_index < 1) page_index = 1;
                return Ok(new
                {
                    is_success = true,
                    data = await _productV2DetailMongoAccess.Listing(keyword, group_id, page_index, page_size)
                });

            }
            catch
            {

            }
            return Ok(new
            {
                is_success = false
            });
        }
        public async Task<IActionResult> ProductSubListing(List<string> main_products)
        {
            try
            {
                List<ProductMongoDbModel> sub_product = new List<ProductMongoDbModel>();
                if (main_products != null && main_products.Count > 0)
                {
                    sub_product = await _productV2DetailMongoAccess.SubListing(main_products);
                }
                return Ok(new
                {
                    is_success = true,
                    data = sub_product
                });

            }
            catch
            {

            }
            return Ok(new
            {
                is_success = false
            });
        } 
        public async Task<IActionResult> ProductDetail(string product_id)
        {
            try
            {
                return Ok(new
                {
                    is_success = true,
                    data = await _productV2DetailMongoAccess.GetByID(product_id)
                });
            }
            catch
            {

            }
            return Ok(new
            {
                is_success = false
            });
        }
    }
  
}