using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BLL
{
    public class QuanLyPhanTich_BLL
    {
        QuanLyPhanTich_DAO quan_ly_pt;

        public QuanLyPhanTich_BLL() { quan_ly_pt = new QuanLyPhanTich_DAO(); }

        public object lay_ngay_xuat_phieu(string madh)
        {
            return quan_ly_pt.lay_ngay_xuat_phieu(madh);
        }

        public object kiem_tra_ma_dh(string madh)
        {
            return quan_ly_pt.kiem_tra_ma_dh(madh);
        }

        public DataTable lay_du_lieu_don_hang()
        {
            return quan_ly_pt.lay_du_lieu_don_hang();
        }

        public DataTable lay_du_lieu_tong()
        {
            return quan_ly_pt.lay_du_lieu_tong();
        }

        public DataTable tim_kiem_don_hang(string madh)
        {
            return quan_ly_pt.tim_kiem_don_hang(madh);
        }
    }
}
