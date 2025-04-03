using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Fast_Food.Models; // Namespace của bạn

public class ThongKeController : Controller
{
    private readonly DoAnStoreContext _context; // DbContext của bạn

    public ThongKeController(DoAnStoreContext context)
    {
        _context = context;
    }

    // 📊 Thống kê doanh thu & số đơn hàng theo ngày
    public async Task<JsonResult> GetRevenueByDay(int month, int year)
    {
        var data = await _context.HoaDons
            .Where(hd => hd.ThoiGianDat.Month == month && hd.ThoiGianDat.Year == year)
            .GroupBy(hd => hd.ThoiGianDat.Day)
            .Select(g => new
            {
                Day = g.Key,
                Total = g.Sum(hd => hd.TongTien ?? 0),
                Orders = g.Count()  // 📦 Số lượng đơn hàng 
            })
            .OrderBy(g => g.Day)
            .ToListAsync();

        return Json(data);
    }

    // 📊 Thống kê doanh thu & số đơn hàng theo tháng
    public async Task<JsonResult> GetRevenueByMonth(int year)
    {
        var data = await _context.HoaDons
            .Where(hd => hd.ThoiGianDat.Year == year)
            .GroupBy(hd => hd.ThoiGianDat.Month)
            .Select(g => new
            {
                Month = g.Key,
                Total = g.Sum(hd => hd.TongTien ?? 0),
                Orders = g.Count()  // 📦 Số lượng đơn hàng
            })
            .OrderBy(g => g.Month)
            .ToListAsync();

        return Json(data);
    }

    // 📊 Thống kê doanh thu & số đơn hàng theo năm
    public async Task<JsonResult> GetRevenueByYear()
    {
        var data = await _context.HoaDons
            .GroupBy(hd => hd.ThoiGianDat.Year)
            .Select(g => new
            {
                Year = g.Key,
                Total = g.Sum(hd => hd.TongTien ?? 0),
                Orders = g.Count()  // 📦 Số lượng đơn hàng
            })
            .OrderBy(g => g.Year)
            .ToListAsync();

        return Json(data);
    }

    public IActionResult Index()
    {
        return View();
    }
}
