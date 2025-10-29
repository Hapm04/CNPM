using DAL;
using System;
using System.Data;

namespace BLL
{
    public class Home_BLL
    {
        Home_DAO home;
        public Home_BLL() { home = new Home_DAO(); }


        public DataTable lay_du_lieu_KhongKhi()
        {
            return this.home.lay_du_lieu_KhongKhi();
        }
        public DataTable khong_khi_data(DateTime current, DateTime pre, DataTable khongkhi_matching)
        {
            return this.home.khong_khi_data(current, pre, khongkhi_matching);
        }
        public DataTable chi_tieu_khong_khi(DataTable khongkhi_matching)
        {
            return this.home.chi_tieu_khong_khi(khongkhi_matching);
        }




        public DataTable lay_du_lieu_NuocMat()
        {
            return this.home.lay_du_lieu_NuocMat();
        }
        public DataTable nuoc_mat_data(DateTime current, DateTime pre, DataTable nuocmat_matching)
        {
            return this.home.nuoc_mat_data(current, pre, nuocmat_matching);
        }
        public DataTable chi_tieu_nuoc_mat(DataTable nuocmat_matching)
        {
            return this.home.chi_tieu_nuoc_mat(nuocmat_matching);
        }




        public DataTable lay_du_lieu_KhiThai()
        {

            return this.home.lay_du_lieu_KhiThai();
        }
        public DataTable khi_thai_data(DateTime current, DateTime pre, DataTable khithai_matching)
        {
            return this.home.khi_thai_data(current, pre, khithai_matching);
        }
        public DataTable chi_tieu_khi_thai(DataTable khithai_matching)
        {
            return this.home.chi_tieu_khi_thai(khithai_matching);
        }

    }

}