using System.Data;
using DAL;

namespace BLL
{
    public class QuanLyPhongThiNghiem_BLL
    {
        private QuanLyPhongThiNghiem_DAO dao = new QuanLyPhongThiNghiem_DAO();

        public DataTable LayDuLieuNuocMat(string viTriLayMau, string maDH)
        {
            return dao.LayDuLieuNuocMat(viTriLayMau, maDH);
        }

        public bool CapNhatTrangThaiMau(string maDH, string viTriLayMau)
        {
            return dao.CapNhatTrangThaiMau(maDH, viTriLayMau) > 0;
        }

        public bool CapNhatDuLieuNuocMat(object[] parameters)
        {
            return dao.CapNhatDuLieuNuocMat(parameters) > 0;
        }

        public DataTable LayDuLieuKhongKhi(string maDH, string viTriLayMau)
        {
            return dao.GetKhongKhiData(maDH, viTriLayMau);
        }

        public DataTable LayChiTieuKhongKhi(string maDH, string viTriLayMau)
        {
            return dao.GetChiTieuKhongKhi(maDH, viTriLayMau);
        }

        public DataTable LayChiTieuNuocMat(string maDH, string viTriLayMau)
        {
            return dao.GetChiTieuNuocMat(maDH, viTriLayMau);
        }

        public bool CapNhatDuLieuKhongKhi(string maDH, string viTriLayMau, float pm10, float so2, float o3)
        {
            int result = dao.UpdateKhongKhi(maDH, viTriLayMau, pm10, so2, o3);
            return result > 0;
        }

        public DataTable LayDuLieuKhiThai(string viTriLayMau, string maDH)
        {
            return dao.GetKhiThai(viTriLayMau, maDH);
        }

        public DataTable LayChiTieuKhiThai(string viTriLayMau, string maDH)
        {
            return dao.GetChiTieuKhiThai(viTriLayMau, maDH);
        }

        public bool CapNhatKhiThai(string maDH, string viTriLayMau, object[] values)
        {
            int result = dao.UpdateKhiThai(maDH, viTriLayMau, values);
            return result > 0;
        }
    }
}
