using System;
using System.Data;
using EcoProject.DAO;

namespace DAL
{
    public class LichSu_DAO
    {
        DataProvider dp;

        public LichSu_DAO() { dp = new DataProvider(); }

        public DataTable lay_du_lieu()
        {
            try
            {
                string lay_du_lieu = "select * from LichSu";
                return dp.ExecuteQuery(lay_du_lieu);
            }
            catch { return null; }
        }

        public string lay_ten_nhan_vien(string manv)
        {
            try
            {
                string ten_nv = "select HoTen from NhanVien where MaNV = @manv";
                string ten = dp.ExecuteScalar(ten_nv, new object[] { manv }).ToString();
                return ten;
            }
            catch { return null; }
        }

        public DataTable lay_du_lieu_tim_kiem(string madh)
        {
            try
            {
                string tim_kiem = "select * from LichSu where MaDH LIKE '%' + @madh + '%' ";
                return dp.ExecuteQuery(tim_kiem, new object[] {madh});
            }
            catch { return null; }
        }

        public string noi_dung_chinh_sua(string manv, string madh, string thoigianStr)
        {
            try
            {
                string lay_noi_dung = "select NoiDungChinhSua from LichSu where MaNV = @manv and MaDH = @madh and ThoiGianChinhSua = @thoigian";
                return dp.ExecuteScalar(lay_noi_dung, new object[] { manv, madh, thoigianStr }).ToString();
            }
            catch { return null; }
        }

        public bool kiem_tra_mau(string madh, string vitrilaymau)
        {
            string query = "Select TrangThai from Mau where MaDH = @madh and ViTriLayMau = @vitrilaymau";
            string result = dp.ExecuteScalar(query, new object[] { madh, vitrilaymau }).ToString();
            if (result == "Đã hoàn thành")
            {
                return true;
            }
            return false;
        }

        public void them_lich_su_phong_lab_khi_thai(string manv, string madh, string vitrilaymau)
        {
            try
            {
                string content = $"Nhân viên {manv} đã thay đổi dữ liệu phòng thí nghiệm của mẫu khí thải thuộc đơn hàng {madh} có vị trí lấy mẫu là {vitrilaymau} vào lúc {DateTime.Now}";
                string inert_lich_su = "insert into LichSu values ( @manv , @thoigian , @madh , @content )";
                this.dp.ExecuteNonQuery(inert_lich_su, new object[] { manv, DateTime.Now, madh, content });
            }
            catch { return; }
        }

        public void them_lich_su_phong_lab_nuoc_mat(string manv, string madh, string vitrilaymau)
        {
            try
            {
                string content = $"Nhân viên {manv} đã thay đổi dữ liệu phòng thí nghiệm của mẫu nước mặt thuộc đơn hàng {madh} có vị trí lấy mẫu là {vitrilaymau} vào lúc {DateTime.Now}";
                string inert_lich_su = "insert into LichSu values ( @manv , @thoigian , @madh , @content )";
                this.dp.ExecuteNonQuery(inert_lich_su, new object[] { manv, DateTime.Now, madh, content });
            }
            catch { return ; }
        }

        public void them_lich_su_phong_lab_khong_khi(string manv, string madh, string vitrilaymau)
        {
            try
            {
                string content = $"Nhân viên {manv} đã thay đổi dữ liệu phòng thí nghiệm của mẫu không khí xung quanh thuộc đơn hàng {madh} có vị trí lấy mẫu là {vitrilaymau} vào lúc {DateTime.Now}";
                string inert_lich_su = "insert into LichSu values ( @manv , @thoigian , @madh , @content )";
                this.dp.ExecuteNonQuery(inert_lich_su, new object[] { manv, DateTime.Now, madh, content });
            }
            catch { return; }
        }

        public void them_lich_su_hien_truong_nuoc_mat(string manv, string madh, string vitrilaymau)
        {
            try
            {
                string content = $"Nhân viên {manv} đã thay đổi dữ liệu hiện trường của mẫu nước mặt thuộc đơn hàng {madh} có vị trí lấy mẫu là {vitrilaymau} vào lúc {DateTime.Now}";
                string inert_lich_su = "insert into LichSu values ( @manv , @thoigian , @madh , @content )";
                this.dp.ExecuteNonQuery(inert_lich_su, new object[] { manv, DateTime.Now, madh, content });
            }
            catch { return; }
        }

        public void them_lich_su_hien_truong_khong_khi(string manv, string madh, string vitrilaymau)
        {
            try
            {
                string content = $"Nhân viên {manv} đã thay đổi dữ liệu hiện trường của không khí xung quanh thuộc đơn hàng {madh} có vị trí lấy mẫu là {vitrilaymau} vào lúc {DateTime.Now}";
                string inert_lich_su = "insert into LichSu values ( @manv , @thoigian , @madh , @content )";
                this.dp.ExecuteNonQuery(inert_lich_su, new object[] { manv, DateTime.Now, madh, content });
            }
            catch { return; }
        }

        public void them_lich_su_hien_truong_khi_thai(string manv, string madh, string vitrilaymau)
        {
            try
            {
                string content = $"Nhân viên {manv} đã thay đổi dữ liệu hiện trường của không khí xung quanh thuộc đơn hàng {madh} có vị trí lấy mẫu là {vitrilaymau} vào lúc {DateTime.Now}";
                string inert_lich_su = "insert into LichSu values ( @manv , @thoigian , @madh , @content )";
                this.dp.ExecuteNonQuery(inert_lich_su, new object[] { manv, DateTime.Now, madh, content });
            }
            catch { return; }
        }

        public void them_lich_su_chi_tieu_nuoc_mat(string manv, string madh, string vitrilaymau)
        {
            try
            {
                string content = $"Nhân viên {manv} đã thay đổi dữ liệu chỉ tiêu của mẫu nước mặt thuộc đơn hàng {madh} có vị trí lấy mẫu là {vitrilaymau} vào lúc {DateTime.Now}";
                string inert_lich_su = "insert into LichSu values ( @manv , @thoigian , @madh , @content )";
                this.dp.ExecuteNonQuery(inert_lich_su, new object[] { manv, DateTime.Now, madh, content });
            }
            catch { return; }
        }

        public void them_lich_su_chi_tieu_khong_khi(string manv, string madh, string vitrilaymau) 
        {
            try
            {
                string content = $"Nhân viên {manv} đã thay đổi dữ liệu chỉ tiêu của mẫu không khí xung quanh thuộc đơn hàng {madh} có vị trí lấy mẫu là {vitrilaymau} vào lúc {DateTime.Now}";
                string inert_lich_su = "insert into LichSu values ( @manv , @thoigian , @madh , @content )";
                this.dp.ExecuteNonQuery(inert_lich_su, new object[] { manv, DateTime.Now, madh, content });
            }
            catch { return; }
        }

        public void them_lich_su_chi_tieu_khi_thai(string manv, string madh, string vitrilaymau) 
        {
            try
            {
                string content = $"Nhân viên {manv} đã thay đổi dữ liệu chỉ tiêu của mẫu khí thải thuộc đơn hàng {madh} có vị trí lấy mẫu là {vitrilaymau} vào lúc {DateTime.Now}";
                string inert_lich_su = "insert into LichSu values ( @manv , @thoigian , @madh , @content )";
                this.dp.ExecuteNonQuery(inert_lich_su, new object[] { manv, DateTime.Now, madh, content });
            }
            catch { return; }
        }
    }
}
