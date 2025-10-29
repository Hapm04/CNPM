using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using BLL;

namespace EcoProject.user_control
{
    public partial class uc_order : UserControl
    {

        QuanLyDonHang_BLL quanly;
        public uc_order()
        {
            InitializeComponent();
            SetWatermarkImage();
            quanly = new QuanLyDonHang_BLL();
        }

        private void Btn_TaoMoi(object sender, EventArgs e)
        {
            Add_Orders add_Orders = new Add_Orders();
            ((dashboard)this.ParentForm).ShowFormOnPanel(add_Orders);
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
            float widthRatio = (float)View.Width / logo.Width;
            float heightRatio = (float)View.Height / logo.Height;
            float scaleRatio = Math.Min(widthRatio, heightRatio); // Chọn tỷ lệ nhỏ hơn để logo không bị kéo

            // Tính toán kích thước mới của logo
            int newWidth = (int)(logo.Width * scaleRatio);
            int newHeight = (int)(logo.Height * scaleRatio);

            // Tính toán vị trí căn giữa
            PointF point = new PointF((View.Width - newWidth) / 2, (View.Height - newHeight) / 2);

            // Vẽ logo lên bảng
            graphics.DrawImage(logo, new Rectangle((int)point.X, (int)point.Y, newWidth, newHeight), 0, 0, logo.Width, logo.Height, GraphicsUnit.Pixel, attributes);
        }

        private void SetWatermarkImage()
        {
            // Lấy đường dẫn đến thư mục bin
            string imagePath = Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName, "logo.png");

            try
            {
                // Tải logo
                Image logo = Image.FromFile(imagePath);

                // Xử lý sự kiện Paint của Guna2DataGridView
                View.Paint += (sender, e) =>
                {
                    DrawWatermark(e.Graphics, logo);
                };

                // Xử lý sự kiện Scroll của Guna2DataGridView
                View.Scroll += (sender, e) =>
                {
                    View.Invalidate(); // Yêu cầu vẽ lại DataGridView
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải ảnh logo. Lỗi: " + ex.Message);
            }
        }

        private void loadData()
        {
            
            View.Rows.Clear();
            DataTable dt = new DataTable();
            dt = quanly.lay_du_lieu_don_hang();

            if (dt == null)
            {
                return;
            }

            foreach (DataRow row in dt.Rows)
            {
                int index = View.Rows.Add(); // Thêm một hàng mới và lấy chỉ số hàng
                DataGridViewRow newRow = View.Rows[index];

                // Gán giá trị cho các cột dựa trên chỉ số cột
                newRow.Cells[0].Value = row["MaDH"];
                newRow.Cells[1].Value = row["TenCongTy"];
                newRow.Cells[2].Value = Convert.ToDateTime(row["NgayTaoDH"]).ToString("dd/MM/yyyy");
                newRow.Cells[3].Value = Convert.ToDateTime(row["HanTraHang"]).ToString("dd/MM/yyyy");
            }
           
        }

        private void tim_kiem_don_hang(string madh, string ten)
        {
            View.Rows.Clear();

            DataTable dt = new DataTable();
            dt = quanly.tim_kiem(madh, ten); 
            if (dt == null) { return; }

            foreach (DataRow row in dt.Rows)
            {
                int index = View.Rows.Add(); // Thêm một hàng mới và lấy chỉ số hàng
                DataGridViewRow newRow = View.Rows[index];

                // Gán giá trị cho các cột dựa trên chỉ số cột
                newRow.Cells[0].Value = row["MaDH"];
                newRow.Cells[1].Value = row["TenCongTy"];
                newRow.Cells[2].Value = Convert.ToDateTime(row["NgayTaoDH"]).ToString("dd/MM/yyyy");
                newRow.Cells[3].Value = Convert.ToDateTime(row["HanTraHang"]).ToString("dd/MM/yyyy");
            }
        }

        private void uc_order_Load(object sender, EventArgs e)
        {
            loadData();
            load_data_processed();
        }

        

