using EcoProject.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class FogetPass_DAO
    {
        DataProvider dp;
        public FogetPass_DAO() { dp=new DataProvider(); }

        public int check_email(string email)
        {
            string query = "SELECT COUNT(*) FROM NhanVien WHERE Email = @Email";
            int result = (int)dp.ExecuteScalar(query, new object[] { email });
            return result;
        }
        public void update_matkhau(string matkhau, string email) 
        {
            string query_update = "update NhanVien set MatKhau = @MatKhauMoi where Email = @Email";

            dp.ExecuteNonQuery(query_update, new object[] { matkhau,email });
        }
    }
}
