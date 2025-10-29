using System;
using System.Globalization;
using System.Windows.Forms;
using BLL;
using EcoProject.user_control;

namespace EcoProject
{
    public partial class Update_content : Form
    {
        QuanLyDonHang_BLL quanly;
        string mdh;

        public Update_content(string mdh, string hantra, string ngayky, string tencongty, string quy)
        {
            try
            {
                InitializeComponent();
                quanly = new QuanLyDonHang_BLL();
                this.mdh = mdh;
                DateTime trakq = DateTime.ParseExact(hantra, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                traketqua.Value = DateTime.Parse(trakq.ToString("MM/dd/yyyy"));
                DateTime ngaydat = DateTime.ParseExact(ngayky, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                ngaydathang.Value = DateTime.Parse(ngaydat.ToString("MM/dd/yyyy"));
                tenkhachhang.Text = tencongty;
                cbx_quy.Text = quy;
                this.WindowState = FormWindowState.Normal;
                this.TopMost = true;
                this.Show();
            }
            catch (Exception ex) { MessageBox.Show($"{ex.Message}"); }
        }

        private void ThemDonHang(object sender, EventArgs e)
        {
            foreach (Control item in panelOrder.Controls)
            {
                if (item.Text == "")
                {
                    lbl_error_lack_of_inf.Visible = true;
                    item.Focus();
                    return;
                }
            }
            DateTime ngaytraketqua = DateTime.Parse(traketqua.Text);
            DateTime dathang = DateTime.Parse(ngaydathang.Text);
            TimeSpan day = ngaytraketqua.Subtract(dathang);
            while (true)
            {
                if (day.Days < 10 || day.Days > 15)
                {
                    lbl_error_ngaytra.Visible = true;
                    traketqua.Focus();
                    return;
                }
                break;
            }
         
            string makh = this.quanly.kiem_tra_makh(tenkhachhang.Text);
            if (makh == null)
            {
                lbl_error_ten_cong_ty.Visible = true;
                return;
            }
            Boolean check = this.quanly.cap_nhat_don_hang(traketqua.Value, ngaydathang.Value, makh, cbx_quy.Text, this.mdh);
            if (check == true)
            {
                MessageBox.Show("Bạn đã cập nhật đơn hàng thành công!");
                uc_order uc_Order = new uc_order();
                ((dashboard)this.ParentForm).ShowUserControlOnPanel(uc_Order);
                this.Close();
            }
            else
            {
                MessageBox.Show("Không thể cập nhật đơn hàng!");
            }
        }

        private void btn_quay_lai_Click(object sender, EventArgs e)
        {
            uc_order uc_Order = new uc_order();
            ((dashboard)this.ParentForm).ShowUserControlOnPanel(uc_Order);
            this.Close();
        }
    }
}
