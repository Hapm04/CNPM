using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using BLL;

namespace EcoProject.user_control
{
    public partial class uc_notifications : UserControl
    {
        QuanLyThongBao_BLL quan_ly_thong_bao;
        public uc_notifications()
        {
            InitializeComponent();
            SetWatermarkImage();
            quan_ly_thong_bao = new QuanLyThongBao_BLL();
        }


        private void SetWatermarkImage()
        {
            // Đường dẫn đến logo của bạn
            // Lấy đường dẫn đến thư mục bin
            string imagePath = Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName, "logo.png");

            try
            {
                // Tải logo
                Image logo = Image.FromFile(imagePath);

                // Xử lý sự kiện Paint của Guna2DataGridView
                dtgrid_thong_bao.Paint += (sender, e) =>
                {
                    DrawWatermark(e.Graphics, logo);
                };

                // Xử lý sự kiện Scroll của Guna2DataGridView
                dtgrid_thong_bao.Scroll += (sender, e) =>
                {
                    dtgrid_thong_bao.Invalidate(); // Yêu cầu vẽ lại DataGridView
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
            float widthRatio = (float)dtgrid_thong_bao.Width / logo.Width;
            float heightRatio = (float)dtgrid_thong_bao.Height / logo.Height;
            float scaleRatio = Math.Min(widthRatio, heightRatio); // Chọn tỷ lệ nhỏ hơn để logo không bị kéo

            // Tính toán kích thước mới của logo
            int newWidth = (int)(logo.Width * scaleRatio);
            int newHeight = (int)(logo.Height * scaleRatio);

            // Tính toán vị trí căn giữa
            PointF point = new PointF((dtgrid_thong_bao.Width - newWidth) / 2, (dtgrid_thong_bao.Height - newHeight) / 2);

            // Vẽ logo lên bảng
            graphics.DrawImage(logo, new Rectangle((int)point.X, (int)point.Y, newWidth, newHeight), 0, 0, logo.Width, logo.Height, GraphicsUnit.Pixel, attributes);
        }

        private void Btn_Tat_Ca_Click(object sender, EventArgs e)
        {
            load_all();
        }

        private void uc_notifications_Load_1(object sender, EventArgs e)
        {
            load_all();
        }

        private void load_all()
        {
            dtgrid_thong_bao.Rows.Clear();
            DataTable dt = new DataTable();
            dt = this.quan_ly_thong_bao.lay_du_lieu();

            if (dt == null) { return; }

            foreach (DataRow dr in dt.Rows)
            {
                DateTime now = DateTime.Now;
                DateTime dateTime = DateTime.Parse(dr[2].ToString());
                TimeSpan timeSpan = dateTime.Subtract(now);
                string note = "";

                if (timeSpan.Days < 0)
                {
                    note = "Quá hạn";
                }
                else if (timeSpan.Days < 4)
                {
                    note = "Sắp đến hạn";
                }
                else { continue; }

                int index = dtgrid_thong_bao.Rows.Add(); // Thêm một hàng mới và lấy chỉ số hàng
                DataGridViewRow newRow = dtgrid_thong_bao.Rows[index];

                // Gán giá trị cho các cột dựa trên chỉ số cột
                newRow.Cells[0].Value = dr["TenCongTy"];
                newRow.Cells[1].Value = dr["MaDH"];
                newRow.Cells[2].Value = Convert.ToDateTime(dr["HanTraHang"]).ToString("dd/MM/yyyy");
                newRow.Cells[3].Value = note;
            }
        }

        private void Btn_Qua_Han_Click(object sender, EventArgs e)
        {
            dtgrid_thong_bao.Rows.Clear();
            DataTable dt = new DataTable();
            dt = this.quan_ly_thong_bao.qua_han();

            if (dt == null) { return; }

            foreach (DataRow dr in dt.Rows)
            {
                DateTime now = DateTime.Now;
                DateTime dateTime = DateTime.Parse(dr[2].ToString());
                TimeSpan timeSpan = dateTime.Subtract(now);
                string note = "";

                if (timeSpan.Days < 0)
                {
                    note = "Quá hạn";
                }
                else { continue; }

                int index = dtgrid_thong_bao.Rows.Add(); // Thêm một hàng mới và lấy chỉ số hàng
                DataGridViewRow newRow = dtgrid_thong_bao.Rows[index];

                // Gán giá trị cho các cột dựa trên chỉ số cột
                newRow.Cells[0].Value = dr["TenCongTy"];
                newRow.Cells[1].Value = dr["MaDH"];
                newRow.Cells[2].Value = Convert.ToDateTime(dr["HanTraHang"]).ToString("dd/MM/yyyy");
                newRow.Cells[3].Value = note;
            }
        }

        private void Btn_Sap_Het_Han_Click(object sender, EventArgs e)
        {
            dtgrid_thong_bao.Rows.Clear();
            DataTable dt = new DataTable();
            dt = this.quan_ly_thong_bao.sap_qua_han();

            if (dt == null) { return; }

            foreach (DataRow dr in dt.Rows)
            {
                DateTime now = DateTime.Now;
                DateTime dateTime = DateTime.Parse(dr[2].ToString());
                TimeSpan timeSpan = dateTime.Subtract(now);
                string note = "";

                if (timeSpan.Days <= 4)
                {
                    note = "Sắp đến hạn";
                }
                else { continue; }

                int index = dtgrid_thong_bao.Rows.Add(); // Thêm một hàng mới và lấy chỉ số hàng
                DataGridViewRow newRow = dtgrid_thong_bao.Rows[index];

                // Gán giá trị cho các cột dựa trên chỉ số cột
                newRow.Cells[0].Value = dr["TenCongTy"];
                newRow.Cells[1].Value = dr["MaDH"];
                newRow.Cells[2].Value = Convert.ToDateTime(dr["HanTraHang"]).ToString("dd/MM/yyyy");
                newRow.Cells[3].Value = note;
            }
        }
    }
}
