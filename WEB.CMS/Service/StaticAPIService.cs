using HuloToys_Front_End.Controllers.Files.Bussiness;
using Newtonsoft.Json;
using Repositories.IRepositories;
using System.Linq;
using System.Text;
using Utilities;
using Utilities.ViewModels.Article;
using WEB.CMS.Models;

namespace WEB.Adavigo.CMS.Service
{
    public class StaticAPIService
    {
        private string API_IMAGE = "https://static-image.adavigo.com/images/upload";
        private string API_VIDEO = "https://static-image.adavigo.com/Video/upload-video";
        private string STATIC_URL = "https://static-image.adavigo.com";
        private string KEY = "wVALy5t0tXEgId5yMDNg06OwqpElC9I0sxTtri4JAlXluGipo6kKhv2LoeGQnfnyQlC07veTxb7zVqDVKwLXzS7Ngjh1V3SxWz69";
        private readonly IConfiguration _configuration;
        public StaticAPIService(IConfiguration configuration)
        {
            KEY = configuration["API:UploadImageKey"];
            API_IMAGE = configuration["API:UploadImage"];
            API_VIDEO = configuration["API:UploadVideo"];
            STATIC_URL = configuration["API:StaticURL"];
            _configuration=configuration;
        }
        public async Task<string> ConvertOtherImageURLToStaticURL(string url)
        {

            try
            {
                if (url.Contains(_configuration["DomainConfig:ImageStatic"]))
                {
                    return url;
                }
                using HttpClient client = new HttpClient();
                string base64image = "";
                try
                {
                    // Download the image data
                    byte[] imageBytes = await client.GetByteArrayAsync(url);

                    // Convert the image data to Base64 string
                    base64image= Convert.ToBase64String(imageBytes);
                }
                catch (Exception ex)
                {
                    return url;
                }
                var split_url = url.Split(".");
                var j_param = new Dictionary<string, string> {
                    { "data_file", base64image },
                    { "extend", split_url[split_url.Length-1] }};
                string tokenData = CommonHelper.Encode(JsonConvert.SerializeObject(j_param), KEY);
                using (HttpClient httpClient = new HttpClient())
                {
                    var contentObj = new { token = tokenData };
                    var content = new StringContent(JsonConvert.SerializeObject(contentObj), Encoding.UTF8, "application/json");
                    var result = await httpClient.PostAsync(API_IMAGE, content);
                    dynamic resultContent = Newtonsoft.Json.Linq.JObject.Parse(result.Content.ReadAsStringAsync().Result);
                    if (resultContent.status == 0)
                    {
                        return resultContent.url_path;
                    }
                    else
                    {
                        LogHelper.InsertLogTelegram("UploadImageBase64. Result: " + resultContent.status + ". Message: " + resultContent.msg);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UploadImageBase64 - " + ex.Message.ToString());
            }
            return url;
        }
        public async Task<string> UploadImageBase64(ImageBase64 modelImage)
        {
            try
            {
                var j_param = new Dictionary<string, string> {
                    { "data_file", modelImage.ImageData },
                    { "extend", modelImage.ImageExtension }};
                string tokenData = CommonHelper.Encode(JsonConvert.SerializeObject(j_param), KEY);
                using (HttpClient httpClient = new HttpClient())
                {
                    var contentObj = new { token = tokenData };
                    var content = new StringContent(JsonConvert.SerializeObject(contentObj), Encoding.UTF8, "application/json");
                    var result = await httpClient.PostAsync(API_IMAGE, content);
                    dynamic resultContent = Newtonsoft.Json.Linq.JObject.Parse(result.Content.ReadAsStringAsync().Result);
                    if (resultContent.status == 0)
                    {
                        return resultContent.url_path;
                    }
                    else
                    {
                        LogHelper.InsertLogTelegram("UploadImageBase64. Result: " + resultContent.status + ". Message: " + resultContent.msg);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UploadImageBase64 - " + ex.Message.ToString());
            }
            return null;
        }
        public async Task<string> UploadVideoBase64(ImageBase64 modelVideo)
        {
            try
            {
                var j_param = new Dictionary<string, string> {
                    { "data_file", modelVideo.ImageData },
                    { "extend", modelVideo.ImageExtension }};
                string tokenData = CommonHelper.Encode(JsonConvert.SerializeObject(j_param), KEY);
                using (HttpClient httpClient = new HttpClient())
                {
                    var contentObj = new { token = tokenData };
                    var content = new StringContent(JsonConvert.SerializeObject(contentObj), Encoding.UTF8, "application/json");
                    var result = await httpClient.PostAsync(API_VIDEO, content);
                    dynamic resultContent = Newtonsoft.Json.Linq.JObject.Parse(result.Content.ReadAsStringAsync().Result);
                    if (resultContent.status == "success")
                    {
                        return resultContent.url_path;
                    }
                    else
                    {
                        LogHelper.InsertLogTelegram("UploadImageBase64. Result: " + resultContent.status + ". Message: " + resultContent.msg);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UploadImageBase64 - " + ex.Message.ToString());
            }
            return null;
        }
        public ImageBase64 GetImageSrcBase64Object(string imgSrc)
        {
            try
            {
                if (!string.IsNullOrEmpty(imgSrc) && imgSrc.StartsWith("data:image"))
                {
                    var ImageBase64 = new ImageBase64();
                    var base64Data = imgSrc.Split(',')[0];
                    ImageBase64.ImageData = imgSrc.Split(',')[1];
                    ImageBase64.ImageExtension = base64Data.Split(';')[0].Split('/')[1];
                    return ImageBase64;
                }
            }
            catch (FormatException)
            {

            }
            return null;
        }
        public ImageBase64 GetVideoSrcBase64Object(string imgSrc)
        {
            try
            {
                if (!string.IsNullOrEmpty(imgSrc) && imgSrc.StartsWith("data:video"))
                {
                    var ImageBase64 = new ImageBase64();
                    var base64Data = imgSrc.Split(',')[0];
                    ImageBase64.ImageData = imgSrc.Split(',')[1];
                    ImageBase64.ImageExtension = base64Data.Split(';')[0].Split('/')[1];
                    return ImageBase64;
                }
            }
            catch (FormatException)
            {

            }
            return null;
        }
    }
}
