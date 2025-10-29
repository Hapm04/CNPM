using DAL;
using System.Data;

namespace BLL
{
    public class QuanLyPhieuTraHang_BLL
    {
        QuanLyPhieuTraHang_DAO pth = new QuanLyPhieuTraHang_DAO();

        public DataTable lay_du_lieu()
        {
            return pth.lay_du_lieu();
        }

        public DataTable tim_kiem(string ma_dh)
        {
            return pth.tim_kiem(ma_dh);
        }

        public DataTable thong_tin_khach_hang(string ma_dh)
        {
            return pth.thong_tin_khach_hang(ma_dh);
        }

        public DataTable loai_mau(string ma_dh)
        {
            return pth.loai_mau(ma_dh);
        }

        public DataTable so_mau(string ma_dh)
        {
            return pth.so_mau(ma_dh);
        }

        public DataTable ngay_xuat_phieu(string ma_dh)
        {
            return pth.ngay_xuat_phieu(ma_dh);
        }

        public DataTable vi_tri_lay_mau(string ma_dh)
        {
            return pth.vi_tri_lay_mau(ma_dh);
        }

        public DataTable lay_du_lieu_kiem_dinh(string ma_dh)
        {
            return pth.lay_du_lieu_kiem_dinh(ma_dh);
        }
        public bool cap_nhat_phieu_tra_hang(byte[] pdfBytes, string ma_dh)
        {
            return pth.cap_nhat_phieu_tra_hang(pdfBytes, ma_dh);
        }
        public DataTable lay_phieu_tra_hang(string ma_dh)
        {
            return pth.lay_phieu_tra_hang(ma_dh);
        }
    }
}
