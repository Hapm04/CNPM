using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using EcoProject.Nhap_lieu;
using Guna.UI2.WinForms;
using BLL;

namespace EcoProject
{
    public partial class phan_tich_khong_khi : Form
    {
        LichSu_BLL lich_su;
        QuanLyPhongThiNghiem_BLL quanLyPhanTich_BLL = new QuanLyPhongThiNghiem_BLL();

        public phan_tich_khong_khi(string MaDH, string MaNV, string ViTriLayMau, bool IsEditMode)
        {
            InitializeComponent();
            lich_su = new LichSu_BLL();
            quanLyPhanTich_BLL =  new QuanLyPhongThiNghiem_BLL();
            currentMaDH = MaDH;
            currentMaNV = MaNV;
            currentViTriLayMau = ViTriLayMau;
            edit = IsEditMode;
        }

        public string currentMaDH { get; set; }
        public string currentMaNV { get; set; }
        public string currentViTriLayMau { get; set; }
        public bool edit { get; set; }

        private bool ValidateInputs()
        {
            // Kiểm tra dữ liệu rỗng
            if (!ValidateIsNullOrEmpty(TB_PM10) ||
                !ValidateIsNullOrEmpty(TB_SO2) ||
                !ValidateIsNullOrEmpty(BoxO3))
            {
                errorMess.Text = "Vui lòng nhập đầy đủ dữ liệu!";
                errorMess.Visible = true;

                return false;
            }

            // Kiểm tra giá trị từng TextBox
            if (
            !ValidateNumber(TB_PM10, 0, 500, "Bụi PM10 phải nằm trong khoảng từ 0 đến 500 µg/Nm³!") ||
            !ValidateNumber(TB_SO2, 0, 1000, "SO2 phải nằm trong khoảng từ 0 đến 1,000 µg/Nm³!") ||
            !ValidateNumber(BoxO3, 0, 500, "O3 phải nằm trong khoảng từ 0 đến 500 µg/Nm³!") 
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
            if (string.IsNullOrEmpty(textBox.Text))
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
                if(!ValidateInputs())
                {
                    //MessageBox.Show("Vui lòng nhập đầy đủ dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (edit)
                { 
                    this.quanLyPhanTich_BLL.CapNhatTrangThaiMau(this.currentMaDH, this.currentViTriLayMau); 
                }

                else
                {
                    this.quanLyPhanTich_BLL.CapNhatTrangThaiMau(this.currentMaDH, this.currentViTriLayMau);
                }

                bool result = this.quanLyPhanTich_BLL.CapNhatDuLieuKhongKhi(this.currentMaDH, this.currentViTriLayMau, float.Parse(TB_PM10.Text), float.Parse(TB_SO2.Text), float.Parse(BoxO3.Text));

                if (result)
                {
                    MessageBox.Show("Dữ liệu đã được cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (this.lich_su.kiem_tra_mau(this.currentMaDH, this.currentViTriLayMau))
                    {
                        this.lich_su.them_lich_su_phong_lab_khong_khi(this.currentMaNV, this.currentMaDH, this.currentViTriLayMau);
                    }
                    chi_tieu_khong_khi chi_tieu_khong_khi = new chi_tieu_khong_khi(currentMaDH, currentViTriLayMau, edit);
                    ((dashboard)this.ParentForm).ShowFormOnPanel(chi_tieu_khong_khi);
                }

                else
                {
                    MessageBox.Show("Cập nhật dữ liệu thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            catch (Exception ex)
            {
                errorMess.Text = ex.Message;
                errorMess.Visible = true;

            }
        }

        private void phan_tich_khong_khi_Load(object sender, EventArgs e)
        {
            if (edit)
            {
                DataTable KhongKhiResultTable = this.quanLyPhanTich_BLL.LayDuLieuKhongKhi(currentMaDH, currentViTriLayMau);

                // Kiểm tra kết quả
                if (KhongKhiResultTable.Rows.Count > 0)
                {
                    foreach (DataRow row in KhongKhiResultTable.Rows)
                    {
                        string viTri = row["ViTriLayMau"].ToString();
                        string pm10 = row["PM10"].ToString();
                        string so2 = row["SO2"].ToString();
                        string o3 = row["O3"].ToString();
                        string nhietDo = row["NhietDo"].ToString();
                        string maNV = row["MaNV"].ToString();
                        string maDH = row["MaDH"].ToString();

                        // Gán giá trị vào các TextBox
                        TB_PM10.Text = pm10;
                        TB_SO2.Text = so2;
                        BoxO3.Text = o3;
                    }
                }

                DataTable chiTieuResultTable = this.quanLyPhanTich_BLL.LayChiTieuKhongKhi(currentMaDH, currentViTriLayMau);

                // Kiểm tra kết quả
                if (KhongKhiResultTable.Rows.Count > 0 && chiTieuResultTable.Rows.Count > 0)
                {
                    DataRow khongKhiRow = KhongKhiResultTable.Rows[0];
                    DataRow chiTieuRow = chiTieuResultTable.Rows[0];

                    // Lấy dữ liệu từ bảng KhongKhi
                    float so2Value = float.Parse(khongKhiRow["SO2"].ToString());
                    float o3Value = float.Parse(khongKhiRow["O3"].ToString());
                    float pm10Value = float.Parse(khongKhiRow["PM10"].ToString());

                    // Lấy dữ liệu từ bảng ChiTieuKK
                    float chiTieuSO2 = float.Parse(chiTieuRow["SO2"].ToString());
                    float chiTieuO3 = float.Parse(chiTieuRow["O3"].ToString());
                    float chiTieuPM10 = float.Parse(chiTieuRow["PM10"].ToString());

                    // So sánh và cập nhật label
                    vuotSo2.Text = CompareValue(so2Value, chiTieuSO2, "SO2");
                    vuotSo2.Visible = !string.IsNullOrEmpty(vuotSo2.Text);

                    vuotO3.Text = CompareValue(o3Value, chiTieuO3, "O3");
                    vuotO3.Visible = !string.IsNullOrEmpty(vuotO3.Text);

                    vuotPm10.Text = CompareValue(pm10Value, chiTieuPM10, "PM10");
                    vuotPm10.Visible = !string.IsNullOrEmpty(vuotPm10.Text);
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



        private void vuotSo2_Click(object sender, EventArgs e)
        {

        }
    }
}