        private void View_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4 && e.RowIndex >= 0)
            {

                string madh = View.CurrentRow.Cells[0].Value.ToString();
                string tencongty = View.CurrentRow.Cells[1].Value.ToString();
                string ngaytao = View.CurrentRow.Cells[2].Value.ToString();
                string hantra = View.CurrentRow.Cells[3].Value.ToString();
                string quy = this.quanly.lay_quy(madh);
                Update_content up = new Update_content(madh, hantra, ngaytao, tencongty, quy);
                Form parentForm = this.FindForm();

                if (parentForm != null)
                {
                    // Đăng ký sự kiện khi form con (up) đóng
                    up.FormClosed += (s, args) =>
                    {
                        parentForm.Show(); // Hiển thị lại form cha
                        loadData();        // Gọi lại hàm load dữ liệu
                    };

                    // Hiển thị form con trong panel của dashboard
                    ((dashboard)this.ParentForm).ShowFormOnPanel(up);
                }
            }
            else if (e.ColumnIndex == 5 && e.RowIndex >= 0)
            {
                if (this.quanly.xoa_don_hang(View.CurrentRow.Cells[0].Value.ToString())) { return; }
                else { MessageBox.Show("Không thể xóa đơn hàng này!"); }
            }
        }

        private void load_data_processed()
        {
            
            ShowDanhSachDaXL.Rows.Clear();
            DataTable dt = new DataTable();
            dt = this.quanly.cong_ty_da_xu_ly();

            if (dt == null) { return; }
                
            foreach (DataRow row in dt.Rows)
            {
                int index = ShowDanhSachDaXL.Rows.Add(); // Thêm một hàng mới và lấy chỉ số hàng
                DataGridViewRow newRow = ShowDanhSachDaXL.Rows[index];

                // Gán giá trị cho các cột dựa trên chỉ số cột
                newRow.Cells[0].Value = row["MaKH"];
                newRow.Cells[1].Value = row["TenCongTy"];
                newRow.Cells[2].Value = Convert.ToDateTime(row["NgayXuatPhieuTraHang"]).ToString("dd/MM/yyyy");
            }
           

        }

        private void tbx_tim_kiem_cong_ty_TextChanged(object sender, EventArgs e)
        {
            if (tbx_tim_kiem_cong_ty.Text != "")
            {
                tim_kiem_cong_ty();
            }
            else
            {
                load_data_processed();
            }
        }

        private void tim_kiem_cong_ty()
        {
           
            ShowDanhSachDaXL.Rows.Clear();

            DataTable dt = this.quanly.tim_kiem_cong_ty(tbx_tim_kiem_cong_ty.Text);
            if (dt == null) { return; }
            foreach (DataRow row in dt.Rows)
            {
                int index = ShowDanhSachDaXL.Rows.Add(); // Thêm một hàng mới và lấy chỉ số hàng
                DataGridViewRow newRow = ShowDanhSachDaXL.Rows[index];

                // Gán giá trị cho các cột dựa trên chỉ số cột
                newRow.Cells[0].Value = row["MaKH"];
                newRow.Cells[1].Value = row["TenCongTy"];
                newRow.Cells[2].Value = Convert.ToDateTime(row["NgayXuatPhieuTraHang"]).ToString("dd/MM/yyyy");
            }
           
        }

        private void tboxTimKiem_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tboxTimKiem.Text) && string.IsNullOrEmpty(tbx_tim_ten_ct.Text))
            {
                // Nếu cả hai TextBox đều trống, gọi loadData
                loadData();
            }
            else
            {
                // Nếu có ít nhất một TextBox có giá trị, gọi tim_kiem_don_hang
                tim_kiem_don_hang(tboxTimKiem.Text, tbx_tim_ten_ct.Text);
            }
        }

        private void tbx_tim_ten_ct_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tboxTimKiem.Text) && string.IsNullOrEmpty(tbx_tim_ten_ct.Text))
            {
                // Nếu cả hai TextBox đều trống, gọi loadData
                loadData();
            }
            else
            {
                // Nếu có ít nhất một TextBox có giá trị, gọi tim_kiem_don_hang
                tim_kiem_don_hang(tboxTimKiem.Text, tbx_tim_ten_ct.Text);
            }
        }
    }
}
