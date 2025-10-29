using System;
using System.Windows.Forms;
using EcoProject.user_control;
using BLL;

namespace EcoProject
{
    public partial class Add_Orders : Form
    {
        QuanLyDonHang_BLL quanly;
        public Add_Orders()
        {
            InitializeComponent();
            quanly = new QuanLyDonHang_BLL();
            this.WindowState = FormWindowState.Normal;
            this.TopMost = true;
            this.Show();
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

            string makh = this.quanly.kiem_tra_makh(tenkhachhang.Text);

            if (makh == null)
            {
                lbl_error_khach_hang.Visible = true;
                tenkhachhang.Focus();
                return;
            }
            else
            {
                lbl_error_khach_hang.Visible = false;
            }

            DateTime ngaytraketqua = DateTime.Parse(traketqua.Text);
            DateTime dathang = DateTime.Parse(ngaydathang.Text);
            TimeSpan day = ngaytraketqua.Subtract(dathang);

            if (day.Days < 10 || day.Days > 15)
            {
                lbl_error_ngaytra.Visible = true;
                traketqua.Focus();
                return;
            }
            else
            {
                lbl_error_ngaytra.Visible = false;
            }

           
            int ket_qua = this.quanly.them_don_hang(traketqua.Value, ngaydathang.Value, makh, quy.Text);
            
            if (ket_qua == 1)
            {
                MessageBox.Show("Bạn đã thêm đơn hàng thành công!");
                uc_order uc_Order = new uc_order();
                ((dashboard)this.ParentForm).ShowUserControlOnPanel(uc_Order);
                this.Close();
            }
            MessageBox.Show("Đã có lỗi xảy ra khi thêm đơn hàng mới!");
            return;
        }

        private void btn_quay_lai_Click(object sender, EventArgs e)
        {
            uc_order uc_Order = new uc_order();
            ((dashboard)this.ParentForm).ShowUserControlOnPanel(uc_Order);
            this.Close();
        }
    }
}
