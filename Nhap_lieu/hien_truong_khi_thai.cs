using EcoProject.user_control;
using Guna.UI2.WinForms;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using BLL;

namespace EcoProject.Nhap_lieu
{
    public partial class hien_truong_khi_thai : Form
    {
        LichSu_BLL lich_su;
        QuanLyHienTruong_BLL QuanLyHienTruong_BLL;

        public hien_truong_khi_thai(string selectedLoaiMau, string selectedMaDonHang, bool IsEditMode, string currentViTriLayMau)
        {
            InitializeComponent();
            lich_su = new LichSu_BLL();
            QuanLyHienTruong_BLL = new QuanLyHienTruong_BLL();
            loai_mau = selectedLoaiMau;
            maDonHang = selectedMaDonHang;
            edit = IsEditMode;
            viTri = currentViTriLayMau;
        }
        
        public string maDonHang { get ; set; }
        public string loai_mau { get; set; }
        public bool edit { get; set; }
        public string viTri { get; set; }


        private void guna2HtmlLabel3_Click(object sender, EventArgs e)
        {

        }

        private bool ValidateInputs()
        {
            // Kiểm tra dữ liệu rỗng
            if (!ValidateIsNullOrEmpty(TB_SO2)||
                !ValidateIsNullOrEmpty(BoxNO2) ||
                !ValidateIsNullOrEmpty(BoxPM) ||
                !ValidateIsNullOrEmpty(TB_nhiet_do))
            {
                //errorMess.Text = "Vui lòng nhập đầy đủ dữ liệu!";
                //errorMess.Visible = true;

                return false;
            }

            // Kiểm tra giá trị từng TextBox
            if (
            !ValidateNumber(TB_SO2, 0, 1000, "SO2 phải nằm trong khoảng từ 0 đến 1000 µg/Nm³!") ||
            !ValidateNumber(BoxNO2, 0, 500, "NO2 phải nằm trong khoảng từ 0 đến 500 µg/Nm³!") ||
            !ValidateNumber(BoxPM, 0, 500, "Bụi PM phải nằm trong khoảng từ 0 đến 500 µg/Nm³!") ||
            !ValidateNumber(TB_nhiet_do, 0, 80, "Nhiệt độ phải nằm trong khoảng từ 0 đến 80°C!")
                )
            {
                return false;
            }

            // Nếu không có lỗi
            errorMess.Visible = false;
            return true;
        }

        private bool ValidateIsNullOrEmpty(Guna2TextBox textBox)
        {
            if(string.IsNullOrEmpty(textBox.Text))
            {
                errorMess.Text = "Vui lòng nhập đầy đủ các trường dữ liệu!";
                errorMess.Visible = true;
                textBox.Focus();
                textBox.BorderColor = Color.Red;
                return false;
            }
            // Đặt lại màu viền nếu hợp lệ
            textBox.BorderColor = Color.FromArgb(60, 202, 144);
            return true;
        }

        private bool ValidateNumber(Guna2TextBox textBox, float minValue, float maxValue, string errorMessage)
        {
            if (!float.TryParse(textBox.Text, out float value) || value < minValue || value > maxValue)
            {
                errorMess.Text = errorMessage;
                errorMess.Visible = true;
                textBox.Focus();
                textBox.BorderColor = Color.Red;
                return false;
            }

            // Đặt lại màu viền nếu hợp lệ
            textBox.BorderColor = Color.FromArgb(60, 202, 144);
            errorMess.Visible = false; // Ẩn thông báo lỗi nếu hợp lệ
            return true;
        }

        private void savechange_Click(object sender, EventArgs e)
        {
            try
            {
                // Gọi hàm ValidateInputs để kiểm tra tất cả các textbox
                if (!ValidateInputs())
                {
                    return; // Ngừng xử lý nếu có lỗi
                }

                string nhan_vien = SessionInfo.MaNV;

                if (!edit)
                {
                        this.QuanLyHienTruong_BLL.ThemMau(vi_tri_lay_mau.Text, maDonHang, nhan_vien, loai_mau);

                        this.QuanLyHienTruong_BLL.ThemMoiKhiThai(vi_tri_lay_mau.Text, float.Parse(TB_SO2.Text), float.Parse(BoxNO2.Text), float.Parse(BoxPM.Text), float.Parse(TB_nhiet_do.Text), nhan_vien, maDonHang);

                        // Đóng form hiện tại
                        uc_lab_analysis_management1 uc_lab_analysis_management1 = new uc_lab_analysis_management1();
                        ((dashboard)this.ParentForm).ShowUserControlOnPanel(uc_lab_analysis_management1);
                }

                else
                {
                    if (this.lich_su.kiem_tra_mau(this.maDonHang, this.viTri))
                    {
                        this.lich_su.them_lich_su_hien_truong_khi_thai(SessionInfo.MaNV, maDonHang, viTri);
                    }

                    this.QuanLyHienTruong_BLL.CapNhatKhiThai(vi_tri_lay_mau.Text, float.Parse(TB_SO2.Text), float.Parse(BoxNO2.Text), float.Parse(BoxPM.Text), float.Parse(TB_nhiet_do.Text), nhan_vien, maDonHang);

                    phan_tich_khi_thai phan_tich_nm = new phan_tich_khi_thai(maDonHang, nhan_vien, viTri, edit);
                    ((dashboard)this.ParentForm).ShowFormOnPanel(phan_tich_nm);
                }
            }

            catch (Exception ex)
            {
                errorMess.Text = ex.Message;
                errorMess.Visible = true;

            }


        }


