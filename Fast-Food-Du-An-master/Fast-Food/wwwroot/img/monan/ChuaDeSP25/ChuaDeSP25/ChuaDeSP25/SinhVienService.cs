using System;
using System.Collections.Generic;

public class SinhVienService
{
    private Dictionary<string, SinhVien> danhSachSinhVien = new Dictionary<string, SinhVien>();

    public void ThemSinhVien(SinhVien sv)
    {
        if (sv == null || string.IsNullOrWhiteSpace(sv.MaSV))
        {
            throw new ArgumentException("Sinh vien khong hop le!");
        }
        danhSachSinhVien[sv.MaSV] = sv;
    }

    public void SuaSinhVien(string maSV, SinhVien svMoi)
    {
        if (string.IsNullOrWhiteSpace(maSV) || svMoi == null)
        {
            throw new ArgumentException("Thong tin sua khong hop le!");
        }
        if (!danhSachSinhVien.ContainsKey(maSV))
        {
            throw new KeyNotFoundException("Khong tim thay sinh vien!");
        }
        if (string.IsNullOrWhiteSpace(svMoi.Ten) || svMoi.Tuoi <= 0 || svMoi.DiemTrungBinh < 0 || string.IsNullOrWhiteSpace(svMoi.ChuyenNganh))
        {
            throw new ArgumentException("Thong tin sinh vien khong hop le!");
        }

        danhSachSinhVien[maSV] = svMoi;
    }
}
