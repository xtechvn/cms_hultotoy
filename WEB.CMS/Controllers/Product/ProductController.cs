using Entities.ViewModels.Products;
using Microsoft.AspNetCore.Mvc;
using WEB.CMS.Customize;
using WEB.CMS.Models.Product;

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