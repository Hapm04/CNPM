using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using EcoProject.user_control;
using Guna.UI2.WinForms;
using BLL;


namespace EcoProject.Nhap_lieu
{
    public partial class hien_truong_nuoc_mat : Form
    {
        LichSu_BLL lich_su;
        QuanLyHienTruong_BLL quanLyHienTruong_BLL = new QuanLyHienTruong_BLL();

        public hien_truong_nuoc_mat(string selectedLoaiMau, string selectedMaDonHang, bool IsEditMode, string currentViTriLayMau)
        {
            InitializeComponent();
            lich_su = new LichSu_BLL();
            quanLyHienTruong_BLL = new QuanLyHienTruong_BLL();
            loai_mau = selectedLoaiMau;
            maDonHang = selectedMaDonHang;
            edit = IsEditMode;
            viTri = currentViTriLayMau;
        }

        public string maDonHang { get; set; }
        public string loai_mau { get; set; }
        public bool edit { get; set; }
        public string viTri { get; set; }

        private bool ValidateInputs()
        {
            // Kiểm tra dữ liệu rỗng
            if (!ValidateIsNullOrEmpty(BoxDO) ||
                !ValidateIsNullOrEmpty(BoxpH) ||
                !ValidateIsNullOrEmpty(BoxTDS) ||
                !ValidateIsNullOrEmpty(TB_nhiet_do))
            {
                return false;
            }

            // Kiểm tra giá trị từng TextBox
            if (!ValidateNumber(BoxDO, 0, 20, "DO phải nằm trong khoảng từ 0 đến 20 mg/L!") ||
                !ValidateNumber(BoxpH, 0, 14, "pH phải nằm trong khoảng từ 0 đến 14!") ||
                !ValidateNumber(BoxTDS, 0, 500, "TDS phải nằm trong khoảng từ 0 đến 500 mg/L!") ||
                !ValidateNumber(TB_nhiet_do, 0, 80, "Nhiệt độ phải nằm trong khoảng từ 0 đến 80°C!") )
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
                    this.quanLyHienTruong_BLL.ThemNuocMat(vi_tri_lay_mau.Text, maDonHang, nhan_vien, float.Parse(BoxDO.Text), float.Parse(BoxpH.Text), float.Parse(BoxTDS.Text), float.Parse(TB_nhiet_do.Text));

                    // Đóng form hiện tại và hiển thị lại form uc_lab_analysis_management1
                    uc_lab_analysis_management1 uc_lab_analysis_management1 = new uc_lab_analysis_management1();
                    ((dashboard)this.ParentForm).ShowUserControlOnPanel(uc_lab_analysis_management1);

                }
                else
                {
                    if (this.lich_su.kiem_tra_mau(this.maDonHang, this.viTri))
                    {
                        this.lich_su.them_lich_su_hien_truong_nuoc_mat(SessionInfo.MaNV, maDonHang, viTri);
                    }

                    this.quanLyHienTruong_BLL.CapNhatNuocMat(vi_tri_lay_mau.Text, maDonHang, float.Parse(BoxDO.Text), float.Parse(BoxpH.Text), float.Parse(BoxTDS.Text), float.Parse(TB_nhiet_do.Text), nhan_vien);


                    phan_tich_nuoc_mat phan_tich_nm = new phan_tich_nuoc_mat(maDonHang, nhan_vien, viTri, edit);
                    ((dashboard)this.ParentForm).ShowFormOnPanel(phan_tich_nm);
                }
            }

            catch (Exception ex)
            {
                errorMess.Text = ex.Message;
                errorMess.Visible = true;
            }

        }

        private void hien_truong_nuoc_mat_Load(object sender, EventArgs e)
        {
            if (edit)
            {
                DataTable NuocMatResultTable = this.quanLyHienTruong_BLL.LayDuLieuNuocMat(maDonHang, viTri);

                // Kiểm tra kết quả
                if (NuocMatResultTable.Rows.Count > 0)
                {
                    foreach (DataRow row in NuocMatResultTable.Rows)
                    {
                        string viTri = row["ViTriLayMau"].ToString();
                        string DO = row["DO"].ToString();
                        string pH = row["pH"].ToString();
                        string TDS = row["TDS"].ToString();
                        string nhietDo = row["NhietDo"].ToString();
                        string maNV = row["MaNV"].ToString();
                        string maDH = row["MaDH"].ToString();

                        vi_tri_lay_mau.Text = viTri;
                        vi_tri_lay_mau.Enabled = false;
                        BoxpH.Text = pH;
                        BoxTDS.Text = TDS;
                        BoxDO.Text = DO;
                        TB_nhiet_do.Text = nhietDo;
                    }
                }

                DataTable chiTieuResultTable = this.quanLyHienTruong_BLL.LayChiTieuNuocMat(maDonHang, viTri);

                // Kiểm tra kết quả
                if (NuocMatResultTable.Rows.Count > 0 && chiTieuResultTable.Rows.Count > 0)
                {
                    DataRow nuocMatRow = NuocMatResultTable.Rows[0];
                    DataRow chiTieuRow = chiTieuResultTable.Rows[0];

                    // Lấy dữ liệu từ bảng NuocMat
                    float doValue = float.Parse(nuocMatRow["DO"].ToString());
                    float phValue = float.Parse(nuocMatRow["pH"].ToString());
                    float tdsValue = float.Parse(nuocMatRow["TDS"].ToString());
                    float nhietDoValue = float.Parse(nuocMatRow["NhietDo"].ToString());

                    // Lấy dữ liệu từ bảng ChiTieuNuocMat
                    float chiTieuDO = float.Parse(chiTieuRow["DO"].ToString());
                    float chiTieupH = float.Parse(chiTieuRow["pH"].ToString());
                    float chiTieuTDS = float.Parse(chiTieuRow["TDS"].ToString());
                    float chiTieuNhietDo = float.Parse(chiTieuRow["NhietDo"].ToString());

                    // So sánh và cập nhật label
                    vuotDo.Text = CompareValue(doValue, chiTieuDO, "DO");
                    vuotDo.Visible = true;
                    vuotph.Text = CompareValue(phValue, chiTieupH, "pH");
                    vuotph.Visible = true;
                    vuotTds.Text = CompareValue(tdsValue, chiTieuTDS, "TDS");
                    vuotTds.Visible = true;
                    vuotNd.Text = CompareValue(nhietDoValue, chiTieuNhietDo, "Nhiệt độ");
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


        private void vuotNd_Click(object sender, EventArgs e)
        {

        }
    }
}
