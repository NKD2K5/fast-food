using Fast_Food.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace Fast_Food.Momo
{
    public class MomoService : IMomoService
    {
        private readonly IOptions<MomoOptionModel> _options;
        private readonly HttpClient _httpClient;

        public MomoService(IOptions<MomoOptionModel> options, HttpClient httpClient)
        {
            _options = options;
            _httpClient = httpClient;
        }

        public async Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(OrderInfoModel model)
        {
            if (model == null || model.Amount <= 0)
            {
                throw new ArgumentException("Dữ liệu không hợp lệ để tạo thanh toán.");
            }

            // Tạo OrderId và OrderInfo
            model.OrderId = DateTime.UtcNow.Ticks.ToString();
            model.OrderInfo = $"Khách hàng: {model.FullName}. Nội dung: {model.OrderInfo}";

            // Chuẩn bị dữ liệu raw để ký HMAC SHA256
            var rawData = $"partnerCode={_options.Value.PartnerCode}" +
                          $"&accessKey={_options.Value.AccessKey}" +
                          $"&requestId={model.OrderId}" +
                          $"&amount={model.Amount.ToString("F0")}" +
                          $"&orderId={model.OrderId}" +
                          $"&orderInfo={model.OrderInfo}" +
                          $"&returnUrl={_options.Value.ReturnUrl}" +
                          $"&notifyUrl={_options.Value.NotifyUrl}" +
                          $"&extraData=";

            var signature = ComputeHmacSha256(rawData, _options.Value.SecretKey);

            // Tạo dữ liệu JSON để gửi lên MoMo
            var requestData = new
            {
                partnerCode = _options.Value.PartnerCode,
                accessKey = _options.Value.AccessKey,
                requestId = model.OrderId,
                amount = model.Amount.ToString("F0"),
                orderId = model.OrderId,
                orderInfo = model.OrderInfo,
                returnUrl = _options.Value.ReturnUrl,
                notifyUrl = _options.Value.NotifyUrl,
                requestType = _options.Value.RequestType,
                extraData = "",
                signature = signature
            };

            var requestContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

            // Gửi yêu cầu đến API MoMo
            var response = await _httpClient.PostAsync(_options.Value.MomoApiUrl, requestContent);
            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Lỗi MoMo API: {response.StatusCode} - {responseString}");
            }

            var responseModel = JsonConvert.DeserializeObject<MomoCreatePaymentResponseModel>(responseString);
            if (responseModel == null)
            {
                throw new Exception("Lỗi khi giải mã phản hồi từ MoMo.");
            }

            return responseModel;
        }

        public async Task<MomoExecuteResponseModel> PaymentExecuteAsync(IQueryCollection collection)
        {
            var amount = collection.ContainsKey("amount") ? collection["amount"].ToString() : "0";
            var orderInfo = collection.ContainsKey("orderInfo") ? collection["orderInfo"].ToString() : "No Info";
            var orderId = collection.ContainsKey("orderId") ? collection["orderId"].ToString() : "No Order";

            return await Task.FromResult(new MomoExecuteResponseModel()
            {
                Amount = amount,
                OrderId = orderId,
                OrderInfo = orderInfo
            });
        }

        private string ComputeHmacSha256(string message, string secretKey)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey));
            return BitConverter.ToString(hmac.ComputeHash(Encoding.UTF8.GetBytes(message)))
                .Replace("-", "")
                .ToLower();
        }
    }
}
