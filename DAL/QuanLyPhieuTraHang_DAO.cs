using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcoProject.DAO;

namespace DAL
{
    public class QuanLyPhieuTraHang_DAO
    {
        DataProvider dp = new DataProvider();
        public DataTable lay_du_lieu()
        {
            DataTable dt = new DataTable();
            try
            {
                string query1 = "SELECT DonHang.MaDH, TenCongTy, NgayTaoDH" +
                            " FROM KhachHang" +
                            " JOIN DonHang ON KhachHang.MaKH = DonHang.MaKH" +
                            " JOIN Mau ON DonHang.MaDH = Mau.MaDH" +
                            " GROUP BY DonHang.MaDH, TenCongTy, NgayTaoDH" +
                            " HAVING COUNT(CASE WHEN Mau.TrangThai != N'Đã hoàn thành' THEN 1 END) = 0;";
                dt = dp.ExecuteQuery(query1);
            }
            catch { return null; }
            return dt;
        }

        public DataTable tim_kiem(string ma_dh)
        {
            DataTable dt = new DataTable();
            try
            {
                string query1 = "SELECT DonHang.MaDH, TenCongTy, NgayTaoDH " +
                                "FROM KhachHang " +
                                "JOIN DonHang ON KhachHang.MaKH = DonHang.MaKH " +
                                "JOIN Mau ON DonHang.MaDH = Mau.MaDH AND DonHang.MaDh LIKE @madh " +
                                "GROUP BY DonHang.MaDH, TenCongTy, NgayTaoDH " +
                                "HAVING COUNT(CASE WHEN Mau.TrangThai != N'Đã hoàn thành' THEN 1 END) = 0;";
                dt = dp.ExecuteQuery(query1, new object[] { ma_dh });
            }
            catch { return null; }
            return dt;
        }

        public DataTable thong_tin_khach_hang(string ma_dh)
        {
            DataTable dt = new DataTable();
            try
            {
                string q1 = "select TenCongTy, DiaChi, NgayTaoDH " +
                            "from KhachHang, DonHang " +
                            "where KhachHang.MaKH = donhang.MaKH and MaDH = @madh";
                dt = dp.ExecuteQuery(q1, new object[] { ma_dh });
            }
            catch { return null; }
            return dt;
        }

        public DataTable loai_mau(string ma_dh)
        {
            DataTable dt = new DataTable();
            try
            {
                string q2 = "SELECT DISTINCT LoaiMau FROM Mau WHERE MaDH = @madh";
                dt = dp.ExecuteQuery(q2, new object[] { ma_dh });
            }
            catch { return null; }
            return dt;
        }

        public DataTable so_mau(string ma_dh)
        {
            DataTable dt = new DataTable();
            try
            {
                string q3 = "SELECT COUNT(*) AS SoLuongMau " +
                            "FROM Mau " +
                            "WHERE MaDH = @madh";
                dt = dp.ExecuteQuery(q3, new object[] { ma_dh });
            }
            catch { return null; }
            return dt;
        }

        public DataTable ngay_xuat_phieu(string ma_dh)
        {
            DataTable dt = new DataTable();
            try
            {
                string q4 = "SELECT COALESCE(NgayXuatPhieuTraHang, GETDATE()) AS NgayXuatPhieuTraHang " +
                            "FROM DonHang WHERE madh = @madh";
                dt = dp.ExecuteQuery(q4, new object[] { ma_dh });
            }
            catch { return null; }
            return dt;
        }

        public DataTable vi_tri_lay_mau(string ma_dh)
        {
            DataTable dt = new DataTable();
            try
            {
                string q5 = "select ViTriLayMau, LoaiMau from Mau where MaDH = @madh";
                dt = dp.ExecuteQuery(q5, new object[] { ma_dh });
            }
            catch { return null; }
            return dt;
        }

        public DataTable lay_du_lieu_kiem_dinh(string ma_dh)
        {
            DataTable dt = new DataTable();
            try
            {
                string q6 = "SELECT " +
                            "M.LoaiMau, M.ViTriLayMau, " +
                            "KK.CO AS KhongKhi_CO, KK.SO2 AS KhongKhi_SO2, KK.O3 AS KhongKhi_O3, " +
                            "KK.PM10 AS KhongKhi_PM10, KK.PM2dot5 AS KhongKhi_PM2dot5, " +
                            "KK.NhietDo AS KhongKhi_NhietDo, KK.NO2 AS KhongKhi_NO2, " +
                            "KT.CO AS KhiThai_CO, KT.NhietDo AS KhiThai_NhietDo, " +
                            "KT.NO2 AS KhiThai_NO2, KT.O2 AS KhiThai_O2, KT.Hg AS KhiThai_Hg, KT.PM AS KhiThai_PM, " +
                            "KT.NH3 AS KhiThai_NH3, KT.N_O AS KhiThai_N_O, KT.ApSuat AS KhiThai_ApSuat, " +
                            "KT.SO2 AS KhiThai_SO2, KT.H2S AS KhiThai_H2S, " +
                            "NM.TDS AS NuocMat_TDS, NM.NhietDo AS NuocMat_NhietDo, " +
                            "NM.pH AS NuocMat_pH, NM.PO4 AS NuocMat_PO4, NM.NH4 AS NuocMat_NH4, NM.tongP AS NuocMat_tongP, " +
                            "NM.tongN AS NuocMat_tongN, NM.TOC AS NuocMat_TOC, NM.TSS AS NuocMat_TSS, " +
                            "NM.COD AS NuocMat_COD, NM.DO AS NuocMat_DO, NM.NO3 AS NuocMat_NO3 " +
                            "FROM Mau M " +
                            "LEFT JOIN KhongKhi KK ON M.MaNV = KK.MaNV AND M.MaDH = KK.MaDH AND M.ViTriLayMau = KK.ViTriLayMau " +
                            "LEFT JOIN KhiThai KT ON M.MaNV = KT.MaNV AND M.MaDH = KT.MaDH AND M.ViTriLayMau = KT.ViTriLayMau " +
                            "LEFT JOIN NuocMat NM ON M.MaNV = NM.MaNV AND M.MaDH = NM.MaDH AND M.ViTriLayMau = NM.ViTriLayMau " +
                            "WHERE M.LoaiMau IN (N'Không khí xung quanh', N'Khí Thải', N'Nước mặt') " +
                            "AND M.MaDH = @madh";
                dt = dp.ExecuteQuery(q6, new object[] { ma_dh });
            }
            catch { return null; }
            return dt;
        }
        public bool cap_nhat_phieu_tra_hang(byte[] pdfBytes, string ma_dh)
        {
            try
            {
                string query = "UPDATE DonHang " +
                               "SET PhieuTraHang = @pdfData , NgayXuatPhieuTraHang = GETDATE() " +
                               "WHERE MaDH = @MaDH";
                dp.ExecuteQuery(query, new object[] { pdfBytes, ma_dh });
            }
            catch { return false; }
            return true;
        }

        public DataTable lay_phieu_tra_hang(string ma_dh)
        {
            DataTable dt = new DataTable();
            try
            {
                string query = "SELECT PhieuTraHang FROM DonHang WHERE MaDH = @MaDH";
                dt = dp.ExecuteQuery(query, new object[] { ma_dh });
            }
            catch { return null; }
            return dt;
        }
    }
}