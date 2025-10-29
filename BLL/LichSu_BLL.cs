using System.Data;
using DAL;

namespace BLL
{
    public class LichSu_BLL
    {
        LichSu_DAO lich_su;

        public LichSu_BLL() { lich_su = new LichSu_DAO(); }

        public DataTable lay_du_lieu()
        {
            return this.lich_su.lay_du_lieu();
        }

        public string lay_ten_nhan_vien(string manv)
        {
            return this.lich_su.lay_ten_nhan_vien(manv);
        }

        public DataTable lay_du_lieu_tim_kiem(string madh)
        {
            return this.lich_su.lay_du_lieu_tim_kiem(madh);
        }

        public string noi_dung_chinh_sua(string manv, string madh, string thoigianStr)
        {
            return this.lich_su.noi_dung_chinh_sua(manv, madh, thoigianStr);
        }

        public bool kiem_tra_mau(string madh, string vitrilaymau)
        {
            return this.lich_su.kiem_tra_mau(madh, vitrilaymau);
        }

        public void them_lich_su_phong_lab_khi_thai(string manv, string madh, string vitrilaymau)
        {
            this.lich_su.them_lich_su_phong_lab_khi_thai(manv, madh, vitrilaymau);
        }

        public void them_lich_su_phong_lab_nuoc_mat(string manv, string madh, string vitrilaymau)
        {
            this.lich_su.them_lich_su_phong_lab_nuoc_mat(manv, madh, vitrilaymau);
        }

        public void them_lich_su_phong_lab_khong_khi(string manv, string madh, string vitrilaymau)
        {
            this.lich_su.them_lich_su_phong_lab_khong_khi(manv, madh, vitrilaymau);
        }

        public void them_lich_su_hien_truong_nuoc_mat(string manv, string madh, string vitrilaymau)
        {
            this.lich_su.them_lich_su_hien_truong_nuoc_mat(manv, madh, vitrilaymau);
        }

        public void them_lich_su_hien_truong_khong_khi(string manv, string madh, string vitrilaymau)
        {
            this.lich_su.them_lich_su_hien_truong_khong_khi(manv, madh, vitrilaymau);
        }

        public void them_lich_su_hien_truong_khi_thai(string manv, string madh, string vitrilaymau)
        {
            this.lich_su.them_lich_su_hien_truong_khi_thai(manv, madh, vitrilaymau);
        }

        public void them_lich_su_chi_tieu_nuoc_mat(string manv, string madh, string vitrilaymau) 
        {
            this.lich_su.them_lich_su_chi_tieu_nuoc_mat(manv, madh, vitrilaymau);
        }

        public void them_lich_su_chi_tieu_khong_khi(string manv, string madh, string vitrilaymau)
        {
            this.lich_su.them_lich_su_chi_tieu_khong_khi(manv, madh, vitrilaymau);
        }

        public void them_lich_su_chi_tieu_khi_thai(string manv, string madh, string vitrilaymau)
        {
            this.lich_su.them_lich_su_chi_tieu_khi_thai(manv, madh, vitrilaymau);
        }
    }
}
