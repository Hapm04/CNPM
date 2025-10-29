using System.Data;
using EcoProject.DAO;

namespace DAL
{
    public class QuanLyPhongThiNghiem_DAO
    {
        private DataProvider provider = new DataProvider();

        public DataTable LayDuLieuNuocMat(string viTriLayMau , string maDH)
        {
            try
            {
                string query = "SELECT ViTriLayMau , NH4 , NO3 , PO4 , COD , TSS , tongN , TOC , tongP , NhietDo , MaNV , MaDH " +
                           "FROM NuocMat " +
                           "WHERE ViTriLayMau = @ViTriLayMau AND MaDH = @MaDH ; ";
                return provider.ExecuteQuery(query, new object[] { viTriLayMau, maDH });
            }
            catch { return null; }
            
        }

        public int CapNhatTrangThaiMau(string maDH , string viTriLayMau)
        {
            try
            {
                string query = "UPDATE Mau SET TrangThai = N'Đã hoàn thành' WHERE MaDH = @MaDH AND ViTriLayMau = @ViTriLayMau ; ";
                return provider.ExecuteNonQuery(query, new object[] { maDH, viTriLayMau });
            }
            catch { return 0; }
        }

        public int CapNhatDuLieuNuocMat(object[] parameters)
        {
            try
            {
                string query = "UPDATE NuocMat SET NH4 = @NH4 , NO3 = @NO3 , PO4 = @PO4 , COD = @COD , TSS = @TSS , " +
                               "tongN = @tongN , TOC = @TOC , tongP = @tongP WHERE MaDH = @MaDH AND ViTriLayMau = @ViTriLayMau ";
                return provider.ExecuteNonQuery(query, parameters);
            }
            catch { return 0; }
        }

        public DataTable GetKhongKhiData(string maDH , string viTriLayMau)
        {
            try
            {
                string query = "SELECT * FROM KhongKhi WHERE MaDH = @MaDH AND ViTriLayMau = @ViTriLayMau ";
                return provider.ExecuteQuery(query, new object[] { maDH, viTriLayMau });
            }
            catch {return null; }
        }

        public DataTable GetChiTieuKhongKhi(string maDH, string viTriLayMau)
        {
            try
            {
                string query = "SELECT * FROM ChiTieuKK WHERE MaDH = @MaDH AND ViTriLayMau = @ViTriLayMau ";
                return provider.ExecuteQuery(query, new object[] { maDH, viTriLayMau });
            }
            catch {return null; }
        }

        public DataTable GetChiTieuNuocMat(string maDH, string viTriLayMau)
        {
            try
            {
                string query = "SELECT * FROM ChiTieuNuocMat WHERE MaDH = @MaDH AND ViTriLayMau = @ViTriLayMau ";
                return provider.ExecuteQuery(query, new object[] { maDH, viTriLayMau });
            }
            catch { return null; }
        }

        public int UpdateKhongKhi(string maDH , string viTriLayMau , float pm10 , float so2 , float o3)
        {
            try
            {
                string query = "UPDATE KhongKhi SET PM10 = @PM10 , SO2 = @SO2 , O3 = @O3 WHERE MaDH = @MaDH AND ViTriLayMau = @ViTriLayMau ";
                return provider.ExecuteNonQuery(query, new object[] { pm10, so2, o3, maDH, viTriLayMau });
            }
            catch {return 0; }
        }

        public DataTable GetKhiThai(string viTriLayMau , string maDH)
        {
            try
            {
                string query = "SELECT * FROM KhiThai WHERE ViTriLayMau = @ViTriLayMau AND MaDH = @MaDH ; ";
                return provider.ExecuteQuery(query, new object[] { viTriLayMau, maDH });
            }
            catch { return null; }
        }

        public DataTable GetChiTieuKhiThai(string viTriLayMau , string maDH)
        {
            try
            {
                string query = "SELECT * FROM ChiTieuKhiThai WHERE ViTriLayMau = @ViTriLayMau AND MaDH = @MaDH ; ";
                return provider.ExecuteQuery(query, new object[] { viTriLayMau, maDH });
            }
            catch { return null; }
        }

        public int UpdateKhiThai(string maDH , string viTriLayMau , object[] values)
        {
            try
            {
                string query = @"
                UPDATE KhiThai 
                SET ApSuat = @ApSuat , CO = @CO , H2S = @H2S , O2 = @O2 , NH3 = @NH3 , Hg = @Hg , N_O = @N_O 
                WHERE MaDH = @MaDH AND ViTriLayMau = @ViTriLayMau ";
                return provider.ExecuteNonQuery(query, values);
            }
            catch { return 0; }
        }

    }
}
