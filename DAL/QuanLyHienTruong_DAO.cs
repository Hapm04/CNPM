using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcoProject.DAO;
using System.Data;

namespace DAL
{
    public class QuanLyHienTruong_DAO
    {
        private DataProvider provider = new DataProvider();

        public int InsertMau(string vi_tri_lay_mau, string maDonHang, string nhan_vien, string loai_mau)
        {
            try
            {
                string mauQuery = "INSERT INTO Mau (ViTriLayMau, MaDH, MaNV, LoaiMau, Trangthai) VALUES ( @ViTriLayMau , @MaDH , @MaNV , @LoaiMau , @Trangthai )";
                return provider.ExecuteNonQuery(mauQuery, new object[] {
                vi_tri_lay_mau,
                maDonHang,
                nhan_vien,
                loai_mau,
                "Đang xử lý mẫu"
                });

            }
            catch
            {
                return 0;
            }

        }

        public DataTable GetNuocMatData(string maDH, string viTriLayMau)
        {
            try
            {
                string query = @"
                SELECT ViTriLayMau , DO , pH , TDS , NhietDo , MaNV , MaDH
                FROM NuocMat
                WHERE MaDH = @MaDH AND ViTriLayMau = @ViTriLayMau ; ";

                return provider.ExecuteQuery(query, new object[] { maDH, viTriLayMau });

            }
            catch
            {
                return null;
            }
        }

        public DataTable GetChiTieuNuocMatData(string maDH, string viTriLayMau)
        {
            try
            {
                string query = @"
                SELECT DO , pH , TDS , NhietDo
                FROM ChiTieuNuocMat
                WHERE MaDH = @MaDH AND ViTriLayMau = @ViTriLayMau ; ";

                return provider.ExecuteQuery(query, new object[] { maDH, viTriLayMau });
            }
            catch 
            {
                return null;
            }

        }

        public int InsertNuocMat(string viTriLayMau, string maDH, string maNV, float DO, float pH, float TDS, float nhietDo)
        {
            try
            {
                string query = @"
                INSERT INTO NuocMat (ViTriLayMau , DO , pH , TDS , NhietDo , MaDH , MaNV )
                VALUES ( @ViTriLayMau , @DO , @pH , @TDS , @NhietDo , @MaDH , @MaNV  ) ; ";

                return provider.ExecuteNonQuery(query, new object[] { viTriLayMau, DO, pH, TDS, nhietDo, maDH, maNV });
            }
            catch 
            {
                return 0;
            }

        }

        public int UpdateNuocMat(string viTriLayMau, string maDH, float DO, float pH, float TDS, float nhietDo, string maNV)
        {
            try
            {
                string query = @"
                UPDATE NuocMat 
                SET DO = @DO , pH = @pH , TDS = @TDS , NhietDo = @NhietDo  
                WHERE ViTriLayMau = @ViTriLayMau AND MaDH = @MaDH ; ";

                return provider.ExecuteNonQuery(query, new object[] { DO, pH, TDS, nhietDo, maNV, viTriLayMau, maDH });
            }
            catch 
            {
                return 0;
            }

        }

        public DataTable GetKhongKhiData(string viTriLayMau, string maDonHang)
        {
            try
            {
                string query = "SELECT ViTriLayMau , PM2dot5 , CO , NO2 , NhietDo , MaNV , MaDH " +
               "FROM KhongKhi " +
               "WHERE ViTriLayMau = @ViTriLayMau AND MaDH = @MaDH ; ";
                return provider.ExecuteQuery(query, new object[] { viTriLayMau, maDonHang });
            }
            catch 
            {
                return null;
            }

        }

        public DataTable GetChiTieuKKData(string viTriLayMau, string maDonHang)
        {
            try
            {
                string query = "SELECT CO , SO2 , O3 , PM10 , PM2dot5 , NhietDo , NO2 " +
               "FROM ChiTieuKK " +
               "WHERE ViTriLayMau = @ViTriLayMau AND MaDH = @MaDH ; ";
                return provider.ExecuteQuery(query, new object[] { viTriLayMau, maDonHang });
            }
            catch 
            {
                return null;
            }

        }

        public int InsertKhongKhiData(string viTriLayMau, string PM2dot5, string CO, string NO2, string nhietDo, string maDH, string maNV)
        {
            try
            {
                string query = "INSERT INTO KhongKhi (ViTriLayMau , PM2dot5 , CO , NO2 , NhietDo , MaDH , MaNV ) " +
               "VALUES ( @ViTriLayMau , @PM2dot5 , @CO , @NO2 , @NhietDo , @MaDH , @MaNV  ) ; ";
                return provider.ExecuteNonQuery(query, new object[] { viTriLayMau, PM2dot5, CO, NO2, nhietDo, maDH, maNV });
            }
            catch 
            {
                return 0;
            }

        }

        public int UpdateKhongKhiData(string viTriLayMau, string PM2dot5, string CO, string NO2, string nhietDo, string maDH, string maNV)
        {
            try
            {
                string query = "UPDATE KhongKhi SET PM2dot5 = @PM2dot5 , CO = @CO , NO2 = @NO2 , NhietDo = @NhietDo " +
               "WHERE ViTriLayMau = @ViTriLayMau AND MaDH = @MaDH ; ";
                return provider.ExecuteNonQuery(query, new object[] { PM2dot5, CO, NO2, nhietDo, maNV, viTriLayMau, maDH });
            }
            catch 
            {
                return 0;
            }
        }


        public DataTable GetKhiThaiByViTriAndMaDH(string viTri, string maDH)
        {
            try
            {
                string query = @"SELECT ViTriLayMau , SO2 , NO2 , PM , NhietDo , MaNV , MaDH 
                             FROM KhiThai 
                             WHERE ViTriLayMau = @ViTriLayMau AND MaDH = @MaDH ";
                return provider.ExecuteQuery(query, new object[] { viTri, maDH });
            }
            catch
            {
                return null;
            }

        }

        public DataTable GetChiTieuKhiThaiByViTriAndMaDH(string viTri, string maDH)
        {
            try
            {
                string query = @"SELECT SO2 , NO2 , PM , NhietDo 
                             FROM ChiTieuKhiThai 
                             WHERE ViTriLayMau = @ViTriLayMau AND MaDH = @MaDH ";
                return provider.ExecuteQuery(query, new object[] { viTri, maDH });
            }
            catch
            {
                return null;
            }

        }

        public int InsertKhiThai(string viTriLayMau, float so2, float no2, float pm, float nhietDo, string maNV, string maDH)
        {
            try
            {
                string query = @"INSERT INTO KhiThai (ViTriLayMau , SO2 , NO2 , PM , NhietDo , MaNV , MaDH ) 
                             VALUES ( @ViTriLayMau , @SO2 , @NO2 , @PM , @NhietDo , @MaNV , @MaDH ) ";
                return provider.ExecuteNonQuery(query, new object[] { viTriLayMau, so2, no2, pm, nhietDo, maNV, maDH });
            }
            catch 
            {
                return 0;
            }

        }

        public int UpdateKhiThai(string viTriLayMau, float so2, float no2, float pm, float nhietDo, string maNV, string maDH)
        {
            try
            {
                string query = @"UPDATE KhiThai 
                             SET SO2 = @SO2 , NO2 = @NO2 , PM = @PM , NhietDo = @NhietDo  
                             WHERE ViTriLayMau = @ViTriLayMau AND MaDH = @MaDH ";
                return provider.ExecuteNonQuery(query, new object[] { so2, no2, pm, nhietDo, maNV, viTriLayMau, maDH });
            }
            catch 
            {
                return 0;
            }

        }
    }
}