        private void vi_tri_lay_mau_TextChanged(object sender, EventArgs e)
        {

        }

        private void hien_truong_khi_thai_Load(object sender, EventArgs e)
        {
            if (edit)
            {

                DataTable khiThaiResultTable = this.QuanLyHienTruong_BLL.LayThongTinKhiThai(viTri, maDonHang);
                // Kiểm tra kết quả
                if (khiThaiResultTable.Rows.Count > 0)
                {
                    foreach (DataRow row in khiThaiResultTable.Rows)
                    {
                        string viTri = row["ViTriLayMau"].ToString();
                        string so2 = row["SO2"].ToString();
                        string no2 = row["NO2"].ToString();
                        string pm = row["PM"].ToString();
                        string nhietDo = row["NhietDo"].ToString();
                        string maNV = row["MaNV"].ToString();
                        string maDH = row["MaDH"].ToString();

                        vi_tri_lay_mau.Text = viTri;
                        vi_tri_lay_mau.Enabled = false;
                        TB_SO2.Text = so2;
                        BoxPM.Text = pm;
                        BoxNO2.Text = no2;
                        TB_nhiet_do.Text = nhietDo;
                    }
                }

                DataTable khiThaiResultTables = this.QuanLyHienTruong_BLL.LayThongTinKhiThai(viTri, maDonHang);

                DataTable chiTieuResultTable = this.QuanLyHienTruong_BLL.LayChiTieuKhiThai(viTri, maDonHang);


                // Kiểm tra kết quả
                if (khiThaiResultTable.Rows.Count > 0 && chiTieuResultTable.Rows.Count > 0)
                {
                    DataRow khiThaiRow = khiThaiResultTable.Rows[0];
                    DataRow chiTieuRow = chiTieuResultTable.Rows[0];

                    // Lấy dữ liệu từ bảng KhiThai
                    float so2 = float.Parse(khiThaiRow["SO2"].ToString());
                    float no2 = float.Parse(khiThaiRow["NO2"].ToString());
                    float pm = float.Parse(khiThaiRow["PM"].ToString());
                    float nhietDo = float.Parse(khiThaiRow["NhietDo"].ToString());

                    // Lấy dữ liệu từ bảng ChiTieuKhiThai
                    float chiTieuSO2 = float.Parse(chiTieuRow["SO2"].ToString());
                    float chiTieuNO2 = float.Parse(chiTieuRow["NO2"].ToString());
                    float chiTieuPM = float.Parse(chiTieuRow["PM"].ToString());
                    float chiTieuNhietDo = float.Parse(chiTieuRow["NhietDo"].ToString());

                    // So sánh và cập nhật label
                    vuotSo2.Text = CompareValue(so2, chiTieuSO2, "SO2");
                    vuotSo2.Visible = true;
                    vuotNo2.Text = CompareValue(no2, chiTieuNO2, "NO2");
                    vuotNo2.Visible = true;
                    vuotPM.Text = CompareValue(pm, chiTieuPM, "PM");
                    vuotPM.Visible = true;
                    vuotNd.Text = CompareValue(nhietDo, chiTieuNhietDo, "Nhiệt độ");
                    vuotNd.Visible = true;
                }
            }
        }

        // Hàm so sánh và trả về kết quả vượt ngưỡng
        private string CompareValue(float actualValue, float thresholdValue, string chiTieuName)
        {
            if (actualValue > thresholdValue)
            {
                // Tính phần trăm vượt ngưỡng
                float vuotPhanTram = ((actualValue - thresholdValue) / thresholdValue) * 100;

                if (vuotPhanTram > 5)
                {
                    return $"Vượt ngưỡng {vuotPhanTram:F2}% ({actualValue} > {thresholdValue})";
                }

                else
                {
                    return ""; // Không cần thông báo nếu vượt ngưỡng không quá 5%
                }
            }

            else if (actualValue < thresholdValue)
            {
                // Tính phần trăm thấp hơn ngưỡng
                float thapPhanTram = ((thresholdValue - actualValue) / thresholdValue) * 100;
                if (thapPhanTram > 5)
                {
                    return $"Thấp ngưỡng {thapPhanTram:F2}% ({actualValue} < {thresholdValue})";
                }

                else
                {
                    return ""; // Không cần thông báo nếu thấp ngưỡng không quá 5%
                }
            }

            else
            {
                // Giá trị thực tế bằng ngưỡng, không cần thông báo gì
                return "";
            }
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        }
}
