using System.Data;
using EcoProject.DAO;

namespace DAL
{
    public class QuanLyThongBao_DAO
    {
        DataProvider dp;
        public QuanLyThongBao_DAO() { dp = new DataProvider(); }

        public DataTable lay_du_lieu()
        {
            try
            {
                string query = "select TenCongTy, DonHang.MaDH, HanTraHang from DonHang, KhachHang where DonHang.MaKH = KhachHang.MaKH and (Datediff(day, GetDate(), DonHang.HanTraHang) < 4 or Datediff(day, GetDate(), DonHang.HanTraHang) < 0) and (NgayXuatPhieuTraHang is null)";
                return this.dp.ExecuteQuery(query); ;
            }
            catch { return null; }
        }

        public DataTable qua_han()
        {
            try
            {
                string query = "select TenCongTy, DonHang.MaDH, HanTraHang from DonHang, KhachHang where DonHang.MaKH = KhachHang.MaKH and (Datediff(day, GetDate(), DonHang.HanTraHang) < 0) and (NgayXuatPhieuTraHang is null)";
                return this.dp.ExecuteQuery(query);
            }
            catch { return null; }
        }

        public DataTable sap_qua_han()
        {
            try
            {
                string query = "select TenCongTy, DonHang.MaDH, HanTraHang from DonHang, KhachHang where DonHang.MaKH = KhachHang.MaKH and (Datediff(day, GetDate(), DonHang.HanTraHang) between 0 and 4) and (NgayXuatPhieuTraHang is null)";
                return dp.ExecuteQuery(query);
            }
            catch { return null; }
        }
    }
}
