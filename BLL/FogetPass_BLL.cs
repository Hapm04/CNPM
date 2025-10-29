using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class FogetPass_BLL
    {
        FogetPass_DAO forgetpass;
        public FogetPass_BLL() { forgetpass= new FogetPass_DAO(); }

        public int check_email(string email)
        {
            return this.forgetpass.check_email(email);
        }
        public void update_matkhau(string matkhau, string email)
        {
            this.forgetpass.update_matkhau(matkhau, email);
        }
    }
}
