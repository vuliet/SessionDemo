using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace SessionDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {

        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetSession")]
        public IActionResult Get()
        {
            CountAccess(HttpContext);

            var sessionData = CountAccessInfo(HttpContext);

            return Ok(new { data = sessionData});
        }

        private void CountAccess(HttpContext context)
        {
            // Lấy ISession
            var session = context.Session;
            string key_access = "info_access";

            // Lưu vào  Session thông tin truy cập
            // Định nghĩa cấu trúc dữ liệu lưu trong Session
            var accessInfoType = new
            {
                count = 0,
                lasttime = DateTime.Now
            };

            // Đọc chuỗi lưu trong Sessin với key = info_access
            string json = session.GetString(key_access);
            dynamic lastAccessInfo;
            if (json != null)
            {
                // Convert chuỗi Json - thành đối tượng có cấu trúc như accessInfoType
                lastAccessInfo = JsonConvert.DeserializeObject(json, accessInfoType.GetType());
            }
            else
            {
                // json chưa từng lưu trong Session, accessInfo lấy bằng giá trị khởi  tạo
                lastAccessInfo = accessInfoType;
            }

            // Cập nhật thông tin
            var accessInfoSave = new
            {
                count = lastAccessInfo.count + 1,
                lasttime = DateTime.Now
            };

            // Convert accessInfo thành chuỗi Json và lưu lại vào Session
            string jsonSave = JsonConvert.SerializeObject(accessInfoSave);
            session.SetString(key_access, jsonSave);
            Console.WriteLine(jsonSave);
        }

        private static string CountAccessInfo(HttpContext context)
        {
            var session = context.Session;          // Lấy ISession
            string key_access = "info_access";

            // Lưu vào  Session thông tin truy cập
            // Định nghĩa cấu trúc dữ liệu lưu trong Session
            var accessInfoType = new
            {
                count = 0,
                lasttime = DateTime.Now
            };

            // Đọc chuỗi lưu trong Sessin với key = info_access
            string json = session.GetString(key_access);
            dynamic lastAccessInfo;
            if (json != null)
            {
                // Convert chuỗi Json - thành đối tượng
                lastAccessInfo = JsonConvert.DeserializeObject(json, accessInfoType.GetType());
            }
            else
            {
                // json chưa từng lưu trong Session, accessInfo lấy bằng giá trị khởi  tạo
                lastAccessInfo = accessInfoType;
            }
            if (lastAccessInfo.count == 0)
            {
                return "Chưa truy cập /Product lần  nào";
            }

            string thongtin = $"Số lần truy cập /Product: {lastAccessInfo.count}  - lần cuối: {lastAccessInfo.lasttime.ToLongTimeString()}";
            return thongtin;
        }
    }
}