using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using EcoProject.user_control;
using Guna.UI2.WinForms;
using BLL;

namespace EcoProject
{
    public partial class hien_truong_khong_khi : Form
    {
        LichSu_BLL lich_su;
        QuanLyHienTruong_BLL quanLyHienTruong_BLL = new QuanLyHienTruong_BLL();

        public hien_truong_khong_khi(string selectedLoaiMau, string selectedMaDonHang, bool IsEditMode, string currentViTriLayMau)
        {
            InitializeComponent();
            lich_su = new LichSu_BLL();
            quanLyHienTruong_BLL =  new QuanLyHienTruong_BLL();
            loai_mau = selectedLoaiMau;
            maDonHang = selectedMaDonHang;
            edit = IsEditMode;
            viTri = currentViTriLayMau;
        }
        public string maDonHang { get; set; }
        public string loai_mau { get; set; }
        public string viTri { get; set; }
        public bool edit { get; set; }
        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void hien_truong_khong_khi_Load(object sender, EventArgs e)
        {
            if (edit)
            {
                DataTable KhongKhiResultTable = this.quanLyHienTruong_BLL.LayDuLieuKhongKhi(viTri, maDonHang);
                // Kiểm tra kết quả
                if (KhongKhiResultTable.Rows.Count > 0)
                {
                    foreach (DataRow row in KhongKhiResultTable.Rows)
                    {
                        string viTri = row["ViTriLayMau"].ToString();
                        string PM2dot5 = row["PM2dot5"].ToString();
                        string CO = row["CO"].ToString();
                        string NO2 = row["NO2"].ToString();
                        string nhietDo = row["NhietDo"].ToString();
                        string maNV = row["MaNV"].ToString();
                        string maDH = row["MaDH"].ToString();

                        // Cập nhật giá trị vào các ô nhập liệu
                        vi_tri_lay_mau.Text = viTri;
                        vi_tri_lay_mau.Enabled = false;
                        BoxCO.Text = CO;
                        BoxNO2.Text = NO2;
                        BoxPM2dot5.Text = PM2dot5;
                        TB_nhiet_do.Text = nhietDo;
                    }
                }

                DataTable chiTieuResultTable = this.quanLyHienTruong_BLL.LayChiTieuKK(viTri, maDonHang);

                // Kiểm tra kết quả
                if (KhongKhiResultTable.Rows.Count > 0 && chiTieuResultTable.Rows.Count > 0)
                {
                    DataRow khongKhiRow = KhongKhiResultTable.Rows[0];
                    DataRow chiTieuRow = chiTieuResultTable.Rows[0];

                    // Lấy dữ liệu từ bảng KhongKhi
                    float co = float.Parse(khongKhiRow["CO"].ToString());
                    float no2 = float.Parse(khongKhiRow["NO2"].ToString());
                    float pm25 = float.Parse(khongKhiRow["PM2dot5"].ToString());
                    float nhietDo = float.Parse(khongKhiRow["NhietDo"].ToString());

                    // Lấy dữ liệu từ bảng ChiTieuKK
                    float chiTieuCO = float.Parse(chiTieuRow["CO"].ToString());
                    float chiTieuNO2 = float.Parse(chiTieuRow["NO2"].ToString());
                    float chiTieuPM25 = float.Parse(chiTieuRow["PM2dot5"].ToString());
                    float chiTieuNhietDo = float.Parse(chiTieuRow["NhietDo"].ToString());

                    // So sánh và cập nhật label
                    vuotCo.Text = CompareValue(co, chiTieuCO, "CO");
                    vuotCo.Visible = true;
                    vuotNo2.Text = CompareValue(no2, chiTieuNO2, "NO2");
                    vuotNo2.Visible = true;
                    vuotPm25.Text = CompareValue(pm25, chiTieuPM25, "PM2.5");
                    vuotPm25.Visible = true;
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


        private bool ValidateInputs()
        {
            // Kiểm tra dữ liệu rỗng
            if (!ValidateIsNullOrEmpty(BoxPM2dot5) ||
                !ValidateIsNullOrEmpty(BoxCO) ||
                !ValidateIsNullOrEmpty(BoxNO2) ||
                !ValidateIsNullOrEmpty(TB_nhiet_do))
            {
                //errorMess.Text = "Vui lòng nhập đầy đủ dữ liệu!";
                //errorMess.Visible = true;

                return false;
            }

            // Kiểm tra giá trị từng TextBox
            if (
            !ValidateNumber(BoxPM2dot5, 0, 150, "Bụi PM2.5 phải nằm trong khoảng từ 0 đến 150 µg/Nm³!") ||
            !ValidateNumber(BoxCO, 0, 100000, "CO phải nằm trong khoảng từ 0 đến 100,000 µg/Nm³!") ||
            !ValidateNumber(BoxNO2, 0, 500, "NO2 phải nằm trong khoảng từ 0 đến 500 µg/Nm³!") ||
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
                if (!ValidateInputs())
                {
                    return;
                }

                string nhan_vien = SessionInfo.MaNV;

                if (!edit)
                {
                    this.quanLyHienTruong_BLL.ThemMau(vi_tri_lay_mau.Text, maDonHang, nhan_vien, loai_mau);
                    this.quanLyHienTruong_BLL.ThemDuLieuKhongKhi(nhan_vien, maDonHang, vi_tri_lay_mau.Text, BoxPM2dot5.Text, BoxCO.Text, BoxNO2.Text, TB_nhiet_do.Text);

                    //// Đóng form hiện tại và chuyển sang uc_lab_analysis_management1
                    uc_lab_analysis_management1 uc_lab_analysis_management1 = new uc_lab_analysis_management1();

                    ((dashboard)this.ParentForm).ShowUserControlOnPanel(uc_lab_analysis_management1);
                }

                else
                {
                    if (this.lich_su.kiem_tra_mau(this.maDonHang, this.viTri))
                    {
                        this.lich_su.them_lich_su_hien_truong_khong_khi(SessionInfo.MaNV, maDonHang, viTri);
                    }

                    this.quanLyHienTruong_BLL.CapNhatDuLieuKhongKhi(vi_tri_lay_mau.Text, maDonHang, BoxPM2dot5.Text, BoxCO.Text, BoxNO2.Text, TB_nhiet_do.Text, nhan_vien);

                    phan_tich_khong_khi phan_tich_nm = new phan_tich_khong_khi(maDonHang, nhan_vien, viTri, edit);
                    ((dashboard)this.ParentForm).ShowFormOnPanel(phan_tich_nm);
                }

            }

            catch (Exception ez)
            {
                errorMess.Text = ez.Message;
                errorMess.Visible = true;

            }

        }

        private void guna2Panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }
    }
}
