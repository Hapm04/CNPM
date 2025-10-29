using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcoProject.DAO;
namespace DAL
{
    public class ChangePass_DAO
    {
        DataProvider dp;
        public ChangePass_DAO() { dp=new DataProvider(); }

        public int check_manv(string manv, string mk_cu)
        {
            string query = "SELECT COUNT(*) FROM NhanVien WHERE MaNV = @MaNV AND MatKhau = @mk_cu ";
            return (int)dp.ExecuteScalar(query, new object[] { manv , mk_cu });
        }

        public void update_mk(string mk, string manv)
        {
            string query_update = "update NhanVien set MatKhau = @MatKhauMoi where MaNV = @MaNV";
            dp.ExecuteNonQuery(query_update, new object[] { mk, manv });
        }
    }
}
