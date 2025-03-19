using Fast_Food.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using RestSharp;

namespace Fast_Food.Momo
{
    public class MomoService : IMomoService
    {
        private readonly IOptions<MomoOptionModel> _options;

        public MomoService(IOptions<MomoOptionModel> options)
        {
            _options = options;
        }

        public async Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(OrderInfoModel model)
        {
            if (model == null || model.Amount <= 0)
            {
                throw new ArgumentException("Dữ liệu không hợp lệ để tạo thanh toán.");
            }

            model.OrderId = DateTime.UtcNow.Ticks.ToString();
            model.OrderInfo = $"Khách hàng: {model.FullName}. Nội dung: {model.OrderInfo}";

            var rawData = $"partnerCode={_options.Value.PartnerCode}" +
                          $"&accessKey={_options.Value.AccessKey}" +
                          $"&requestId={model.OrderId}" +
                          $"&amount={model.Amount.ToString("F0")}" + // ✅ Chuyển đổi decimal sang string
                          $"&orderId={model.OrderId}" +
                          $"&orderInfo={model.OrderInfo}" +
                          $"&returnUrl={_options.Value.ReturnUrl}" +
                          $"&notifyUrl={_options.Value.NotifyUrl}" +
                          $"&extraData=";

            var signature = ComputeHmacSha256(rawData, _options.Value.SecretKey);

            var client = new RestClient(_options.Value.MomoApiUrl);
            var request = new RestRequest() { Method = Method.Post };
            request.AddHeader("Content-Type", "application/json; charset=UTF-8");

            var requestData = new
            {
                accessKey = _options.Value.AccessKey,
                partnerCode = _options.Value.PartnerCode,
                requestType = _options.Value.RequestType,
                notifyUrl = _options.Value.NotifyUrl,
                returnUrl = _options.Value.ReturnUrl,
                orderId = model.OrderId,
                amount = model.Amount.ToString("F0"), // ✅ Sửa lỗi decimal -> string
                orderInfo = model.OrderInfo,
                requestId = model.OrderId,
                extraData = "",
                signature = signature
            };

            request.AddJsonBody(requestData);

            var response = await client.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Lỗi MoMo API: {response.StatusCode} - {response.Content}");
            }

            return JsonConvert.DeserializeObject<MomoCreatePaymentResponseModel>(response.Content);
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
