using EcoProject.DAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DAL
{
    public class QuanLyKhachHang_DAO
    {
        DataProvider dp = new DataProvider();

        public DataTable lay_du_lieu()
        {
            DataTable dt = new DataTable();
            try
            {
                string query = "select TenCongTy, MaDH, NguoiDaiDien, SDT, DiaChi " +
                            "from KhachHang, DonHang " +
                            "where KhachHang.MaKh = DonHang.MaKH";
                dt = dp.ExecuteQuery(query);
            }
            catch { return null; }
            return dt;
        }

        public DataTable tim_kiem(string ten_ct, string ma_dh, string tinh)
        {
            DataTable dt = new DataTable();
            try
            {
                string query = "select TenCongTy, MaDH, NguoiDaiDien, SDT, DiaChi " +
                               "FROM KhachHang, DonHang " +
                               "WHERE KhachHang.MaKh = DonHang.MaKH AND ";

                ten_ct = Regex.Replace(ten_ct.Trim(), @"\s+", " ");
                ma_dh = Regex.Replace(ma_dh.Trim(), @"\s+", " ");
                ma_dh = Regex.Replace(ma_dh.Trim(), @"\s+", " ");

                List<string> conditions = new List<string>();
                conditions.Add("((TenCongTy LIKE N'%' + @ten_ct + '%' OR KhachHang.MaKH LIKE N'%' + @ten_ct + '%') OR ( @ten_ct = '') )");
                conditions.Add("(MaDH LIKE '%' + @ma_dh + '%' OR @ma_dh = '')");
                conditions.Add("(DiaChi LIKE N'%' + @tinh + '%' OR @tinh = '')");
                
                query += string.Join(" AND ", conditions);
                dt = dp.ExecuteQuery(query, new object[] {ten_ct, ma_dh, tinh});
            }
            catch { return null; }
            return dt;
        }




        public int kiem_tra_don_hang(string ma_dh)
        {
            int result = 0;
            try
            {
                string query = "Select count(*) " +
                                "from DonHang " +
                                "where MaDH = @madh and " +
                                "MaDH IN (SELECT DonHang.MaDH " +
                                            "FROM KhachHang " +
                                            "JOIN DonHang ON KhachHang.MaKH = DonHang.MaKH " +
                                            "JOIN Mau ON DonHang.MaDH = Mau.MaDH " +
                                            "GROUP BY DonHang.MaDH, TenCongTy, NgayTaoDH " +
                                            "HAVING COUNT(CASE WHEN Mau.TrangThai != N'Đã hoàn thành' THEN 1 END) = 0)";
                object r = dp.ExecuteScalar(query, new object[] { ma_dh });
                result = Convert.ToInt32(r);
            }
            catch { return -1; }
            return result;
        }

        public DataTable lay_thong_tin_khach_hang(string ma_dh)
        {
            DataTable dt = new DataTable();
            try
            {
                string query2 = "select KhachHang.* " +
                                    "from KhachHang, DonHang " +
                                    "where KhachHang.MaKH = DonHang.MaKH and MaDH = @madh ";
                dt = dp.ExecuteQuery(query2, new object[] { ma_dh });
            }
            catch { return null; }
            return dt;
        }

        public int them_khach_hang(string nguoi_dd, string ten_ct, 
                                    string email, string dia_chi, string nganh_cn, string sdt, string ghi_chu)
        {
            int result = -1;
            try
            {
                string query = "INSERT INTO khachhang (NguoiDaiDien, TenCongTy, Email, DiaChi, NganhCongNghiep, SDT, GhiChu) " +
                                   "VALUES ( @NguoiDaiDien , @TenCongTy , @Email , @DiaChi , @NganhCongNghiep , @SDT , @GhiChu )";

                DataProvider provider = new DataProvider();

                result = dp.ExecuteNonQuery(query, new object[] { nguoi_dd, ten_ct, email, dia_chi, nganh_cn, sdt, ghi_chu });
            }
            catch { return -1; }
            return result;
        }

        public int sua_thong_tin_khach_hang(string nguoi_dd, string ten_ct,
                                    string email, string dia_chi, string nganh_cn, 
                                    string sdt, string ghi_chu, string ma_kh)
        {
            int result = -1;
            try
            {
                string query = "UPDATE KhachHang " +
                                "SET NguoiDaiDien = @NguoiDaiDien , TenCongTy = @TenCongTy , Email = @Email , " +
                                "DiaChi = @DiaChi , NganhCongNghiep = @NganhCongNghiep , SDT = @SDT , GhiChu = @GhiChu " +
                                "WHERE MaKH = @MaKH";

                DataProvider provider = new DataProvider();

                result = dp.ExecuteNonQuery(query, new object[] { nguoi_dd, ten_ct, email, dia_chi, nganh_cn, sdt, ghi_chu, ma_kh });
            }
            catch { return -1; }
            return result;
        }
    }
}
