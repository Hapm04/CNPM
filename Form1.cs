using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using BLL;


namespace EcoProject
{
    public partial class Form1 : Form
    {
        QuanLyTaiKhoan_BLL form1;
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        public Form1()
        {
            form1 = new QuanLyTaiKhoan_BLL();
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += Worker_Completed;
            worker.RunWorkerAsync();
            TK_DangNhap.Text = Properties.Settings.Default.username;
            MK_DangNhap.Text = Properties.Settings.Default.password;
            if (Properties.Settings.Default.username != "")
            {
                checkBox1.Checked = true;
            }
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            HeavyTask();
        }

        private void Worker_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            UpdateUI();
        }

        private void HeavyTask()
        {
            System.Threading.Thread.Sleep(1); // Giả lập công việc mất thời gian
        }

        private void UpdateUI()
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

            //int result = (int)dataProvider.ExecuteScalar(query, new object[] { TK_DangNhap.Text, MK_DangNhap.Text });
            int result = (int)form1.check_tk_mk(TK_DangNhap.Text, MK_DangNhap.Text);

            //trả về 1 nếu tồn tại mã nhân viên cùng với mật khẩu được nhập trong khung textbox
            if (result == 1)
            {
                //string hung_manv = dataProvider.ExecuteScalar(query1, new object[] { TK_DangNhap.Text }).ToString();
                if (checkBox1.Checked)
                {
                    Properties.Settings.Default.username = TK_DangNhap.Text;
                    Properties.Settings.Default.password = MK_DangNhap.Text;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    Properties.Settings.Default.Reset();
                }
                
                string hung_manv = form1.tim_manv(TK_DangNhap.Text);
                this.Hide();
                dashboard dashboard = new dashboard(hung_manv);
                SessionInfo.MaNV = hung_manv; // Thay thế bằng mã nhân viên thực tế
                //SessionInfo.TenNV = ;

                dashboard.Show();
            }
            else
            {
                check_mk.Text = "Sai tài khoản hoặc mật khẩu";
                check_mk.Visible = true;
            }
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2CircleButton2_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void guna2Button2_Click_1(object sender, EventArgs e)
        {
            FogetPassword.FogetPass fogetPass = new FogetPassword.FogetPass();
            fogetPass.Show();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }


        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

            guna2Panel1.BackColor = Color.FromArgb(125, Color.White);

        }

        private void guna2CirclePictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
    public static class SessionInfo
    {
        public static string MaNV { get; set; }
        public static string TenNV { get; set; }

    }
}