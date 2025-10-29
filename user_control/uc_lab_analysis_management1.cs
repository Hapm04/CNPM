using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using EcoProject.Nhap_lieu;
using System.Drawing.Imaging;
using System.IO;
using BLL;

namespace EcoProject.user_control
{

    public partial class uc_lab_analysis_management1 : UserControl
    {
        private string currentMaDH;
        private string currentViTriLayMau;
        private string currentMaNV;
        private string currentLoaiMau;
        QuanLyPhanTich_BLL quan_ly_pt;
        public bool IsEditMode { get; set; }

        public uc_lab_analysis_management1()
        {
            InitializeComponent();
            quan_ly_pt = new QuanLyPhanTich_BLL();
            LoadDonHang();
            LoadData();
            SetWatermarkImage();
        }

        private void SetWatermarkImage()
        {
            // Đường dẫn đến logo của bạn
            string imagePath = Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName, "logo.png");


            try
            {
                // Tải logo
                Image logo = Image.FromFile(imagePath);

                // Xử lý sự kiện Paint của Guna2DataGridView
                DGV_danh_sach_mau.Paint += (sender, e) =>
                {
                    DrawWatermark(e.Graphics, logo);
                };

                // Xử lý sự kiện Scroll của Guna2DataGridView
                DGV_danh_sach_mau.Scroll += (sender, e) =>
                {
                    DGV_danh_sach_mau.Invalidate(); // Yêu cầu vẽ lại DataGridView
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải ảnh logo. Lỗi: " + ex.Message);
            }
        }

        private void DrawWatermark(Graphics graphics, Image logo)
        {
            // Tạo một độ mờ cho ảnh
            float opacity = 0.1f; // Độ mờ (0.0f là trong suốt, 1.0f là không mờ)
            ColorMatrix matrix = new ColorMatrix();
            matrix.Matrix33 = opacity; // Đặt độ mờ vào alpha

            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(matrix);

            // Tính toán tỷ lệ kích thước của logo sao cho vừa với DataGridView
            float widthRatio = (float)DGV_danh_sach_mau.Width / logo.Width;
            float heightRatio = (float)DGV_danh_sach_mau.Height / logo.Height;
            float scaleRatio = Math.Min(widthRatio, heightRatio); // Chọn tỷ lệ nhỏ hơn để logo không bị kéo

            // Tính toán kích thước mới của logo
            int newWidth = (int)(logo.Width * scaleRatio);
            int newHeight = (int)(logo.Height * scaleRatio);

            // Tính toán vị trí căn giữa
            PointF point = new PointF((DGV_danh_sach_mau.Width - newWidth) / 2, (DGV_danh_sach_mau.Height - newHeight) / 2);

            // Vẽ logo lên bảng
            graphics.DrawImage(logo, new Rectangle((int)point.X, (int)point.Y, newWidth, newHeight), 0, 0, logo.Width, logo.Height, GraphicsUnit.Pixel, attributes);
        }

        private void uc_lab_analysis_management_Load(object sender, EventArgs e)
        {
            Btn_sua_du_lieu_hien_truong.Enabled = false;
            Btn_them_du_lieu_hien_truong.Enabled = false;
            Btn_sua_du_lieu_phan_tich.Enabled = false;
            Btn_them_du_lieu_phan_tich.Enabled = false;
        }

