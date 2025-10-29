using BLL;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace EcoProject.FogetPassword
{

    public partial class doi_mat_khau : Form
    {
        ChangePass_BLL changepass;
        public string MaNV { get; set; } // Thuộc tính lưu mã nhân viên
        public dashboard dashboard { get; set; }
        public doi_mat_khau()
        {
            changepass = new ChangePass_BLL();
            InitializeComponent();

        }
        public doi_mat_khau(string MaNV, dashboard Dashboard) : this()
        {
            this.dashboard = Dashboard;
            this.MaNV = MaNV;
            this.WindowState = FormWindowState.Normal;
            this.TopMost = true;
            this.Show();
        }

        private void Btn_Return(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Btn_Mini(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;

        }

        private void Btn_Doi_Mat_Khau_Click(object sender, EventArgs e)
        {
            
            int result = changepass.check_manv(this.MaNV, TB_Mat_Khau_Cu.Text);

            //trả về 1 nếu tồn tại mã nhân viên cùng với mật khẩu được nhập trong khung textbox
            if (result == 1)
            {

                if (TB_Mat_Khau_Moi.Text == TB_Xac_Nhan_Mat_Khau.Text)
                {

                    changepass.update_mk(TB_Mat_Khau_Moi.Text, this.MaNV);
                    //TB_Mat_Khau_Cu.Clear();
                    //TB_Mat_Khau_Moi.Clear();
                    //TB_Xac_Nhan_Mat_Khau.Clear();

                    //lbl_check_valid.Visible = true;
                    //lbl_check_valid.Text = "Đổi mật khẩu thành công";
                    MessageBox.Show("Đổi mật khẩu thành công!");
                    this.dashboard.kiem_tra_ly_do = true;
                    Properties.Settings.Default.password = "";
                    Properties.Settings.Default.Save();
                    this.dashboard.Close();
                    this.Close();
                }
                else
                {
                    //MessageBox.Show("Mật khẩu xác nhận lại không chính xác");
                    lbl_check_valid.Visible = true;
                    lbl_check_valid.Text = "Mật khẩu xác nhận lại không chính xác";
                }
            }
            else
            {
                //MessageBox.Show("Sai mật khẩu cũ");
                lbl_check_valid.Visible = true;
                lbl_check_valid.Text = "Sai mật khẩu cũ";
            }
        }

        private void guna2CirclePictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {
            guna2Panel1.BackColor = Color.FromArgb(125, Color.White);

        }
    }
}
