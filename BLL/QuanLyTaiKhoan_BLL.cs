using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BLL
{
    public class QuanLyTaiKhoan_BLL
    {
        QuanLyTaiKhoan_DAO quan_ly_tk;
        public QuanLyTaiKhoan_BLL() { quan_ly_tk = new QuanLyTaiKhoan_DAO(); }

        public int check_tk_mk(string TK_DangNhap, string MK_DangNhap)
        {
            return this.quan_ly_tk.check_tk_mk(TK_DangNhap, MK_DangNhap);
        }

        public string tim_manv(string TK_DangNhap)
        {
            return this.quan_ly_tk.tim_manv(TK_DangNhap);
        }

        public string lay_ten_nv(string manv)
        {
            return this.quan_ly_tk.lay_ten_nv(manv);
        }
    }
}