        private void Sua(object sender, EventArgs e)
        {

            if (!ValidateNgayNhapMau(currentMaDH))
            {
                return;
            }
            if (!string.IsNullOrEmpty(currentMaDH) && !string.IsNullOrEmpty(currentMaNV) && !string.IsNullOrEmpty(currentViTriLayMau))
            {
                // Lấy thông tin từ cell click
                DataGridViewRow selectedRow = DGV_danh_sach_mau.SelectedRows[0];
                string selectedLoaiMau = selectedRow.Cells["Loại Mẫu"].Value.ToString();
                IsEditMode = true;

                // Kiểm tra nếu thông tin đã thay đổi


                Form formToDisplay;
                switch (selectedLoaiMau)
                {


                    case "Khí thải":
                        formToDisplay = new hien_truong_khi_thai(selectedLoaiMau, currentMaDH, IsEditMode, currentViTriLayMau);
                        break;
                    case "Không khí xung quanh":
                        formToDisplay = new hien_truong_khong_khi(selectedLoaiMau, currentMaDH, IsEditMode, currentViTriLayMau);
                        break;
                    case "Nước mặt":
                        formToDisplay = new hien_truong_nuoc_mat(selectedLoaiMau, currentMaDH, IsEditMode, currentViTriLayMau);
                        break;
                    default:
                        // Handle the case when an invalid value is selected
                        MessageBox.Show("Invalid value selected for CBB_loai_mau.");
                        return;
                }
                ((dashboard)this.ParentForm).ShowFormOnPanel(formToDisplay);


            }
            else
            {
                // Hiển thị thông báo yêu cầu nhập đầy đủ thông tin
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void guna2HtmlLabel13_Click(object sender, EventArgs e)
        {

        }

        public bool ValidateNgayNhapMau(string maDonHang)
        {
            try
            {
                object result = quan_ly_pt.lay_ngay_xuat_phieu(maDonHang);

                if (result.ToString() == "")
                {     
                    return true;
                }
                
                errorMess.Text = $"Đơn hàng {maDonHang} đã xuất phiếu trả hàng. Không thể nhập mẫu!";
                errorMess.Visible = true;

                return false; 
            }
            catch
            {   
                errorMess.Text = $"Đã có lỗi xảy ra, vui lòng thử lại.";
                errorMess.Visible = true;
                return false;
            }
        }





        private void Btn_them_du_lieu_hien_truong_Click(object sender, EventArgs e)
        {
            // Step 1: Retrieve selected values from ComboBox controls
            //string selectedMaDonHang = CBB_ma_don_hang.SelectedItem.ToString();
            string selectedMaDonHang = TB_tim_kiem_theo_ma_don_hang.Text;
            if (!ValidateNgayNhapMau(selectedMaDonHang))
            {
                return;
            }
            errorMess.Visible = false;

            string selectedLoaiMau = CBB_loai_mau.SelectedItem.ToString();
            currentViTriLayMau = "";
            IsEditMode = false;

            // Step 2: Check if selectedMaDonHang exists in the database
            bool maDonHangExists = CheckMaDonHangExists(selectedMaDonHang);

            if (maDonHangExists)
            {
                // Step 3: Determine which form to display based on selectedLoaiMau
                Form formToDisplay;
                switch (selectedLoaiMau)
                {


                    case "Khí thải":
                        formToDisplay = new hien_truong_khi_thai(selectedLoaiMau, selectedMaDonHang, IsEditMode, currentViTriLayMau);
                        break;
                    case "Không khí xung quanh":
                        formToDisplay = new hien_truong_khong_khi(selectedLoaiMau, selectedMaDonHang, IsEditMode, currentViTriLayMau);
                        break;
                    case "Nước mặt":
                        formToDisplay = new hien_truong_nuoc_mat(selectedLoaiMau, selectedMaDonHang, IsEditMode, currentViTriLayMau);
                        break;
                    default:
                        // Handle the case when an invalid value is selected
                        MessageBox.Show("Invalid value selected for CBB_loai_mau.");
                        return;
                }

                // Step 5: Show the form on the panel control (panel1) of the dashboard.cs
                ((dashboard)this.ParentForm).ShowFormOnPanel(formToDisplay);
            }

            else
            {
                //MessageBox.Show("Selected MaDonHang does not exist in the database.");
                errorMess.Text = "Mã đơn hàng không tồn tại!";
                errorMess.Visible = true;
            }
        }

        private bool CheckMaDonHangExists(string maDonHang)
        {
            
            int count = (int)quan_ly_pt.kiem_tra_ma_dh(maDonHang);

            if (count > 0)
            {
                return true;
            }
            return false;

        }

        private void CBB_ma_don_hang_SelectedIndexChanged(object sender, EventArgs e)
        {
            Check_bbx_maDh_bbx_loaiMau();
        }

        private void LoadDonHang()
        {
            // Thực hiện truy vấn để lấy danh sách đơn hàng từ cơ sở dữ liệu
            
            DataTable dt = quan_ly_pt.lay_du_lieu_don_hang();

            // Xóa các mục hiện tại trong ComboBox
            CBB_ma_don_hang.Items.Clear();

            // Thêm các mã đơn hàng vào ComboBox
            foreach (DataRow row in dt.Rows)
            {
                string maDonHang = row["MaDH"].ToString();
                CBB_ma_don_hang.Items.Add(maDonHang);
            }
        }

        private void Check_bbx_maDh_bbx_loaiMau()
        {
            if (CheckMaDonHangExists(TB_tim_kiem_theo_ma_don_hang.Text) && CBB_loai_mau.SelectedItem != null)
            {
                Btn_them_du_lieu_hien_truong.Enabled = true;
                Btn_them_du_lieu_phan_tich.Enabled = false;
                Btn_sua_du_lieu_hien_truong.Enabled = false;
                Btn_sua_du_lieu_phan_tich.Enabled = false;
            }
        }

        private void Btn_them_du_lieu_phan_tich_Click(object sender, EventArgs e)
        {
            if (!ValidateNgayNhapMau(currentMaDH))
            {
                return;
            }

            if (!string.IsNullOrEmpty(currentMaDH) && !string.IsNullOrEmpty(currentMaNV) && !string.IsNullOrEmpty(currentViTriLayMau))
            {
                Form formToDisplay = null;
                IsEditMode = false;

                // Kiểm tra vị trí lấy mẫu và xác định form cần mở
                if (currentLoaiMau.Equals("Nước mặt"))
                {
                    //formToDisplay = new hien_truong_nuoc_mat(currentMaDH, currentMaNV);
                    formToDisplay = new phan_tich_nuoc_mat(currentMaDH, currentMaNV, currentViTriLayMau, IsEditMode);

                }
                else if (currentLoaiMau.Equals("Khí thải"))
                {
                    //formToDisplay = new hien_truong_khi_thai(currentMaDH, currentMaNV);
                    formToDisplay = new phan_tich_khi_thai(currentMaDH, currentMaNV, currentViTriLayMau, IsEditMode);

                }
                else
                {
                    //formToDisplay = new hien_truong_khong_khi(currentMaDH, currentMaNV);
                    formToDisplay = new phan_tich_khong_khi(currentMaDH, currentMaNV, currentViTriLayMau, IsEditMode);

                }

                // Hiển thị form đã được xác định
                if (formToDisplay != null)
                {
                    ((dashboard)this.ParentForm).ShowFormOnPanel(formToDisplay);
                }
            }

            else
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin Mã Đơn Hàng, Mã Nhân Viên và Vị Trí Lấy Mẫu.");
            }
        }

        private void LoadData()
        {       
            DataTable data = quan_ly_pt.lay_du_lieu_tong();

            DGV_danh_sach_mau.DataSource = data;
        }

        private void DGV_danh_sach_mau_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void DGV_chon_don_hang_de_nhap_phan_tich(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = DGV_danh_sach_mau.Rows[e.RowIndex];
                string maDonHang = selectedRow.Cells["Mã Đơn Hàng"].Value.ToString();
                string viTriLayMau = selectedRow.Cells["Vị Trí Lấy Mẫu"].Value.ToString();
                string trangThai = selectedRow.Cells["Trạng Thái"].Value.ToString();
                string ketQua = selectedRow.Cells["Kết Quả"].Value.ToString();
                string loaiMau = selectedRow.Cells["Loại Mẫu"].Value.ToString();

                currentMaDH = maDonHang;
                currentViTriLayMau = viTriLayMau;
                currentMaNV = SessionInfo.MaNV;
                currentLoaiMau = loaiMau;
                //cur

                // Kiểm tra giá trị của cột "Trạng Thái"
                if (trangThai == "Đã hoàn thành")
                {
                    // Nếu trạng thái là "Đã hoàn thành", vô hiệu hóa các nút liên quan
                    Btn_them_du_lieu_hien_truong.Enabled = false;
                    Btn_them_du_lieu_phan_tich.Enabled = false;
                    Btn_sua_du_lieu_hien_truong.Enabled = true;
                    Btn_sua_du_lieu_phan_tich.Enabled = true;
                }
                else
                {
                    // Nếu trạng thái không phải là "Đã hoàn thành", bật tất cả các nút
                    Btn_them_du_lieu_hien_truong.Enabled = false;
                    Btn_them_du_lieu_phan_tich.Enabled = true;
                    Btn_sua_du_lieu_hien_truong.Enabled = false;
                    Btn_sua_du_lieu_phan_tich.Enabled = false;
                }
            }
        }

