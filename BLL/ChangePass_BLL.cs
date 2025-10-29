using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Microsoft.SqlServer.Server;
namespace BLL
{
    public class ChangePass_BLL
    {
        ChangePass_DAO changepass;
        public ChangePass_BLL() { changepass= new ChangePass_DAO(); }

        public int check_manv(string manv, string mk_cu)
        {
            return this.changepass.check_manv(manv, mk_cu);
        }

        public void update_mk(string mk, string manv)
        {
            this.changepass.update_mk(mk, manv); 
        }
    }
}
