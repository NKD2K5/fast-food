using NUnit.Framework;
using System;

namespace UnitTestProject
{
    [TestFixture]
    public class SinhVienServiceTests
    {
        private SinhVienService sinhVienService;

        [SetUp]
        public void Setup()
        {
            sinhVienService = new SinhVienService();
            sinhVienService.ThemSinhVien(new SinhVien("SV001", "Nguyen Van A", 20, 7.5f, 2, "CNTT"));
        }

        [Test]
        public void Test_SuaSinhVien_ThanhCong()
        {
            var svMoi = new SinhVien("SV001", "Le Thi B", 21, 8.2f, 3, "Kinh te");
            sinhVienService.SuaSinhVien("SV001", svMoi);
            Assert.Pass("Sua sinh vien thanh cong!");
        }
    
        [Test]
        public void Test_SuaSinhVien_MaSVKhongTonTai()
        {
            var svMoi = new SinhVien("SV999", "Le Thi C", 22, 7.5f, 3, "Marketing");
            Assert.Throws<KeyNotFoundException>(() => sinhVienService.SuaSinhVien("SV999", svMoi));
        }

        [Test]
        public void Test_SuaSinhVien_MaSVRong()
        {
            var svMoi = new SinhVien("", "Le Thi D", 22, 7.5f, 3, "Quan tri");
            Assert.Throws<ArgumentException>(() => sinhVienService.SuaSinhVien("", svMoi));
        }

        [Test]
        public void Test_SuaSinhVien_ThongTinSinhVienKhongHopLe()
        {
            var svMoi = new SinhVien("SV001", "", 21, -1, 3, "");
            Assert.Throws<ArgumentException>(() => sinhVienService.SuaSinhVien("SV001", svMoi));
        }
    }
}
