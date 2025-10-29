using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using EcoProject.Nhap_lieu;
using Guna.UI2.WinForms;
using BLL;

namespace EcoProject
{
    public partial class phan_tich_nuoc_mat : Form
    {
        LichSu_BLL lich_su;
        QuanLyPhongThiNghiem_BLL quanLyPhanTich_BLL = new QuanLyPhongThiNghiem_BLL();
        public phan_tich_nuoc_mat(string MaDH, string MaNV, string ViTriLayMau, bool IsEditMode)
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

        private bool ValidateInputs()
        {


            // Kiểm tra dữ liệu rỗng
            if (!ValidateIsNullOrEmpty(TB_NH4) ||
                !ValidateIsNullOrEmpty(TB_NO3) ||
                !ValidateIsNullOrEmpty(TB_PO4) ||
                !ValidateIsNullOrEmpty(TB_tongN) ||
                !ValidateIsNullOrEmpty(TB_TSS) ||
                !ValidateIsNullOrEmpty(BoxCOD) ||
                !ValidateIsNullOrEmpty(BoxTOC) ||
                !ValidateIsNullOrEmpty(BoxtongP))
            {
                return false;
            }

            // Kiểm tra giá trị từng TextBox
            if (
            !ValidateNumber(TB_NH4, 0, 20, "NH4+ phải nằm trong khoảng từ 0 đến 20 mg/L!") ||
            !ValidateNumber(TB_NO3, 0, 5, "NO3- phải nằm trong khoảng từ 0 đến 5 mg/L!") ||
            !ValidateNumber(TB_PO4, 0, 2, "PO4³- phải nằm trong khoảng từ 0 đến 2 mg/L!") ||
            !ValidateNumber(TB_tongN, 0, 20, "Tổng N phải nằm trong khoảng từ 0 đến 20 mg/L!") ||
            !ValidateNumber(TB_TSS, 0, 500, "TSS phải nằm trong khoảng từ 0 đến 500 mg/L!") ||
            !ValidateNumber(BoxCOD, 0, 100, "COD phải nằm trong khoảng từ 0 đến 100 mg/L!") ||
            !ValidateNumber(BoxTOC, 0, 100, "TOC phải nằm trong khoảng từ 0 đến 100 mg/L!") ||
            !ValidateNumber(BoxtongP, 0, 2, "Tổng P phải nằm trong khoảng từ 0 đến 2 mg/L!")

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

        private void savechange_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInputs())
                {
                    return;
                }

                if(!edit)
                {
                
                    this.quanLyPhanTich_BLL.CapNhatTrangThaiMau(this.currentMaDH, this.currentViTriLayMau);
                }
               
                else
                {
                    this.quanLyPhanTich_BLL.CapNhatTrangThaiMau(this.currentMaDH, this.currentViTriLayMau);
                }

                bool result = this.quanLyPhanTich_BLL.CapNhatDuLieuNuocMat(new object[] { TB_NH4.Text, TB_NO3.Text, TB_PO4.Text, BoxCOD.Text, TB_TSS.Text, TB_tongN.Text, BoxTOC.Text, BoxtongP.Text, this.currentMaDH, this.currentViTriLayMau });

                if (result)
                {
                    MessageBox.Show("Dữ liệu đã được cập nhật thành công.");
                    if (this.lich_su.kiem_tra_mau(this.currentMaDH, this.currentViTriLayMau))
                    {
                        this.lich_su.them_lich_su_phong_lab_nuoc_mat(this.currentMaNV, this.currentMaDH, this.currentViTriLayMau);
                    }
                    //uc_lab_analysis_management1 uc_lab_analysis_management1 = new uc_lab_analysis_management1();
                    chi_tieu_nuoc_mat chi_tieu_nuoc_mat = new chi_tieu_nuoc_mat(currentMaDH, currentViTriLayMau, edit);
                    ((dashboard)this.ParentForm).ShowFormOnPanel(chi_tieu_nuoc_mat);
                }

                else
                {
                    MessageBox.Show("Không tìm thấy dữ liệu cần cập nhật.");
                }
            }

            catch (Exception ex)
            {
                errorMess.Text = ex.Message;
                errorMess.Visible = true;

            }
        }

