﻿using Entities.Models;
using Entities.ViewModels.Products;
using HuloToys_Service.ElasticSearch.NewEs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Nest;
using Newtonsoft.Json;
using Utilities.Contants.ProductV2;
using WEB.Adavigo.CMS.Service;
using WEB.CMS.Customize;
using WEB.CMS.Models;
using WEB.CMS.Models.Product;

namespace WEB.CMS.Controllers
{
    [CustomAuthorize]
    public class ProductController : Controller
    {
        private readonly ProductDetailMongoAccess _productV2DetailMongoAccess;
        private readonly ProductSpecificationMongoAccess _productSpecificationMongoAccess;
        private readonly GroupProductESService _groupProductESService;
        private  StaticAPIService _staticAPIService;
        private readonly int group_product_root = 1;
        public ProductController(IConfiguration configuration)
        {
            _productV2DetailMongoAccess = new ProductDetailMongoAccess(configuration);
            _groupProductESService = new GroupProductESService(configuration["DataBaseConfig:Elastic:Host"], configuration);
            _productSpecificationMongoAccess = new ProductSpecificationMongoAccess( configuration);
            _staticAPIService = new StaticAPIService( configuration);
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
                var main_products = await _productV2DetailMongoAccess.Listing(keyword, group_id, page_index, page_size);
                return Ok(new
                {
                    is_success = true,
                    data = main_products,
                    subdata = await _productV2DetailMongoAccess.SubListing(main_products.Select(x=>x._id))
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
        public async Task<IActionResult> ProductSubListing(string parent_id)
        {
            try
            {
                return Ok(new
                {
                    is_success = true,
                    data = await _productV2DetailMongoAccess.SubListing(parent_id)
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
        public async Task<IActionResult> Summit(ProductMongoDbSummitModel request)
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
                string rs = "";
                var uploaded_image = new List<string>();
                //-- IMg
                if (request.images != null && request.images.Count > 0)
                {
                    foreach (var img in request.images)
                    {
                        if(img!=null && img.Trim() != "")
                        {
                            var data_img = _staticAPIService.GetImageSrcBase64Object(img);
                            if (data_img != null)
                            {
                                var url = await _staticAPIService.UploadImageBase64(data_img);
                                if (url != null && url.Trim() != "")
                                {
                                    uploaded_image.Add(url);
                                    continue;
                                }
                            }
                            uploaded_image.Add(img);

                        }
                       
                    }
                }
                request.images = uploaded_image;
                //-- Avt:
                if (request.avatar != null && request.avatar.Trim() != "")
                {
                    if (request.avatar != null && request.avatar.Trim() != "" && request.avatar.Contains("data:image") && request.avatar.Contains("base64"))
                    {
                        var data_img = _staticAPIService.GetImageSrcBase64Object(request.avatar);
                        if (data_img != null)
                        {
                            request.avatar = await _staticAPIService.UploadImageBase64(data_img);
                        }

                    }
                }
                //-- Attributes Img:
                if (request.attributes_detail != null && request.attributes_detail.Count > 0)
                {
                    foreach (var attributes_detail in request.attributes_detail)
                    {
                        if (attributes_detail.img!=null && attributes_detail.img.Trim()!="" && attributes_detail.img.Contains("data:image") && attributes_detail.img.Contains("base64"))
                        {
                            var data_img = _staticAPIService.GetImageSrcBase64Object(attributes_detail.img);
                            if (data_img != null)
                            {
                                attributes_detail.img = await _staticAPIService.UploadImageBase64(data_img);
                            }

                        }
                    }
                }
               
                //-- Add/Update product_main
                var product_main = JsonConvert.DeserializeObject<ProductMongoDbModel>(JsonConvert.SerializeObject(request));
                //-- Add / Update Sub product
                if (request.variations != null && request.variations.Count > 0)
                {
                    product_main.status = (int)ProductStatus.ACTIVE;
                    var amount_variations = request.variations.Select(x => x.amount);
                    product_main.amount_max = amount_variations.OrderByDescending(x => x).First();
                    product_main.amount_min = amount_variations.OrderBy(x => x).First();
                    product_main.quanity_of_stock = request.variations.Sum(x => x.quanity_of_stock);
                }
                product_main.parent_product_id = "";
                if (product_main._id==null || product_main._id.Trim() == "")
                {
                    product_main.status = (int)ProductStatus.ACTIVE;
                    rs = await _productV2DetailMongoAccess.AddNewAsync(product_main);
                }
                else
                {
                    rs = await _productV2DetailMongoAccess.UpdateAsync(product_main);
                    await _productV2DetailMongoAccess.DeactiveByParentId(product_main._id);
                    
                    
                }
                //-- Add / Update Sub product
                if (request.variations != null && request.variations.Count > 0)
                {
                    foreach (var variation in request.variations)
                    {
                        var product_by_variations = JsonConvert.DeserializeObject<ProductMongoDbModel>(JsonConvert.SerializeObject(request));
                        //product_by_variations._id = variation._id;
                        product_by_variations.variation_detail = variation.variation_attributes;
                        product_by_variations.status = (int)ProductStatus.ACTIVE;
                        product_by_variations.parent_product_id = product_main._id;
                        product_by_variations.price = variation.price;
                        product_by_variations.profit = variation.profit;
                        product_by_variations.amount = variation.amount;
                        product_by_variations.quanity_of_stock = variation.quanity_of_stock;
                        product_by_variations.sku = variation.sku;
                        await _productV2DetailMongoAccess.AddNewAsync(product_by_variations);
                    }
                }
                if (rs != null)
                {
                    return Ok(new
                    {
                        is_success = true,
                        msg = "Thêm mới / Cập nhật sản phẩm thành công",
                        data = rs
                    });
                }

            }
            catch (Exception ex)
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