using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EcoProject.DAO;

namespace DAL
{
    public class QuanLyDonHang_DAO
    {
        DataProvider dp;

        public QuanLyDonHang_DAO()
        {
            dp = new DataProvider();
        }

        public DataTable lay_du_lieu()
        {
            try
            {
                string query = "Select MaDH , TenCongTy, NgayTaoDH, HanTraHang from DonHang, KhachHang " +
                                "where DonHang.MaKH = KhachHang.MaKH and DonHang.NgayXuatPhieuTraHang is NULL"; 
                return this.dp.ExecuteQuery(query);
            }
            catch { return null; }
        }

        public DataTable tim_kiem(string madh, string ten)
        {
            try
            {
                string query = @"SELECT MaDH, TenCongTy, NgayTaoDH, HanTraHang FROM DonHang, KhachHang 
                                WHERE DonHang.MaKH = KhachHang.MaKH AND ( @madh = '' OR MaDH LIKE '%' + @madh + '%') 
                                AND ( @ten = '' OR TenCongTy COLLATE Latin1_General_CI_AI LIKE '%' + @ten + '%') and DonHang.NgayXuatPhieuTraHang is NULL";
                return this.dp.ExecuteQuery(query, new object[] { Regex.Replace(madh.Trim(), @"\s+", " "), Regex.Replace(ten.Trim(), @"\s+", " ") });
            }
            catch { return null; }
        }

        public string lay_quy(string madh)
        {
            try
            {
                string query = "select Quy from DonHang where DonHang.MaDH = @madh";
                return dp.ExecuteScalar(query, new object[] { madh }).ToString();
            }
            catch { return null; }
        }

        public Boolean xoa_don_hang(string madh)
        {
            try
            {
                dp.ExecuteNonQuery("delete from DonHang where DonHang.MaDH = @madh", new object[] { madh });
                return true;
            }
            catch { return false; }
        }

        public DataTable cong_ty_da_xu_ly()
        {
            try
            {
                string query = "select KhachHang.MaKH, TenCongTy, NgayXuatPhieuTraHang from KhachHang, DonHang " +
                               "where (select COUNT(*) from Mau where Mau.MaDH = DonHang.MaDH) = (select COUNT(*) from Mau " +
                               "where Mau.MaDH = DonHang.MaDH and Mau.TrangThai = N'Đã hoàn thành') and KhachHang.MaKH = DonHang.MaKH and DonHang.NgayXuatPhieuTraHang is not null";
                return this.dp.ExecuteQuery(query);
            }
            catch { return null; }
        }

        public DataTable tim_kiem_cong_ty(string chuoi_tim_kiem)
        {
            try 
            {
                string query = "SELECT * " +
                               "FROM ( " +
                                    "SELECT KhachHang.MaKH, TenCongTy, NgayXuatPhieuTraHang " +
                                    "FROM KhachHang, DonHang " +
                                    "WHERE (SELECT COUNT(*) " +
                                            "FROM Mau " +
                                            "WHERE Mau.MaDH = DonHang.MaDH) = " +
                                                "(SELECT COUNT(*) FROM Mau " +
                                                "WHERE Mau.MaDH = DonHang.MaDH AND Mau.TrangThai = N'Đã hoàn thành') " +
                                                "AND KhachHang.MaKH = DonHang.MaKH AND DonHang.NgayXuatPhieuTraHang IS NOT NULL) AS Result WHERE MaKH LIKE '%' + @makh + '%' or TenCongTy COLLATE Latin1_General_CI_AI LIKE '%' + @makh + '%'";
                return this.dp.ExecuteQuery(query, new object[] { Regex.Replace(chuoi_tim_kiem.Trim(), @"\s+", " ") });
            }
            catch {return null;}
        }

        public string kiem_tra_makh(string makh)
        {
            try
            {
                return (string)dp.ExecuteScalar("select MaKH from KhachHang where TenCongTy = @tencongty", new object[] { makh });
            }
            catch { return null; }
        }

        public int them_don_hang(DateTime traketqua, DateTime ngaydathang, string makh, string quy)
        {
            try
            {
                string query_them_DonHang = "insert into DonHang (HanTraHang, NgayTaoDH, MaKH, Quy) values( @ngayketthuc , @ngayky , @makh , @quy )";
                int result = dp.ExecuteNonQuery(query_them_DonHang, new object[]
                {
                    traketqua,
                    ngaydathang,
                    makh,
                    quy
                });
                return result;
            }
            catch { return 0;}
        }

        public Boolean cap_nhat_don_hang(DateTime traketqua, DateTime ngaydathang, string makh, string quy, string madh)
        {
            try
            {
                string query_cap_nhat = "update DonHang set HanTraHang = @hantra , NgayTaoDH = @ngayky , MaKH = @mkh , Quy = @quy where DonHang.MaDH = @madh";
                this.dp.ExecuteNonQuery(query_cap_nhat, new object[] {traketqua, ngaydathang, makh, quy, madh});
                return true;
            }
            catch { return false;}
           
        }
    }
}
