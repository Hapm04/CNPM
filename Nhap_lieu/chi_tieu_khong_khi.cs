using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using EcoProject.user_control;
using BLL;

namespace EcoProject.Nhap_lieu
{
    public partial class chi_tieu_khong_khi : Form
    {
        LichSu_BLL lich_su;
        QuanLyChiTieu_BLL quanLyChiTieu_BLL;

        public chi_tieu_khong_khi(string maDonHang, string viTriLayMau, bool isEdit)
        {
            InitializeComponent();
            lich_su = new LichSu_BLL();
            quanLyChiTieu_BLL = new QuanLyChiTieu_BLL();
            currentMaDH = maDonHang;
            currentViTriLayMau = viTriLayMau;
            edit = isEdit;

        }
        public string currentMaDH { get; set; }
        public string currentViTriLayMau { get; set; }
        public bool edit { get; set; }

        private bool ValidateInputs()
        {
            // Kiểm tra dữ liệu rỗng
            if (!ValidateIsNullOrEmpty(BoxPM2dot5) ||
                !ValidateIsNullOrEmpty(BoxCO) ||
                !ValidateIsNullOrEmpty(BoxNO2) ||
                !ValidateIsNullOrEmpty(TB_nhiet_do) ||
                !ValidateIsNullOrEmpty(TB_PM10) ||
                !ValidateIsNullOrEmpty(TB_SO2) ||
                !ValidateIsNullOrEmpty(BoxO3))
            {
                return false;
            }

            // Kiểm tra giá trị từng TextBox
            if (
            !ValidateNumber(BoxPM2dot5, 0, 60, "PM2.5 phải nằm trong khoảng từ 0 đến 60 µg/Nm³!") ||
            !ValidateNumber(BoxCO, 0, 85000, "CO phải nằm trong khoảng từ 0 đến 85,000 µg/Nm³!") ||
            !ValidateNumber(BoxNO2, 0, 250, "NO₂ phải nằm trong khoảng từ 0 đến 250 µg/Nm³!") ||
            !ValidateNumber(TB_nhiet_do, 0, 80, "Nhiệt độ phải nằm trong khoảng từ 0 đến 80°C!") ||
            !ValidateNumber(TB_PM10, 0, 250, "PM10 phải nằm trong khoảng từ 0 đến 250 µg/Nm³!") ||
            !ValidateNumber(TB_SO2, 0, 400, "SO₂ phải nằm trong khoảng từ 0 đến 400 µg/Nm³!") ||
            !ValidateNumber(BoxO3, 0, 250, "O₃ phải nằm trong khoảng từ 0 đến 250 µg/Nm³!"))
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

        private bool QualifiedTest()
        {
            try
            {
                DataTable nuocMatTable = quanLyChiTieu_BLL.LayDuLieuKhongKhi(currentMaDH, currentViTriLayMau);

                if (nuocMatTable == null)
                {
                    throw new Exception("Không tìm thấy dữ liệu mẫu tương ứng trong cơ sở dữ liệu!");
                }

                if (nuocMatTable.Rows.Count == 0)
                {
                    throw new Exception("Không tìm thấy dữ liệu mẫu tương ứng trong cơ sở dữ liệu.");
                }

                // Lấy dữ liệu chuẩn từ các TextBox
                Dictionary<string, float> chiTieuTextBox = new Dictionary<string, float>
                {
                    { "CO", float.Parse(BoxCO.Text) },
                    { "SO2", float.Parse(TB_SO2.Text) },
                    { "O3", float.Parse(BoxO3.Text) },
                    { "PM10", float.Parse(TB_PM10.Text) },
                    { "PM2dot5", float.Parse(BoxPM2dot5.Text) },
                    { "NhietDo", float.Parse(TB_nhiet_do.Text) },
                    { "NO2", float.Parse(BoxNO2.Text)}
                };

                // Danh sách các chỉ tiêu cần kiểm tra
                string[] chiTieuList = { "CO", "SO2", "O3", "PM10", "PM2dot5", "NhietDo", "NO2"};

                // Lấy hàng dữ liệu đầu tiên từ bảng KhongKhi
                DataRow nuocMatRow = nuocMatTable.Rows[0];

                foreach (string chiTieu in chiTieuList)
                {
                    // Lấy giá trị thực tế từ cơ sở dữ liệu
                    float giaTriThucTe = nuocMatRow[chiTieu] != DBNull.Value ? Convert.ToSingle(nuocMatRow[chiTieu]) : 0f;

                    // Lấy giá trị chuẩn từ TextBox
                    float giaTriChuan = chiTieuTextBox[chiTieu];

                    // Tính chênh lệch phần trăm
                    float chenhlech = Math.Abs((giaTriThucTe - giaTriChuan) / giaTriChuan) * 100;

                    // Kiểm tra nếu chênh lệch vượt quá 5%
                    if (chenhlech > 5)
                    {
                        return false; // Không đạt chuẩn
                    }
                }

                return true; // Đạt chuẩn
            }
            catch (FormatException)
            {
                throw new Exception("Dữ liệu nhập vào không hợp lệ. Vui lòng kiểm tra lại các TextBox.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi: {ex.Message}");
            }
        }

        private void savechange_Click(object sender, EventArgs e)
        {
            try
            {
                string nhanvien = SessionInfo.MaNV;
                if (!ValidateInputs())
           
                {
                    //MessageBox.Show("Vui lòng nhập đầy đủ dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!edit)
                {
                    this.quanLyChiTieu_BLL.ThemChiTieuKhongKhi(currentMaDH, currentViTriLayMau, nhanvien, float.Parse(BoxPM2dot5.Text), float.Parse(BoxCO.Text), float.Parse(BoxNO2.Text), float.Parse(TB_nhiet_do.Text), float.Parse(TB_PM10.Text), float.Parse(TB_SO2.Text), float.Parse(BoxO3.Text));
                    MessageBox.Show("Dữ liệu đã được cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                else
                {
                    bool result = this.quanLyChiTieu_BLL.CapNhatChiTieuKhongKhi(currentMaDH, currentViTriLayMau, float.Parse(BoxPM2dot5.Text), float.Parse(BoxCO.Text), float.Parse(BoxNO2.Text), float.Parse(TB_nhiet_do.Text), float.Parse(TB_PM10.Text), float.Parse(TB_SO2.Text), float.Parse(BoxO3.Text));

                    if (result) 
                    {
                        MessageBox.Show("Dữ liệu đã được cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        if (this.lich_su.kiem_tra_mau(this.currentMaDH, this.currentViTriLayMau))
                        {
                            this.lich_su.them_lich_su_chi_tieu_khong_khi(SessionInfo.MaNV, this.currentMaDH, this.currentViTriLayMau);
                        }
                    }
                }

                if(QualifiedTest())
                {
                    this.quanLyChiTieu_BLL.CapNhatKetQua(currentMaDH, currentViTriLayMau, true);
                }
            
                else
                {
                    this.quanLyChiTieu_BLL.CapNhatKetQua(currentMaDH, currentViTriLayMau, false);
                }

                uc_lab_analysis_management1 uc_lab_analysis_management1 = new uc_lab_analysis_management1();
                ((dashboard)this.ParentForm).ShowUserControlOnPanel(uc_lab_analysis_management1);
            }

            catch
            {
                errorMess.Text = "Vị trí này đã lấy mẫu rồi!";
                errorMess.Visible = true;
            }
        }

        private void chi_tieu_khong_khi_Load(object sender, EventArgs e)
        {
            if (edit)
            { 
                DataTable ChiTieuKKResultTable = quanLyChiTieu_BLL.LayChiTieuKhongKhi(currentMaDH, currentViTriLayMau);

                // Kiểm tra kết quả
                if (ChiTieuKKResultTable.Rows.Count > 0)
                {
                    foreach (DataRow row in ChiTieuKKResultTable.Rows)
                    {
                        string viTri = row["ViTriLayMau"].ToString();
                        string pm2dot5 = row["PM2dot5"].ToString();
                        string co = row["CO"].ToString();
                        string no2 = row["NO2"].ToString();
                        string nhietDo = row["NhietDo"].ToString();
                        string pm10 = row["PM10"].ToString();
                        string so2 = row["SO2"].ToString();
                        string o3 = row["O3"].ToString();
                        string maNV = row["MaNV"].ToString();
                        string maDH = row["MaDH"].ToString();

                        // Gán giá trị vào các TextBox

                        BoxPM2dot5.Text = pm2dot5;
                        BoxCO.Text = co;
                        BoxNO2.Text = no2;
                        TB_nhiet_do.Text = nhietDo;
                        TB_PM10.Text = pm10;
                        TB_SO2.Text = so2;
                        BoxO3.Text = o3;
                    }
                }
            }

        }
    }
}
