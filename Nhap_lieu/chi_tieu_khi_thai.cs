using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using EcoProject.user_control;
using Guna.UI2.WinForms;
using BLL;

namespace EcoProject.Nhap_lieu
{
    public partial class chi_tieu_khi_thai : Form
    {
        LichSu_BLL lich_su;
        QuanLyChiTieu_BLL quanLyChiTieu_BLL;

        public chi_tieu_khi_thai(string viTriLayMau, string maDongHang, bool isEdit)
        {
            InitializeComponent();
            lich_su = new LichSu_BLL();
            quanLyChiTieu_BLL = new QuanLyChiTieu_BLL();
            vi_tri_lay_mau = viTriLayMau;
            ma_dong_hang = maDongHang;
            edit = isEdit;
        }
        private string vi_tri_lay_mau { get; set; }
        private string ma_dong_hang { get; set; }
        private bool edit { get; set; }

        private bool ValidateInputs()
        {
            // Kiểm tra dữ liệu rỗng
            if (!ValidateIsNullOrEmpty(TB_ap_suat) ||
                !ValidateIsNullOrEmpty(TB_CO) ||
                !ValidateIsNullOrEmpty(TB_H2S) ||
                !ValidateIsNullOrEmpty(TB_NO) ||
                !ValidateIsNullOrEmpty(BoxHg) ||
                !ValidateIsNullOrEmpty(BoxNH3) ||
                !ValidateIsNullOrEmpty(BoxO2) ||
                !ValidateIsNullOrEmpty(TB_SO2) ||
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
              
              !ValidateNumber(TB_ap_suat, 0, 100, "Áp suất phải nằm trong khoảng từ 0 đến 100!") ||
              !ValidateNumber(TB_CO, 0, 85000, "CO phải nằm trong khoảng từ 0 đến 85,000 µg/Nm³!") ||
              !ValidateNumber(TB_H2S, 0, 500, "H₂S phải nằm trong khoảng từ 0 đến 500 ppb!") ||
              !ValidateNumber(TB_NO, 0, 250, "NO phải nằm trong khoảng từ 0 đến 250 ppb!") ||
              !ValidateNumber(BoxHg, 0, 10, "Hg phải nằm trong khoảng từ 0 đến 10 µg/Nm³!") ||
              !ValidateNumber(BoxNH3, 0, 100, "NH₃ phải nằm trong khoảng từ 0 đến 100 ppb!") ||
              !ValidateNumber(BoxO2, 0, 21, "O₂ phải nằm trong khoảng từ 0 đến 21%!") ||
              !ValidateNumber(TB_SO2, 0, 400, "SO₂ phải nằm trong khoảng từ 0 đến 400 µg/Nm³!") ||
              !ValidateNumber(BoxNO2, 0, 250, "NO₂ phải nằm trong khoảng từ 0 đến 250 µg/Nm³!") ||
              !ValidateNumber(BoxPM, 0, 500, "PM phải nằm trong khoảng từ 0 đến 500 ppb!") ||
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

        private bool checkValue()
        {

            return true;
        }

        private bool QualifiedTestKhiThai()
        {
            try
            {
                DataTable khiThaiTable = this.quanLyChiTieu_BLL.LayDuLieuKhiThai(ma_dong_hang, vi_tri_lay_mau);

                if (khiThaiTable== null)
                {
                    throw new Exception("Không tìm thấy dữ liệu mẫu tương ứng trong cơ sở dữ liệu!");
                }

                if (khiThaiTable.Rows.Count == 0)
                {
                    throw new Exception("Không tìm thấy dữ liệu mẫu tương ứng trong cơ sở dữ liệu.");
                }

                // Lấy dữ liệu chuẩn từ các TextBox
                Dictionary<string, float> chiTieuTextBox = new Dictionary<string, float>
                {
                    { "CO", float.Parse(TB_CO.Text) },
                    { "NhietDo", float.Parse(TB_nhiet_do.Text) },
                    { "NO2", float.Parse(BoxNO2.Text) },
                    { "O2", float.Parse(BoxO2.Text) },
                    { "Hg", float.Parse(BoxHg.Text) },
                    { "PM", float.Parse(BoxPM.Text) },
                    { "NH3", float.Parse(BoxNH3.Text) },
                    { "N_O", float.Parse(TB_NO.Text) },
                    { "ApSuat", float.Parse(TB_ap_suat.Text) },
                    { "SO2", float.Parse(TB_SO2.Text) },
                    { "H2S", float.Parse(TB_H2S.Text) }
                };

                // Danh sách các chỉ tiêu cần kiểm tra
                string[] chiTieuList = { "CO", "NhietDo", "NO2", "O2", "Hg", "PM", "NH3", "N_O", "ApSuat", "SO2", "H2S" };

                // Lấy hàng dữ liệu đầu tiên từ bảng KhiThai
                DataRow khiThaiRow = khiThaiTable.Rows[0];

                foreach (string chiTieu in chiTieuList)
                {
                    // Lấy giá trị thực tế từ cơ sở dữ liệu
                    float giaTriThucTe = khiThaiRow[chiTieu] != DBNull.Value ? Convert.ToSingle(khiThaiRow[chiTieu]) : 0f;

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
                if (!ValidateInputs())
                {
                    return;
                }

                string nhavien = SessionInfo.MaNV;

                if (!edit)
                {
                    this.quanLyChiTieu_BLL.ThemChiTieuKhiThai(vi_tri_lay_mau, ma_dong_hang, nhavien, float.Parse(TB_ap_suat.Text), 
                        float.Parse(TB_CO.Text), float.Parse(TB_H2S.Text), float.Parse(BoxO2.Text), float.Parse(BoxNH3.Text), float.Parse(BoxHg.Text), 
                        float.Parse(TB_NO.Text), float.Parse(TB_SO2.Text), float.Parse(BoxNO2.Text), float.Parse(BoxPM.Text), float.Parse(TB_nhiet_do.Text));
                }

                else
                {
                    bool result =this.quanLyChiTieu_BLL.CapNhatChiTieuKhiThai(vi_tri_lay_mau, ma_dong_hang, float.Parse(TB_ap_suat.Text), float.Parse(TB_CO.Text), float.Parse(TB_H2S.Text), float.Parse(BoxO2.Text), float.Parse(BoxNH3.Text), float.Parse(BoxHg.Text), float.Parse(TB_NO.Text), float.Parse(TB_SO2.Text), float.Parse(BoxNO2.Text), float.Parse(BoxPM.Text), float.Parse(TB_nhiet_do.Text));

                    if (result)
                    {
                        MessageBox.Show("Dữ liệu đã được cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        if (this.lich_su.kiem_tra_mau(this.ma_dong_hang, this.vi_tri_lay_mau))
                        {
                            this.lich_su.them_lich_su_chi_tieu_khi_thai(SessionInfo.MaNV, this.ma_dong_hang, this.vi_tri_lay_mau);
                        }
                    }
                }

                if (QualifiedTestKhiThai())
                {
                    this.quanLyChiTieu_BLL.CapNhatKetQua(this.ma_dong_hang, this.vi_tri_lay_mau, true);
                }
                else
                {
                    this.quanLyChiTieu_BLL.CapNhatKetQua(this.ma_dong_hang, this.vi_tri_lay_mau, false);
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

        private void chi_tieu_khi_thai_Load(object sender, EventArgs e)
        {
            if (edit)
            {
                DataTable ChiTieuKhiThaiResultTable = this.quanLyChiTieu_BLL.LayChiTieuKhiThai(ma_dong_hang, vi_tri_lay_mau);

                // Kiểm tra kết quả
                if (ChiTieuKhiThaiResultTable.Rows.Count > 0)
                {
                    foreach (DataRow row in ChiTieuKhiThaiResultTable.Rows)
                    {
                        string viTri = row["ViTriLayMau"].ToString();
                        string apSuat = row["ApSuat"].ToString();
                        string co = row["CO"].ToString();
                        string h2s = row["H2S"].ToString();
                        string o2 = row["O2"].ToString();
                        string nh3 = row["NH3"].ToString();
                        string hg = row["Hg"].ToString();
                        string n_o = row["N_O"].ToString();
                        string so2 = row["SO2"].ToString();
                        string no2 = row["NO2"].ToString();
                        string pm = row["PM"].ToString();
                        string nhietDo = row["NhietDo"].ToString();
                        string maNV = row["MaNV"].ToString();
                        string maDH = row["MaDH"].ToString();

                        // Gán giá trị vào các TextBox

                        TB_ap_suat.Text = apSuat;
                        TB_CO.Text = co;
                        TB_H2S.Text = h2s;
                        BoxO2.Text = o2;
                        BoxNH3.Text = nh3;
                        BoxHg.Text = hg;
                        TB_NO.Text = n_o;
                        TB_SO2.Text = so2;
                        BoxNO2.Text = no2;
                        BoxPM.Text = pm;
                        TB_nhiet_do.Text = nhietDo;
                    }
                }
            }

        }
    }
}
