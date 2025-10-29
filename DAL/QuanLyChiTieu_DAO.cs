using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using EcoProject.DAO;

namespace DAL
{
    public class QuanLyChiTieu_DAO
    {
        private DataProvider provider = new DataProvider();

        public DataTable LayDuLieuNuocMat(string maDH, string viTriLayMau)
        {
            try
            {
                string query = @"
                SELECT TDS , NhietDo , pH , PO4 , NH4 , tongP , tongN , TOC , TSS , COD , DO , NO3
                FROM NuocMat
                WHERE MaDH = @MaDH AND ViTriLayMau = @ViTriLayMau ; ";
                return provider.ExecuteQuery(query, new object[] { maDH, viTriLayMau });
            }
            catch
            {
                return null;
            }

        }

        public DataTable LayDuLieuKhiThai(string maDH, string viTriLayMau)
        {
            try
            {
                string query = @"
                SELECT ApSuat , CO , H2S , O2 , NH3 , Hg , N_O , SO2 , NO2 , PM , NhietDo
                FROM KhiThai
                WHERE MaDH = @MaDH AND ViTriLayMau = @ViTriLayMau ; ";
                return provider.ExecuteQuery(query, new object[] { maDH, viTriLayMau });
            }
            catch
            {
                return null;
            }

        }

        public DataTable LayDuLieuKhongKhi(string maDH, string viTriLayMau)
        {
            try
            {
                string query = @"
                SELECT PM2dot5 , CO , NO2 , NhietDo , PM10 , SO2 , O3
                FROM KhongKhi
                WHERE MaDH = @MaDH AND ViTriLayMau = @ViTriLayMau ; ";
                return provider.ExecuteQuery(query, new object[] { maDH, viTriLayMau });
            }
            catch
            {
                return null;
            }

        }

        public int ThemChiTieuNuocMat(string maDH, string viTriLayMau, string maNV, Dictionary<string, float> chiTieu)
        {
            try
            {
                string query = @"
                INSERT INTO ChiTieuNuocMat ( ViTriLayMau , MaDH , MaNV , NH4 , NO3 , PO4 , TongN , TSS , COD , TOC , TongP , DO , pH , TDS , NhietDo ) 
                VALUES ( @ViTriLayMau , @MaDH , @MaNV , @NH4 , @NO3 , @PO4 , @TongN , @TSS , @COD , @TOC , @TongP , @DO , @pH , @TDS , @NhietDo ) ";
                return provider.ExecuteNonQuery(query, new object[] {
                viTriLayMau , maDH , maNV ,
                chiTieu["NH4"] , chiTieu["NO3"] , chiTieu["PO4"] , chiTieu["TongN"] ,
                chiTieu["TSS"] , chiTieu["COD"] , chiTieu["TOC"] , chiTieu["TongP"] ,
                chiTieu["DO"] , chiTieu["pH"] , chiTieu["TDS"] , chiTieu["NhietDo"]
            });
            }
            catch
            {
                return 0;
            }

        }

        public int CapNhatChiTieuNuocMat(string maDH, string viTriLayMau, Dictionary<string, float> chiTieu)
        {
            try
            {
                string query = @"
                UPDATE ChiTieuNuocMat
                SET NH4 = @NH4 , NO3 = @NO3 , PO4 = @PO4 , TongN = @TongN , TSS = @TSS , COD = @COD , 
                    TOC = @TOC , TongP = @TongP , DO = @DO , pH = @pH , TDS = @TDS , NhietDo = @NhietDo 
                WHERE ViTriLayMau = @ViTriLayMau AND MaDH = @MaDH ";
                return provider.ExecuteNonQuery(query, new object[] {
                chiTieu["NH4"] , chiTieu["NO3"] , chiTieu["PO4"] , chiTieu["TongN"] ,
                chiTieu["TSS"] , chiTieu["COD"] , chiTieu["TOC"] , chiTieu["TongP"] ,
                chiTieu["DO"] , chiTieu["pH"] , chiTieu["TDS"] , chiTieu["NhietDo"] ,
                viTriLayMau , maDH
            });
            }
            catch
            {
                return 0;
            }

        }

        public int CapNhatKetQuaMau(string maDH, string viTriLayMau, string ketQua)
        {
            try
            {
                string query = "UPDATE Mau SET Ketqua = @KetQua WHERE MaDH = @MaDH AND ViTriLayMau = @ViTriLayMau ";
                return provider.ExecuteNonQuery(query, new object[] { ketQua, maDH, viTriLayMau });
            }
            catch { return 0; }

        }

        public DataTable LayChiTieuKhongKhi(string maDH, string viTriLayMau)
        {
            try
            {
                string query = @"
                SELECT ViTriLayMau , PM2dot5 , CO , NO2 , NhietDo , PM10 , SO2 , O3 , MaNV , MaDH 
                FROM ChiTieuKK 
                WHERE ViTriLayMau = @ViTriLayMau AND MaDH = @MaDH ; ";
                return provider.ExecuteQuery(query, new object[] { viTriLayMau, maDH });
            }
            catch { return null; }

        }

