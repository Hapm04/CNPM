using System;
using System.Collections.Generic;
using System.Data;
using DAL;

namespace BLL
{
    public class QuanLyThongBao_BLL
    {
        QuanLyThongBao_DAO quan_ly_thong_bao ;

        public QuanLyThongBao_BLL()
        {
            quan_ly_thong_bao = new QuanLyThongBao_DAO();
        }

        public DataTable lay_du_lieu()
        {
            return this.quan_ly_thong_bao.lay_du_lieu();
        }

        public DataTable qua_han()
        {
            return this.quan_ly_thong_bao.qua_han();
        }

        public DataTable sap_qua_han()
        {
            return this.quan_ly_thong_bao .sap_qua_han();
        }
    }
}
