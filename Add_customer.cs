using EcoProject.user_control;
using System;
using System.Windows.Forms;
using BLL;

namespace EcoProject
{
    public partial class Add_customer : Form
    {
        public Add_customer()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Normal;
            this.TopMost = true;
            this.Show();
        }

        private void Btn_Them_Khach_Hang_Click(object sender, EventArgs e)
        {
            QuanLyKhachHang_BLL qlkh = new QuanLyKhachHang_BLL();
            string email = TextMail.Text;
            string sdt = TextSDT.Text;

            if (string.IsNullOrWhiteSpace(TextTenCT.Text) || string.IsNullOrWhiteSpace(TextNguoiDD.Text) 
                || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(sdt) 
                || string.IsNullOrWhiteSpace(TextDiaChi.Text) || string.IsNullOrWhiteSpace(TextNganhCN.Text))
            {
                label_loi_tt.Visible = true;
            }
            else
            {
                label_loi_tt.Visible = false;
                bool isEmailValid = qlkh.KiemTraEmail(email);
                bool isSDTValid = qlkh.KiemTraSDT(sdt);

                label_loi_email.Visible = false;
                label_loi_sdt.Visible = false;

                if (!isEmailValid && !isSDTValid)
                {
                    label_loi_email.Visible = true;
                    label_loi_sdt.Visible = true;
                    TextMail.Focus();
                }
                else if (!isEmailValid)
                {
                    label_loi_email.Visible = true;
                    TextMail.Focus();
                }
                else if (!isSDTValid)
                {
                    label_loi_sdt.Visible = true;
                    TextSDT.Focus();
                }
                else
                {
                    int row = qlkh.them_khach_hang( TextNguoiDD.Text, TextTenCT.Text, email, TextDiaChi.Text, TextNganhCN.Text, sdt, TextGhiChu.Text );

                    if (row == 2)
                    {
                        MessageBox.Show("Khách hàng mới đã được thêm!");
                        TextTenCT.Clear();
                        TextNguoiDD.Clear();
                        TextMail.Clear();
                        TextSDT.Clear();
                        TextDiaChi.Clear();
                        TextNganhCN.Clear();
                        TextGhiChu.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Lỗi khi thêm khách hàng!");
                    }
                }
            }
        }

        private void guna2CircleButton3_Click(object sender, EventArgs e)
        {
            uc_customer uc_add_customer = new uc_customer();    
            ((dashboard)this.ParentForm).ShowUserControlOnPanel(uc_add_customer);
        }
    }
}
