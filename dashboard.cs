using EcoProject.user_control;
using System;
using System.Drawing;
using System.Windows.Forms;
using BLL;

namespace EcoProject
{
    public partial class dashboard : Form
    {
        private int MinWidth;
        private int MinHeight;

        private string MaNV;

        public bool kiem_tra_ly_do = false;
        public string MaNV1 { get => MaNV; set => MaNV = value; }
        private Form currentForm;
        QuanLyTaiKhoan_BLL quan_ly_tk;

        public dashboard()
        {
            InitializeComponent();
            quan_ly_tk = new QuanLyTaiKhoan_BLL();
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Resize += Dashboard_Resize;
            SetMinimumSize();
        }
        private void SetMinimumSize()
        {
            // Get the screen size
            Rectangle screenSize = Screen.PrimaryScreen.WorkingArea;

            // Set the minimum width and height to half the screen size
            MinWidth = screenSize.Width / 2;
            MinHeight = screenSize.Height / 2;
        }

        private void Dashboard_Resize(object sender, EventArgs e)
        {
            if (this.Width < MinWidth)
            {
                this.Width = MinWidth;
            }

            if (this.Height < MinHeight)
            {
                this.Height = MinHeight;
            }
        }
        //
        public dashboard(string MaNV) : this() // Constructor nhận mã nhân viên
        {
            this.MaNV = MaNV;
            // Cho phép thay đổi kích thước
            this.WindowState = FormWindowState.Normal;
            this.TopMost = false;
            this.Show();
            //this.Controls.Add();
            
            string hoten = quan_ly_tk.lay_ten_nv(MaNV);
            ListBoxTenNv.Text = hoten;
            guna2ComboBox1.Items.Clear();
            guna2ComboBox1.Items.Add("Đổi Mật Khẩu");
            guna2ComboBox1.Items.Add("Đăng Xuất");

            guna2ComboBox1.SelectedIndexChanged += guna2ComboBox1_SelectedIndexChanged;
        }
        public void ShowFormOnPanel(Form form)
        {
            // Kiểm tra nếu đã có form hiện tại đang hiển thị trên panel, thì đóng form đó trước khi hiển thị form mới
            if (currentForm != null)
            {
                currentForm.Close();
            }

            // Thiết lập form mới để hiển thị
            currentForm = form;
            currentForm.TopLevel = false;
            //currentForm.FormBorderStyle = FormBorderStyle.Sizable;
            currentForm.Dock = DockStyle.Fill;

            // Thêm form vào panel control (panel1)
            guna2Panel2.Controls.Clear(); // Xóa các control cũ trên panel
            guna2Panel2.Controls.Add(currentForm);
            guna2Panel2.Tag = currentForm;


            // Hiển thị form
            //currentForm.Size = guna2Panel2.Size; // Scale form size to match panel1 size
            currentForm.Show();
            currentForm.BringToFront();
        }

        public void ShowUserControlOnPanel(System.Windows.Forms.UserControl userControl)
        {
            // Kiểm tra nếu đã có form hiện tại đang hiển thị trên panel, thì đóng form đó trước khi hiển thị form mới
            if (currentForm != null)
            {
                currentForm.Close();
            }

            // Thêm UserControl vào panel
            userControl.Dock = DockStyle.Top;
            guna2Panel2.Controls.Clear();
            guna2Panel2.Controls.Add(userControl);
            userControl.Tag = currentForm;
            //userControl.Size = guna2Panel2.Size;
            userControl.Show();

            userControl.BringToFront();

        }



        private void TrangChu(object sender, EventArgs e)
        {
            uc_home newLab = new uc_home();
            this.ShowUserControlOnPanel(newLab);
        }


        private void QuanLiDonHang(object sender, EventArgs e)
        {
            uc_order newLab = new uc_order();
            this.ShowUserControlOnPanel(newLab);
        }

        private void PhanTichMau(object sender, EventArgs e)
        {
            uc_lab_analysis_management1 newLab = new uc_lab_analysis_management1();
            this.ShowUserControlOnPanel(newLab);
        }

        private void LichSu(object sender, EventArgs e)
        {
            uc_historyand_udit_log newLab = new uc_historyand_udit_log();
            this.ShowUserControlOnPanel(newLab);
        }

        private void PhieuTraHang(object sender, EventArgs e)
        {
            uc_reports newLab = new uc_reports();
            this.ShowUserControlOnPanel(newLab);
        }

        private void ThongBao(object sender, EventArgs e)
        {
            uc_notifications newLab = new uc_notifications();
            this.ShowUserControlOnPanel(newLab);
        }

        private void dashboard_Load(object sender, EventArgs e)
        {
            uc_home newLab = new uc_home();
            this.ShowUserControlOnPanel(newLab);
        }

        private void QuanLiKhachHang(object sender, EventArgs e)
        {
            uc_customer newLab = new uc_customer();
            this.ShowUserControlOnPanel(newLab);
        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            guna2ComboBox1.Items.Clear();

            guna2ComboBox1.Items.Add("Đổi Mật Khẩu");
            guna2ComboBox1.Items.Add("Đăng Xuất");
        }


        private void DangXuatDoiMatKhau(object sender, EventArgs e)
        {
            if (guna2ComboBox1.SelectedItem.ToString() == "Đổi Mật Khẩu")
            {
                FogetPassword.doi_mat_khau changePass = new FogetPassword.doi_mat_khau(MaNV, this);
                changePass.Show();
            }
            else if (guna2ComboBox1.SelectedItem.ToString() == "Đăng Xuất")
            {
                this.Hide();
                Form1 form = new Form1();
                form.ShowDialog();
            }
        }

        private void dashboard_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (kiem_tra_ly_do)
            {
                Form1 form = new Form1();
                form.Show();
            }
            else if (e.CloseReason == CloseReason.UserClosing)
            {
                Application.Exit();
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
