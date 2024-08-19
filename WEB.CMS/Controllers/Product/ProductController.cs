using Entities.Models;
using Entities.ViewModels.Products;
using HuloToys_Service.ElasticSearch.NewEs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Nest;
using WEB.CMS.Customize;
using WEB.CMS.Models.Product;

namespace WEB.CMS.Controllers
{
    [CustomAuthorize]
    public class ProductController : Controller
    {
        private readonly ProductDetailMongoAccess _productV2DetailMongoAccess;
        private readonly ProductSpecificationMongoAccess _productSpecificationMongoAccess;
        private readonly GroupProductESService _groupProductESService;
        private readonly int group_product_root = 1;
        public ProductController(IConfiguration configuration)
        {
            _productV2DetailMongoAccess = new ProductDetailMongoAccess(configuration);
            _groupProductESService = new GroupProductESService(configuration["DataBaseConfig:Elastic:Host"], configuration);
            _productSpecificationMongoAccess = new ProductSpecificationMongoAccess( configuration);
        }
        public IActionResult Index()
        {
           
            return View();
        } 
        
        public IActionResult Detail(string id = "")
        {
            ViewBag.ProductId = id;
            return View();
        }
        public async Task<IActionResult> GroupProduct(int group_id = 1, int position = 0)
        {
            try
            {
                if(group_id>0)
                return Ok(new
                {
                    is_success = true,
                    data =  _groupProductESService.GetListGroupProductByParentId(group_id),
                    position = position

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
        public async Task<IActionResult> ProductDetail(string product_id)
        {
            try
            {
                return Ok(new
                {
                    is_success = true,
                    data = await _productV2DetailMongoAccess.GetByID(product_id),
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
        public async Task<IActionResult> Summit(ProductMongoDbModel request)
        {
            try
            {
                if(
                    request.name==null || request.name.Trim()==""
                    || request.images == null || request.images.Count<=0
                    || request.avatar == null || request.avatar.Trim() == ""
                    || request.group_product_id == null || request.group_product_id.Trim() == ""
                    )
                {
                    return Ok(new
                    {
                        is_success = false,
                        msg = "Dữ liệu sản phẩm không chính xác, vui lòng chỉnh sửa và thử lại",
                    });
                }
                request.status=0;
                if(request._id==null || request._id.Trim() == "")
                {
                    var rs=await _productV2DetailMongoAccess.AddNewAsync(request);
                    if (rs != null)
                    {
                        return Ok(new
                        {
                            is_success = true,
                            msg="Thêm mới sản phẩm thành công",
                            data=rs
                        });
                    }
                }
                else
                {
                    var rs = await _productV2DetailMongoAccess.UpdateAsync(request);
                    if (rs != null)
                    {
                        return Ok(new
                        {
                            is_success = true,
                            msg = "Cập nhật sản phẩm thành công",
                            data = rs
                        });
                    }
                }
               

            }
            catch
            {

            }
            return Ok(new
            {
                is_success = false,
                msg = "Thêm mới / Cập nhật sản phẩm thất bại, vui lòng liên hệ bộ phận IT",
            });
        }
        public async Task<IActionResult> ProductDetailGroupProducts(string ids)
        {
            try
            {
                List<GroupProduct> groups = new List<GroupProduct>();
                foreach(var id in ids.Split(","))
                {
                    if (id != null && id.Trim() != "")
                    {
                        try
                        {
                            groups.Add(_groupProductESService.GetDetailGroupProductById(Convert.ToInt32(id)));
                        }
                        catch { }
                    }
                }
                return Ok(new
                {
                    is_success = true,
                    data= groups,
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
        public async Task<IActionResult> AddProductSpecification(int type, string name)
        {
            try
            {
                if (name == null || name.Trim() == "")
                {
                   
                }
                else
                {
                    var exists = await _productSpecificationMongoAccess.GetByNameAndType(type, name);
                    if (exists == null || exists._id == null)
                    {
                        var id = await _productSpecificationMongoAccess.AddNewAsync(new ProductSpecificationMongoDbModel()
                        {
                            attribute_name = name,
                            attribute_type = type,

                        });
                        return Ok(new
                        {
                            is_success = true,
                            data = id
                        });
                    }
                }
               
            }
            catch
            {

            }
            return Ok(new
            {
                is_success = false
            });
        }
        public async Task<IActionResult> GetSpecificationByName(int type, string name)
        {
            try
            {
                if (type<=0)
                {

                }
                else
                {
                    var exists = await _productSpecificationMongoAccess.Listing(type, name);
                    return Ok(new
                    {
                        is_success = true,
                        data = exists
                    });
                }

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