namespace Fast_Food.Momo
{
    public class OrderInfoModel
    {
        public string FullName { get; set; }
        public string OrderId { get; set; }
        public string OrderInfo { get; set; }
        public decimal Amount { get; set; } // Đổi từ string sang decimal
    }
}