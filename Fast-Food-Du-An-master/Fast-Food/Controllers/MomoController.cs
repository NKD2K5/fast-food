using Fast_Food.Momo;
using Microsoft.AspNetCore.Mvc;

namespace Fast_Food.Controllers
{
    public class MomoController : Controller
    {
        private IMomoService _momoService;
        //private readonly IVnPayService _vnPayService;
        public MomoController(IMomoService momoService)
        {
            _momoService = momoService;
        }
        //[HttpPost]
        //[Route("CreatePaymentUrl")]
        //public async Task<IActionResult> CreatePaymentUrl(OrderInfoModel model)
        //{
        //    var response = await _momoService.CreatePaymentAsync(model);
        //    return Redirect(response.PayUrl);
        //}
        [Route("Momo/CreatePaymentUrl")]
        public async Task<IActionResult> CreatePaymentUrl([FromServices] IMomoService momoService)
        {
            var order = new OrderInfoModel
            {
                FullName = "Nguyễn Văn A",
                Amount = 100000,
                OrderInfo = "Thanh toán đơn hàng"
            };

            var response = await momoService.CreatePaymentAsync(order);
            return Redirect(response.PayUrl);
        }

        [HttpGet]
        public IActionResult Pa()
        {
            var re = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);
            return View(re);
        }
    }
}
