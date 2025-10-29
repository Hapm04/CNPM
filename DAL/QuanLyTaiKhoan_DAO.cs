using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcoProject.DAO;
namespace DAL
{
    public class QuanLyTaiKhoan_DAO
    {
        DataProvider dp;
        public QuanLyTaiKhoan_DAO() { dp=new DataProvider(); }

        public int check_tk_mk(string TK_DangNhap, string MK_DangNhap)
        {
            string query = "SELECT COUNT(*) FROM NhanVien WHERE Email = @MaNV AND MatKhau = @MatKhau";
            int result = (int)dp.ExecuteScalar(query, new object[] { TK_DangNhap, MK_DangNhap });
            if (result == 1)
            {
                return result;
            }
            return 0;
        }

        public string tim_manv(string TK_DangNhap)
        {
            string query1 = "select MaNV from NhanVien where Email= @Email";
            string hung_manv = dp.ExecuteScalar(query1, new object[] { TK_DangNhap}).ToString();
            if (hung_manv == "")
            {
                return "";
            }
            return hung_manv;
        }

        public string lay_ten_nv(string manv)
        {
            try
            {
                string query = "select HoTen from NhanVien where MaNV = @MaNV";
                return (string)dp.ExecuteScalar(query, new object[] { manv });
            } catch { return null; }
        }
    }
}