        private void Btn_sua_du_lieu_phan_tich_Click(object sender, EventArgs e)
        {
            if (!ValidateNgayNhapMau(currentMaDH))
            {
                return;
            }

            IsEditMode = true;

            if (!string.IsNullOrEmpty(currentMaDH) && !string.IsNullOrEmpty(currentMaNV) && !string.IsNullOrEmpty(currentViTriLayMau))
            {
                Form formToDisplay = null;

                // Kiểm tra vị trí lấy mẫu và xác định form cần mở
                if (currentLoaiMau.Equals("Nước mặt"))
                {
                    formToDisplay = new phan_tich_nuoc_mat(currentMaDH, currentMaNV, currentViTriLayMau, IsEditMode);
                }

                else if (currentLoaiMau.Equals("Khí thải"))
                {
                    formToDisplay = new phan_tich_khi_thai(currentMaDH, currentMaNV, currentViTriLayMau, IsEditMode);
                }

                else
                {
                    formToDisplay = new phan_tich_khong_khi(currentMaDH, currentMaNV, currentViTriLayMau, IsEditMode);
                }

                // Hiển thị form đã được xác định
                if (formToDisplay != null)
                {
                    ((dashboard)this.ParentForm).ShowFormOnPanel(formToDisplay);
                }
            }

            else
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin Mã Đơn Hàng, Mã Nhân Viên và Vị Trí Lấy Mẫu.");
            }
        }

        private void dat_panel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CBB_loai_mau_SelectedIndexChanged(object sender, EventArgs e)
        {
            Check_bbx_maDh_bbx_loaiMau();
        }

        private DataTable TimKiemDonHang()
        {
            DataTable dt = new DataTable();

            string maDH = "%" + TB_tim_kiem_theo_ma_don_hang.Text.Trim() + "%";
            dt = quan_ly_pt.tim_kiem_don_hang(maDH);

            if (dt == null)
            {
                MessageBox.Show("Lỗi khi tìm kiếm!");
            }

            return dt;
        }


        private void TB_tim_kiem_theo_ma_don_hang_TextChanged(object sender, EventArgs e)
        {
            Check_bbx_maDh_bbx_loaiMau();
            errorMess.Visible = false;

            DataTable searchResult = TimKiemDonHang();
            DGV_danh_sach_mau.DataSource = searchResult;
        }
    }
}
