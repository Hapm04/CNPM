using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class QuanLyHienTruong_BLL
    {
        private QuanLyHienTruong_DAO hienTruongDao = new QuanLyHienTruong_DAO();

        public bool ThemMau(string vi_tri_lay_mau, string maDonHang, string nhan_vien, string loai_mau)
        {
            return hienTruongDao.InsertMau(vi_tri_lay_mau, maDonHang, nhan_vien, loai_mau) > 0;
        }

        public DataTable LayDuLieuNuocMat(string maDH, string viTriLayMau)
        {
            return hienTruongDao.GetNuocMatData(maDH, viTriLayMau);
        }

        public DataTable LayChiTieuNuocMat(string maDH, string viTriLayMau)
        {
            return hienTruongDao.GetChiTieuNuocMatData(maDH, viTriLayMau);
        }

        public bool ThemNuocMat(string viTriLayMau, string maDH, string maNV, float DO, float pH, float TDS, float nhietDo)
        {
            int result = hienTruongDao.InsertNuocMat(viTriLayMau, maDH, maNV, DO, pH, TDS, nhietDo);
            return result > 0;
        }

        public bool CapNhatNuocMat(string viTriLayMau, string maDH, float DO, float pH, float TDS, float nhietDo, string maNV)
        {
            int result = hienTruongDao.UpdateNuocMat(viTriLayMau, maDH, DO, pH, TDS, nhietDo, maNV);
            return result > 0;
        }

        public DataTable LayDuLieuKhongKhi(string viTriLayMau, string maDonHang)
        {
            return hienTruongDao.GetKhongKhiData(viTriLayMau, maDonHang);
        }

        public DataTable LayChiTieuKK(string viTriLayMau, string maDonHang)
        {
            return hienTruongDao.GetChiTieuKKData(viTriLayMau, maDonHang);
        }

        public string SoSanhGiaTri(float actualValue, float thresholdValue, string chiTieuName)
        {
            if (actualValue > thresholdValue)
            {
                float vuotPhanTram = ((actualValue - thresholdValue) / thresholdValue) * 100;
                return vuotPhanTram > 5 ? $"Vượt ngưỡng {vuotPhanTram:F2}% ({actualValue} > {thresholdValue})" : "";
            }
            else if (actualValue < thresholdValue)
            {
                float thapPhanTram = ((thresholdValue - actualValue) / thresholdValue) * 100;
                return thapPhanTram > 5 ? $"Thấp ngưỡng {thapPhanTram:F2}% ({actualValue} < {thresholdValue})" : "";
            }
            return "";
        }

        public int ThemDuLieuKhongKhi(string viTriLayMau, string PM2dot5, string CO, string NO2, string nhietDo, string maDH, string maNV)
        {
            return hienTruongDao.InsertKhongKhiData(viTriLayMau, PM2dot5, CO, NO2, nhietDo, maDH, maNV);
        }

        public int CapNhatDuLieuKhongKhi(string viTriLayMau, string PM2dot5, string CO, string NO2, string nhietDo, string maDH, string maNV)
        {
            return hienTruongDao.UpdateKhongKhiData(viTriLayMau, PM2dot5, CO, NO2, nhietDo, maDH, maNV);
        }

        public DataTable LayThongTinKhiThai(string viTri, string maDH)
        {
            return hienTruongDao.GetKhiThaiByViTriAndMaDH(viTri, maDH);
        }

        public DataTable LayChiTieuKhiThai(string viTri, string maDH)
        {
            return hienTruongDao.GetChiTieuKhiThaiByViTriAndMaDH(viTri, maDH);
        }

        public bool ThemMoiKhiThai(string viTri, float so2, float no2, float pm, float nhietDo, string maNV, string maDH)
        {
            return hienTruongDao.InsertKhiThai(viTri, so2, no2, pm, nhietDo, maNV, maDH) > 0;
        }

        public bool CapNhatKhiThai(string viTri, float so2, float no2, float pm, float nhietDo, string maNV, string maDH)
        {
            return hienTruongDao.UpdateKhiThai(viTri, so2, no2, pm, nhietDo, maNV, maDH) > 0;
        }

        public string SoSanhChiTieu(float giaTriThucTe, float nguongChiTieu, string tenChiTieu)
        {
            if (giaTriThucTe > nguongChiTieu)
            {
                float vuotPhanTram = ((giaTriThucTe - nguongChiTieu) / nguongChiTieu) * 100;
                if (vuotPhanTram > 5)
                {
                    return $"Vượt ngưỡng {vuotPhanTram:F2}% ({giaTriThucTe} > {nguongChiTieu})";
                }
            }
            else if (giaTriThucTe < nguongChiTieu)
            {
                float thapPhanTram = ((nguongChiTieu - giaTriThucTe) / nguongChiTieu) * 100;
                if (thapPhanTram > 5)
                {
                    return $"Thấp ngưỡng {thapPhanTram:F2}% ({giaTriThucTe} < {nguongChiTieu})";
                }
            }
            return ""; // Giá trị hợp lệ hoặc không cần thông báo
        }
    }
}
