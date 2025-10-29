using System;
using System.Data;
using DAL;

namespace BLL
{
    public class QuanLyDonHang_BLL
    {
        QuanLyDonHang_DAO quanly;

        public QuanLyDonHang_BLL()
        {
            quanly = new QuanLyDonHang_DAO();
        }

        public DataTable lay_du_lieu_don_hang()
        {
            return this.quanly.lay_du_lieu();
        }

        public DataTable tim_kiem(string madh, string ten)
        {
            if (madh  == null || ten == null)
            {
                return null;
            }
            return this.quanly.tim_kiem( madh, ten);
        }

        public string lay_quy(string madh)
        {
            if (madh  == null)
            {
                return null;
            }
            return quanly.lay_quy( madh);
        }

        public Boolean xoa_don_hang(string madh)
        {
            return this.quanly.xoa_don_hang(madh);
        }

        public DataTable cong_ty_da_xu_ly()
        {
            return this.quanly.cong_ty_da_xu_ly();
        }

        public DataTable tim_kiem_cong_ty(string chuoi_tim_kiem)
        {
            return this.quanly.tim_kiem_cong_ty(chuoi_tim_kiem);
        }

        public string kiem_tra_makh(string makh)
        {
            return this.quanly.kiem_tra_makh(makh);
        }

        public int them_don_hang(DateTime traketqua, DateTime ngaydathang, string makh, string quy)
        {
            return this.quanly.them_don_hang(traketqua, ngaydathang, makh, quy);
        }

        public Boolean cap_nhat_don_hang(DateTime traketqua, DateTime ngaydathang, string makh, string quy, string madh)
        {
            return this.quanly.cap_nhat_don_hang(traketqua, ngaydathang, makh, quy, madh);
        }
    }
}