        private void phan_tich_nuoc_mat_Load(object sender, EventArgs e)
        {
            if (edit)
            {
                DataTable NuocMatResultTable = this.quanLyPhanTich_BLL.LayDuLieuNuocMat(currentViTriLayMau, currentMaDH);

                // Kiểm tra kết quả
                if (NuocMatResultTable.Rows.Count > 0)
                {
                    foreach (DataRow row in NuocMatResultTable.Rows)
                    {
                        string viTri = row["ViTriLayMau"].ToString();
                        string nh4 = row["NH4"].ToString();
                        string no3 = row["NO3"].ToString();
                        string po4 = row["PO4"].ToString();
                        string cod = row["COD"].ToString();
                        string tss = row["TSS"].ToString();
                        string tongN = row["tongN"].ToString();
                        string toc = row["TOC"].ToString();
                        string tongP = row["tongP"].ToString();
                        string nhietDo = row["NhietDo"].ToString();
                        string maNV = row["MaNV"].ToString();
                        string maDH = row["MaDH"].ToString();

                        // Gán giá trị vào các TextBox
                        TB_NH4.Text = nh4;
                        TB_NO3.Text = no3;
                        TB_PO4.Text = po4;
                        BoxCOD.Text = cod;
                        TB_TSS.Text = tss;
                        TB_tongN.Text = tongN;
                        BoxTOC.Text = toc;
                        BoxtongP.Text = tongP;
                    }
                }

                DataTable chiTieuResultTable = this.quanLyPhanTich_BLL.LayChiTieuNuocMat(currentViTriLayMau, currentMaDH);

                // Kiểm tra kết quả
                if (chiTieuResultTable.Rows.Count > 0)
                {
                    DataRow nuocMatRow = NuocMatResultTable.Rows[0];
                    DataRow chiTieuRow = chiTieuResultTable.Rows[0];

                    // Lấy giá trị từ bảng NuocMat
                    float nh4Value = float.Parse(nuocMatRow["NH4"].ToString());
                    float no3Value = float.Parse(nuocMatRow["NO3"].ToString());
                    float po4Value = float.Parse(nuocMatRow["PO4"].ToString());
                    float codValue = float.Parse(nuocMatRow["COD"].ToString());
                    float tssValue = float.Parse(nuocMatRow["TSS"].ToString());
                    float tongNValue = float.Parse(nuocMatRow["tongN"].ToString());
                    float tocValue = float.Parse(nuocMatRow["TOC"].ToString());
                    float tongPValue = float.Parse(nuocMatRow["tongP"].ToString());

                    // Lấy giá trị từ bảng ChiTieuNuocMat
                    float chiTieuNH4 = float.Parse(chiTieuRow["NH4"].ToString());
                    float chiTieuNO3 = float.Parse(chiTieuRow["NO3"].ToString());
                    float chiTieuPO4 = float.Parse(chiTieuRow["PO4"].ToString());
                    float chiTieuCOD = float.Parse(chiTieuRow["COD"].ToString());
                    float chiTieuTSS = float.Parse(chiTieuRow["TSS"].ToString());
                    float chiTieuTongN = float.Parse(chiTieuRow["tongN"].ToString());
                    float chiTieuTOC = float.Parse(chiTieuRow["TOC"].ToString());
                    float chiTieuTongP = float.Parse(chiTieuRow["tongP"].ToString());

                    // So sánh và hiển thị kết quả vào các Label
                    vuotTongN.Text = CompareValue(tongNValue, chiTieuTongN, "Tong N");
                    vuotTongN.Visible = true;

                    vuotTongP.Text = CompareValue(tongPValue, chiTieuTongP, "Tong P");
                    vuotTongP.Visible = true;

                    vuotToc.Text = CompareValue(tocValue, chiTieuTOC, "TOC");
                    vuotToc.Visible = true;

                    vuotCod.Text = CompareValue(codValue, chiTieuCOD, "COD");
                    vuotCod.Visible = true;

                    vuotPo4.Text = CompareValue(po4Value, chiTieuPO4, "PO4");
                    vuotPo4.Visible = true;

                    vuotNh4.Text = CompareValue(nh4Value, chiTieuNH4, "NH4");
                    vuotNh4.Visible = true;

                    vuotTss.Text = CompareValue(tssValue, chiTieuTSS, "TSS");
                    vuotTss.Visible = true;

                    vuotNo3.Text = CompareValue(no3Value, chiTieuNO3, "NO3");
                    vuotNo3.Visible = true;
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
    }
}
