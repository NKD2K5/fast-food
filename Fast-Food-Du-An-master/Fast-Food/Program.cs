using Fast_Food.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Fast_Food.Momo;

namespace Fast_Food
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.Configure<MomoOptionModel>(builder.Configuration.GetSection("MomoOptions"));
            builder.Services.AddHttpClient<IMomoService, MomoService>();


            // Đăng ký DbContext với chuỗi kết nối
            builder.Services.AddDbContext<DoAnStoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            });

            // Đăng ký HttpContextAccessor
            builder.Services.AddHttpContextAccessor();

            // Đăng ký Session
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(2); // Thời gian hết hạn session
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Cấu hình pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
