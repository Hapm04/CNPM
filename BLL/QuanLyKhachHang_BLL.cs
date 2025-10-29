using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DAL;

namespace BLL
{
    public class QuanLyKhachHang_BLL
    {
        QuanLyKhachHang_DAO kh = new QuanLyKhachHang_DAO();

        public DataTable lay_du_lieu()
        {
            return kh.lay_du_lieu();
        }

        public DataTable tim_kiem(string ten_ct, string ma_dh, string tinh) 
        {
            return kh.tim_kiem(ten_ct, ma_dh, tinh);
        }

        public int kiem_tra_don_hang(string ma_dh)
        { 
            return kh.kiem_tra_don_hang(ma_dh);
        }

        public DataTable lay_thong_tin_khach_hang(string ma_dh) 
        {
            return kh.lay_thong_tin_khach_hang(ma_dh);
        }

        public bool KiemTraEmail(string email)
        {
            if (email.Count(c => c == '@') != 1)
            {
                return false;
            }

            string pattern = @"^[a-zA-Z0-9._%+-]+@gmail\.com$";
            return Regex.IsMatch(email, pattern);
        }
        public bool KiemTraSDT(string sdt)
        {
            string pattern = @"^0\d{9}$";
            return Regex.IsMatch(sdt, pattern);
        }

        public int them_khach_hang(string nguoi_dd, string ten_ct,
                                    string email, string dia_chi, string nganh_cn, string sdt, string ghi_chu)
        {
            return kh.them_khach_hang(nguoi_dd, ten_ct, email, dia_chi, nganh_cn, sdt, ghi_chu);
        }

        public int sua_thong_tin_khach_hang(string nguoi_dd, string ten_ct,
                                    string email, string dia_chi, string nganh_cn, string sdt, string ghi_chu, string ma_kh)
        {
            return kh.sua_thong_tin_khach_hang(nguoi_dd, ten_ct, email, dia_chi, nganh_cn, sdt, ghi_chu, ma_kh);
        }
    }
}