        public DataTable LayChiTieuKhiThai(string maDH, string viTriLayMau)
        {
            try
            {
                string query = @"
                SELECT ViTriLayMau , ApSuat , CO , H2S , O2 , NH3 , Hg , N_O , SO2 , NO2 , PM , NhietDo , MaNV , MaDH
                FROM ChiTieuKhiThai
                WHERE ViTriLayMau = @ViTriLayMau AND MaDH = @MaDH ; ";
                return provider.ExecuteQuery(query, new object[] { viTriLayMau, maDH });
            }
            catch { return null; }

        }

        public DataTable LayChiTieuNuocMat(string maDH, string viTriLayMau)
        {
            try
            {
                string query = @"
                SELECT ViTriLayMau , NH4 , NO3 , PO4 , TongN , TSS , COD , TOC , TongP , DO , pH , TDS , NhietDo , MaNV , MaDH
                FROM ChiTieuNuocMat
                WHERE ViTriLayMau = @ViTriLayMau AND MaDH = @MaDH ; ";
                return provider.ExecuteQuery(query, new object[] { viTriLayMau, maDH });
            }
            catch { return null; }

        }

        public int ThemChiTieuKhongKhi(string maDH, string viTriLayMau, string maNV, float pm2dot5, float co, float no2, float nhietDo, float pm10, float so2, float o3)
        {
            try
            {
                string query = @"
                INSERT INTO ChiTieuKK ( ViTriLayMau , MaDH , MaNV , PM2dot5 , CO , NO2 , NhietDo , PM10 , SO2 , O3 ) 
                VALUES ( @ViTriLayMau , @MaDH , @MaNV , @PM2dot5 , @CO , @NO2 , @NhietDo , @PM10 , @SO2 , @O3 ) ; ";
                return provider.ExecuteNonQuery(query, new object[] { viTriLayMau, maDH, maNV, pm2dot5, co, no2, nhietDo, pm10, so2, o3 });
            }
            catch { return 0; }

        }

        public int CapNhatChiTieuKhongKhi(string maDH, string viTriLayMau, float pm2dot5, float co, float no2, float nhietDo, float pm10, float so2, float o3)
        {
            try
            {
                string query = @"
                UPDATE ChiTieuKK 
                SET PM2dot5 = @PM2dot5 , CO = @CO , NO2 = @NO2 , NhietDo = @NhietDo , PM10 = @PM10 , SO2 = @SO2 , O3 = @O3 
                WHERE ViTriLayMau = @ViTriLayMau AND MaDH = @MaDH ; ";
                return provider.ExecuteNonQuery(query, new object[] { pm2dot5, co, no2, nhietDo, pm10, so2, o3, viTriLayMau, maDH });
            }
            catch { return 0; }

        }

        public int ThemChiTieuKhiThai(string viTriLayMau, string maDongHang, string maNV, float apSuat, float co, float h2s, float o2, float nh3, float hg, float no, float so2, float no2, float pm, float nhietDo)
        {
            try
            {
                string query = @"INSERT INTO ChiTieuKhiThai 
                             ( ViTriLayMau , MaDH , MaNV , ApSuat , CO , H2S , O2 , NH3 , Hg , N_O , SO2 , NO2 , PM , NhietDo ) 
                             VALUES ( @ViTriLayMau , @MaDH , @MaNV , @ApSuat , @CO , @H2S , @O2 , @NH3 , @Hg , @NO , @SO2 , @NO2 , @PM , @NhietDo ) ";
                return provider.ExecuteNonQuery(query, new object[] { viTriLayMau, maDongHang, maNV, apSuat, co, h2s, o2, nh3, hg, no, so2, no2, pm, nhietDo });

            }
            catch { return 0; }

        }

        public int CapNhatChiTieuKhiThai(string viTriLayMau, string maDongHang, float apSuat, float co, float h2s, float o2, float nh3, float hg, float no, float so2, float no2, float pm, float nhietDo)
        {
            try
            {
                string query = @"UPDATE ChiTieuKhiThai 
                             SET ApSuat = @ApSuat , CO = @CO , H2S = @H2S , O2 = @O2 , NH3 = @NH3 , Hg = @Hg , N_O = @NO , SO2 = @SO2 , NO2 = @NO2 , PM = @PM , NhietDo = @NhietDo 
                             WHERE ViTriLayMau = @ViTriLayMau AND MaDH = @MaDH ";
                return provider.ExecuteNonQuery(query, new object[] { apSuat, co, h2s, o2, nh3, hg, no, so2, no2, pm, nhietDo, viTriLayMau, maDongHang });

            }
            catch { return 0; }

        }

        public DataTable LayChiTieuKhongKhiFull(string viTriLayMau, string maDongHang)
        {
            try
            {
                string query = @"SELECT * FROM ChiTieuKhiThai WHERE ViTriLayMau = @ViTriLayMau AND MaDH = @MaDH ";
                return provider.ExecuteQuery(query, new object[] { viTriLayMau, maDongHang });
            }
            catch { return null; }

        }


    }
}
