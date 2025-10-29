using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DAL;
using System.Security.Policy;

namespace BLL
{
    public class QuanLyChiTieu_BLL
    {
        private QuanLyChiTieu_DAO dao = new QuanLyChiTieu_DAO();

        public bool ValidateChiTieu(Dictionary<string, float> chiTieu, out string errorMessage)
        {
            foreach (var item in chiTieu)
            {
                if (item.Value < 0)
                {
                    errorMessage = $"{item.Key} không được nhỏ hơn 0.";
                    return false;
                }
            }
            errorMessage = null;
            return true;
        }

        public bool CompareChiTieu(DataTable chiTieuDB, Dictionary<string, float> chiTieu, float tolerance = 5f)
        {
            foreach (DataRow row in chiTieuDB.Rows)
            {
                foreach (var key in chiTieu.Keys)
                {
                    float giaTriThucTe = row[key] != DBNull.Value ? Convert.ToSingle(row[key]) : 0f;
                    float giaTriChuan = chiTieu[key];
                    float chenhlech = Math.Abs((giaTriThucTe - giaTriChuan) / giaTriChuan) * 100;

                    if (chenhlech > tolerance)
                        return false;
                }
            }
            return true;
        }

        public DataTable LayDuLieuNuocMat(string maDH, string viTriLayMau)
        {
            return dao.LayDuLieuNuocMat(maDH, viTriLayMau);
        }

        public DataTable LayDuLieuKhiThai(string maDH, string viTriLayMau)
        {
            return dao.LayDuLieuKhiThai(maDH, viTriLayMau);
        }

        public DataTable LayDuLieuKhongKhi(string maDH, string viTriLayMau)
        {
            return dao.LayDuLieuKhongKhi(maDH, viTriLayMau);
        }

        public void CapNhatKetQua(string maDH, string viTriLayMau, bool isQualified)
        {
            string ketQua = isQualified ? "Đạt yêu cầu" : "Không đạt yêu cầu";
            dao.CapNhatKetQuaMau(maDH, viTriLayMau, ketQua);
        }

        public DataTable LayChiTieuKhongKhi(string maDH, string viTriLayMau)
        {
            return dao.LayChiTieuKhongKhi(maDH, viTriLayMau);
        }

        public DataTable LayChiTieuKhiThai(string maDH, string viTriLayMau)
        {
            return dao.LayChiTieuKhiThai(maDH, viTriLayMau);
        }

        public DataTable LayChiTieuNuocMat(string maDH, string viTriLayMau)
        {
            return dao.LayChiTieuNuocMat(maDH, viTriLayMau);
        }

        public bool ThemChiTieuKhongKhi(string maDH, string viTriLayMau, string maNV, float pm2dot5, float co, float no2, float nhietDo, float pm10, float so2, float o3)
        {
            // Logic kiểm tra hợp lệ có thể thêm tại đây nếu cần
            return dao.ThemChiTieuKhongKhi(maDH, viTriLayMau, maNV, pm2dot5, co, no2, nhietDo, pm10, so2, o3) > 0;
        }

        public bool CapNhatChiTieuKhongKhi(string maDH, string viTriLayMau, float pm2dot5, float co, float no2, float nhietDo, float pm10, float so2, float o3)
        {
            // Logic kiểm tra hợp lệ có thể thêm tại đây nếu cần
            return dao.CapNhatChiTieuKhongKhi(maDH, viTriLayMau, pm2dot5, co, no2, nhietDo, pm10, so2, o3) > 0;
        }

        public bool CapNhatChiTieuNuocMat(string maDH, string viTriLayMau, Dictionary<string, float> chiTieu)
        {
            return dao.CapNhatChiTieuNuocMat(maDH, viTriLayMau, chiTieu) > 0;
        }

        public bool ThemChiTieuKhiThai(string viTriLayMau, string maDongHang, string maNV, float apSuat, float co, float h2s, float o2, float nh3, float hg, float no, float so2, float no2, float pm, float nhietDo)
        {
            int result = dao.ThemChiTieuKhiThai(viTriLayMau, maDongHang, maNV, apSuat, co, h2s, o2, nh3, hg, no, so2, no2, pm, nhietDo);
            return result > 0;
        }

        public bool ThemChiTieuNuocMat(string maDH, string viTriLayMau, string maNV, Dictionary<string, float> chiTieu)
        {
            int result = dao.ThemChiTieuNuocMat(maDH, viTriLayMau, maNV, chiTieu);
            return result > 0;
        }

        public bool CapNhatChiTieuKhiThai(string viTriLayMau, string maDongHang, float apSuat, float co, float h2s, float o2, float nh3, float hg, float no, float so2, float no2, float pm, float nhietDo)
        {
            int result = dao.CapNhatChiTieuKhiThai(viTriLayMau, maDongHang, apSuat, co, h2s, o2, nh3, hg, no, so2, no2, pm, nhietDo);
            return result > 0;
        }

        public DataTable LayChiTieuKhongKhiFull(string viTriLayMau, string maDongHang)
        {
            return dao.LayChiTieuKhongKhiFull(viTriLayMau, maDongHang);
        }

        public bool KiemTraDuLieu(float giaTriThucTe, float giaTriChuan, float saiSoChoPhep)
        {
            float chenhlech = Math.Abs((giaTriThucTe - giaTriChuan) / giaTriChuan) * 100;
            return chenhlech <= saiSoChoPhep;
        }
    }
}
