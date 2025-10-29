using EcoProject.DAO;
using System;
using System.Data;


namespace DAL
{
    public class Home_DAO
    {
        DataProvider dp;
        public Home_DAO() { dp = new DataProvider(); }


        public DataTable lay_du_lieu_KhongKhi()
        {
            try
            {
                string khongkhi_match_chitieu = "SELECT MaDH, ViTriLayMau FROM KhongKhi";

                DataTable khongkhi_matching = dp.ExecuteQuery(khongkhi_match_chitieu, new object[] { });
                return khongkhi_matching;
            }
            catch { return null; }
        }
        public DataTable khong_khi_data(DateTime current, DateTime pre, DataTable khong_khi_matching)
        {
            string khongKhiQuery = "SELECT CO, SO2, O3, PM10, PM2dot5, NhietDo, NO2 FROM KhongKhi " +
                                       "JOIN Mau ON KhongKhi.ViTriLayMau = Mau.ViTriLayMau " +
                                       "JOIN DonHang ON KhongKhi.MaDH = DonHang.MaDH " +
                                       "WHERE Mau.TrangThai = N'Đã hoàn thành' AND DonHang.HanTraHang <= @currentDate AND DonHang.HanTraHang >= @previous";


            if (khong_khi_matching.Rows.Count > 0)
            {
                return dp.ExecuteQuery(khongKhiQuery, new object[] { current, pre });

            }
            return null;
        }
        public DataTable chi_tieu_khong_khi(DataTable khong_khi_matching)
        {
            string chiTieuQuery = "SELECT CO, SO2, O3, PM10, PM2dot5, NhietDo, NO2 FROM ChiTieuKK " +
                                      "WHERE ViTriLayMau = @vitri AND MaDH = @madh";
            DataRow madh_mc = khong_khi_matching.Rows[0]; // Match the first row
            DataRow vitri_mc = khong_khi_matching.Rows[0]; // Match the first row
            if (khong_khi_matching.Rows.Count > 0)
            {
                return dp.ExecuteQuery(chiTieuQuery, new object[] { vitri_mc["ViTriLayMau"], madh_mc["MaDH"] });
            }
            return null;
        }


        public DataTable lay_du_lieu_NuocMat()
        {
            try
            {
                // SQL queries to retrieve data from both tables
                string nuocmat_match_chitieu = "SELECT MaDH, ViTriLayMau FROM NuocMat";



                // Retrieve matching data for ViTriLayMau and MaDH
                DataTable nuocmat_matching = dp.ExecuteQuery(nuocmat_match_chitieu);
                return nuocmat_matching;
            }
            catch { return null; }
        }
        public DataTable nuoc_mat_data(DateTime current, DateTime pre, DataTable nuoc_mat_matching)
        {
            string nuocMatQuery = "SELECT TDS, NhietDo, pH, PO4, NH4, tongP, tongN, TOC, TSS, COD, DO, NO3 FROM NuocMat " +
                                      "JOIN Mau ON NuocMat.ViTriLayMau = Mau.ViTriLayMau " +
                                      "JOIN DonHang ON NuocMat.MaDH = DonHang.MaDH " +
                                      "WHERE Mau.TrangThai = N'Đã hoàn thành' AND DonHang.HanTraHang <= @currentDate AND DonHang.HanTraHang >= @previous";


            if (nuoc_mat_matching.Rows.Count > 0)
            {
                DataTable nuocMatData = dp.ExecuteQuery(nuocMatQuery, new object[] { current, pre });
                return nuocMatData;

            }
            return null;
        }
        public DataTable chi_tieu_nuoc_mat(DataTable nuocmat_matching)
        {
            string chiTieuQuery = "SELECT TDS, NhietDo, pH, PO4, NH4, tongP, tongN, TOC, TSS, COD, DO, NO3 " +
                                      "FROM ChiTieuNuocMat WHERE ViTriLayMau = @vitri AND MaDH = @madh";
            DataRow madh_mc = nuocmat_matching.Rows[0]; // Match the first row
            DataRow vitri_mc = nuocmat_matching.Rows[0]; // Match the first row
            if (nuocmat_matching.Rows.Count > 0)
            {
                DataTable chiTieuData = dp.ExecuteQuery(chiTieuQuery, new object[] { vitri_mc["ViTriLayMau"], madh_mc["MaDH"] });
                return chiTieuData;
            }
            return null;
        }

        public DataTable lay_du_lieu_KhiThai()
        {
            try
            {
                // SQL queries to retrieve data from both tables
                string khithai_match_chitieu = "SELECT MaDH, ViTriLayMau FROM KhiThai";


                // Retrieve matching data for ViTriLayMau and MaDH
                DataTable khithai_matching = dp.ExecuteQuery(khithai_match_chitieu);

                return khithai_matching;
            }
            catch
            {
                return null;
            }
        }

        public DataTable khi_thai_data(DateTime current, DateTime pre, DataTable khithai_matching)
        {
            string khiThaiQuery = "SELECT CO, NhietDo, NO2, O2, Hg, PM, NH3, N_O, ApSuat, SO2, H2S FROM KhiThai " +
                                      "JOIN Mau ON KhiThai.ViTriLayMau = Mau.ViTriLayMau " +
                                      "JOIN DonHang ON KhiThai.MaDH = DonHang.MaDH " +
                                      "WHERE Mau.TrangThai = N'Đã hoàn thành' AND DonHang.HanTraHang <= @currentDate AND DonHang.HanTraHang >= @previous";


            if (khithai_matching.Rows.Count > 0)
            {
                DataTable khiThaiData = dp.ExecuteQuery(khiThaiQuery, new object[] { current, pre });
                return khiThaiData;
            }
            return null;
        }
        public DataTable chi_tieu_khi_thai(DataTable khithai_matching)
        {
            string chiTieuQuery = "SELECT CO, NhietDo, NO2, O2, Hg, PM, NH3, N_O, ApSuat, SO2, H2S " +
                              "FROM ChiTieuKhiThai WHERE ViTriLayMau = @vitri AND MaDH = @madh";
            DataRow madh_mc = khithai_matching.Rows[0]; // Match the first row
            DataRow vitri_mc = khithai_matching.Rows[0]; // Match the first row
            if (khithai_matching.Rows.Count > 0)
            {
                DataTable chiTieuData = dp.ExecuteQuery(chiTieuQuery, new object[] { vitri_mc["ViTriLayMau"], madh_mc["MaDH"] });
                return chiTieuData;
            }
            return null;
        }
    }
}
