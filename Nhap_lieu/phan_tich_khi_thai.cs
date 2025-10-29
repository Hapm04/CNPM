using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using EcoProject.Nhap_lieu;
using Guna.UI2.WinForms;
using BLL;


namespace EcoProject
{
    public partial class phan_tich_khi_thai : Form
    {
        LichSu_BLL lich_su;
        QuanLyPhongThiNghiem_BLL quanLyPhanTich_BLL = new QuanLyPhongThiNghiem_BLL();
        public phan_tich_khi_thai(string MaDH, string MaNV, string ViTriLayMau, bool IsEditMode)
        {
            InitializeComponent();
            lich_su = new LichSu_BLL();
            quanLyPhanTich_BLL = new QuanLyPhongThiNghiem_BLL();
            currentMaDH = MaDH;
            currentMaNV = MaNV;
            currentViTriLayMau = ViTriLayMau;
            edit = IsEditMode;

        }
        public string currentMaDH { get; set; }
        public string currentMaNV { get; set; }
        public string currentViTriLayMau { get; set; }
        public bool edit { get; set; }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }

        private bool ValidateInputs()
        {
            // Kiểm tra dữ liệu rỗng
            if (!ValidateIsNullOrEmpty(TB_ap_suat) ||
                !ValidateIsNullOrEmpty(TB_CO) ||
                !ValidateIsNullOrEmpty(TB_H2S) ||
                !ValidateIsNullOrEmpty(TB_NO) ||
                !ValidateIsNullOrEmpty(BoxHg) ||
                !ValidateIsNullOrEmpty(BoxNH3) ||
                !ValidateIsNullOrEmpty(BoxO2))
            {
                //errorMess.Text = "Vui lòng nhập đầy đủ dữ liệu!";
                //errorMess.Visible = true;

                return false;
            }

            // Kiểm tra giá trị từng TextBox
            if (
            !ValidateNumber(TB_ap_suat, 0, 10000, "Áp suất phải nằm trong khoảng từ 0 đến 10,000!") ||
            !ValidateNumber(TB_CO, 0, 85000, "CO phải nằm trong khoảng từ 0 đến 85,000 µg/Nm³!") ||
            !ValidateNumber(TB_H2S, 0, 1000, "H2S phải nằm trong khoảng từ 0 đến 1,000 µg/Nm³!") ||
            !ValidateNumber(TB_NO, 0, 500, "NO phải nằm trong khoảng từ 0 đến 500 µg/Nm³!") ||
            !ValidateNumber(BoxHg, 0, 100, "Hg phải nằm trong khoảng từ 0 đến 100 µg/Nm³!") ||
            !ValidateNumber(BoxNH3, 0, 20, "NH3 phải nằm trong khoảng từ 0 đến 20 mg/L!") ||
            !ValidateNumber(BoxO2, 0, 100, "O2 phải nằm trong khoảng từ 0 đến 100%!")

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
                    return; // Ngừng xử lý nếu có lỗi
                }

                if (edit)
                {
                    this.quanLyPhanTich_BLL.CapNhatTrangThaiMau(this.currentMaDH, this.currentViTriLayMau);
                } 
                    else
                {
                    this.quanLyPhanTich_BLL.CapNhatTrangThaiMau(this.currentMaDH, this.currentViTriLayMau);
                }

                bool result = this.quanLyPhanTich_BLL.CapNhatKhiThai(currentMaDH, currentViTriLayMau, new object[] { TB_ap_suat.Text, TB_CO.Text, TB_H2S.Text, BoxO2.Text, BoxNH3.Text, BoxHg.Text, TB_NO.Text, currentMaDH, currentViTriLayMau });

                if (result)
                {
                    MessageBox.Show("Dữ liệu đã được cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (this.lich_su.kiem_tra_mau(this.currentMaDH, this.currentViTriLayMau))
                    {
                        this.lich_su.them_lich_su_phong_lab_khi_thai(this.currentMaNV, this.currentMaDH, this.currentViTriLayMau);
                    }
                    chi_tieu_khi_thai chi_tieu_khi_thai = new chi_tieu_khi_thai(currentViTriLayMau, currentMaDH, edit);
                    ((dashboard)this.ParentForm).ShowFormOnPanel(chi_tieu_khi_thai);

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

        private void phan_tich_khi_thai_Load(object sender, EventArgs e)
        {
            if (edit)
            {
                DataTable KhiThaiResultTable = quanLyPhanTich_BLL.LayDuLieuKhiThai(currentViTriLayMau, currentMaDH);

                // Kiểm tra kết quả
                if (KhiThaiResultTable.Rows.Count > 0)
                {
                    foreach (DataRow row in KhiThaiResultTable.Rows)
                    {
                        string viTri = row["ViTriLayMau"].ToString();
                        string apSuat = row["ApSuat"].ToString();
                        string co = row["CO"].ToString();
                        string h2s = row["H2S"].ToString();
                        string o2 = row["O2"].ToString();
                        string nh3 = row["NH3"].ToString();
                        string hg = row["Hg"].ToString();
                        string n_o = row["N_O"].ToString();
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
                    }
                }

                DataTable chiTieuResultTable = quanLyPhanTich_BLL.LayChiTieuKhiThai(currentViTriLayMau, currentMaDH);

                // Kiểm tra kết quả
                if (KhiThaiResultTable.Rows.Count > 0 && chiTieuResultTable.Rows.Count > 0)
                {
                    DataRow khiThaiRow = KhiThaiResultTable.Rows[0];
                    DataRow chiTieuRow = chiTieuResultTable.Rows[0];

                    // Lấy dữ liệu từ bảng KhiThai
                    float coValue = float.Parse(khiThaiRow["CO"].ToString());
                    float h2sValue = float.Parse(khiThaiRow["H2S"].ToString());
                    float o2Value = float.Parse(khiThaiRow["O2"].ToString());
                    float apSuatValue = float.Parse(khiThaiRow["ApSuat"].ToString());
                    float hgValue = float.Parse(khiThaiRow["Hg"].ToString());
                    float nh3Value = float.Parse(khiThaiRow["NH3"].ToString());
                    float n_oValue = float.Parse(khiThaiRow["N_O"].ToString());

                    // Lấy dữ liệu từ bảng ChiTieuKhiThai
                    float chiTieuCO = float.Parse(chiTieuRow["CO"].ToString());
                    float chiTieuH2S = float.Parse(chiTieuRow["H2S"].ToString());
                    float chiTieuO2 = float.Parse(chiTieuRow["O2"].ToString());
                    float chiTieuApSuat = float.Parse(chiTieuRow["ApSuat"].ToString());
                    float chiTieuHg = float.Parse(chiTieuRow["Hg"].ToString());
                    float chiTieuNH3 = float.Parse(chiTieuRow["NH3"].ToString());
                    float chiTieuNO = float.Parse(chiTieuRow["N_O"].ToString());

                    // So sánh và cập nhật label
                    vuotCo2.Text = CompareValue(coValue, chiTieuCO, "CO");
                    vuotCo2.Visible = true;
                    vuotNo2.Text = CompareValue(n_oValue, chiTieuNO, "NO");
                    vuotNo2.Visible = true;
                    vuotO2.Text = CompareValue(o2Value, chiTieuO2, "O2");
                    vuotO2.Visible = true;
                    vuotAx.Text = CompareValue(apSuatValue, chiTieuApSuat, "Áp suất");
                    vuotAx.Visible = true;
                    vuotHg.Text = CompareValue(hgValue, chiTieuHg, "Hg");
                    vuotHg.Visible = true;
                    vuotH2s.Text = CompareValue(h2sValue, chiTieuH2S, "H2S");
                    vuotH2s.Visible = true;
                    vuotNh3.Text = CompareValue(nh3Value, chiTieuNH3, "NH3");
                    vuotNh3.Visible = true;
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



        private void H2S_Click(object sender, EventArgs e)
        {

        }
    }
}
