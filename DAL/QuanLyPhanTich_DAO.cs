using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcoProject.DAO;

namespace DAL
{
    public class QuanLyPhanTich_DAO
    {
        DataProvider dp;

        public QuanLyPhanTich_DAO() { dp = new DataProvider(); }

        public object lay_ngay_xuat_phieu(string madh)
        {
            try
            {
                string query = @"SELECT NgayXuatPhieuTraHang FROM DonHang WHERE MaDH = @MaDH";
                return dp.ExecuteScalar(query, new object[] { madh });
            }
            catch { return null; }
        }

        public object kiem_tra_ma_dh(string madh)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM DonHang WHERE MaDH = @madh";
                return dp.ExecuteScalar(query, new object[] { madh });
            }
            catch { return null; }
        }

        public DataTable lay_du_lieu_don_hang()
        {
            try
            {
                string query = "SELECT MaDH FROM DonHang";
                return dp.ExecuteQuery(query);
            }
            catch { return null; }
        }

        public DataTable lay_du_lieu_tong()
        {
            try
            {
                string query = "SELECT MaDH AS 'Mã Đơn Hàng', ViTriLayMau AS 'Vị Trí Lấy Mẫu', LoaiMau AS 'Loại mẫu', Trangthai AS 'Trạng Thái', Ketqua AS 'Kết Quả' FROM Mau";
                return dp.ExecuteQuery(query);
            }
            catch { return null; }
        }

        public DataTable tim_kiem_don_hang(string madh)
        {
            try
            {
                string query = @"SELECT MaDH AS [Mã Đơn Hàng], 
                            ViTriLayMau AS [Vị Trí Lấy Mẫu], 
                            TrangThai AS [Trạng Thái], 
                            KetQua AS [Kết Quả], 
                            LoaiMau AS [Loại Mẫu]
                     FROM Mau 
                     WHERE MaDH LIKE @MaDH";
                return dp.ExecuteQuery(query, new object[] { madh });
            }
            catch { return null; }
        }
    }
}
