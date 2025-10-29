using System;
using System.Windows.Forms;
using BLL;
using EcoProject.user_control;

namespace EcoProject
{
    public partial class Update_customer : Form
    {
        public string ma_kh { get; set; }
        public string nguoi_dd { get; set; }
        public string ten_ct { get; set; }
        public string email { get; set; }
        public string dia_chi { get; set; }
        public string nganh_cn { get; set; }
        public string sdt { get; set; }
        public string ghi_chu { get; set; }

        public Update_customer(string ma_kh, string nguoi_dd, string ten_ct, string email, 
                            string dia_chi, string nganh_cn, string sdt, string ghi_chu)
        {
            InitializeComponent();
            this.ma_kh = ma_kh;
            this.nguoi_dd = nguoi_dd;
            this.ten_ct = ten_ct;
            this.email = email;
            this.dia_chi = dia_chi;
            this.nganh_cn = nganh_cn;
            this.sdt = sdt;
            this.ghi_chu = ghi_chu;
            this.WindowState = FormWindowState.Normal;
            this.TopMost = true;
            this.Show();
        }

        private void btn_thoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_thu_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btn_suaKH_Click(object sender, EventArgs e)
        {
            string email = TextMail.Text;
            string sdt = TextSDT.Text;
            QuanLyKhachHang_BLL qlkh = new QuanLyKhachHang_BLL();

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
                    int row = qlkh.sua_thong_tin_khach_hang( TextNguoiDD.Text, TextTenCT.Text, email, TextDiaChi.Text, TextNganhCN.Text, sdt, TextGhiChu.Text, ma_kh);

                    if (row == 1)
                    {
                        MessageBox.Show("Thông tin khách hàng đã được sửa thành công!");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Lỗi khi sửa thông tin khách hàng!");
                    }
                }
            }
        }

        private void Update_customer_Load(object sender, EventArgs e)
        {
            TextNguoiDD.Text = nguoi_dd;
            TextTenCT.Text = ten_ct;
            TextMail.Text = email;
            TextDiaChi.Text = dia_chi;
            TextNganhCN.Text = nganh_cn;
            TextSDT.Text = sdt;
            TextGhiChu.Text = ghi_chu;
        }

        private void btn_quay_lai_Click(object sender, EventArgs e)
        {
            uc_customer uc_Customer = new uc_customer();
            ((dashboard)this.ParentForm).ShowUserControlOnPanel(uc_Customer);
            this.Close();
        }
    }
}
