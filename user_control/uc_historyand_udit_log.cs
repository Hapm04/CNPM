using System;
using System.Data;
using System.Windows.Forms;
using BLL;

namespace EcoProject.user_control
{
    public partial class uc_historyand_udit_log : UserControl
    {
        LichSu_BLL lich_su;
        public uc_historyand_udit_log()
        {
            InitializeComponent();
            lich_su = new LichSu_BLL();
        }

        private void TimKiem(object sender, EventArgs e)
        {
            if (timkiem.Text != "")
            {
                lay_du_lieu_tim_kiem();
            }
            else
            {
                lay_du_lieu();
            }
        }

        private void History_Load(object sender, EventArgs e)
        {
            lay_du_lieu();
        }

        private void lay_du_lieu()
        {
            try
            {
                dtgrid_lich_su.Rows.Clear();
                DataTable dt = new DataTable();
                dt = this.lich_su.lay_du_lieu();

                if (dt == null) { return; }


                foreach (DataRow dr in dt.Rows)
                {
                    int index = dtgrid_lich_su.Rows.Add(); // Thêm một hàng mới và lấy chỉ số hàng
                    DataGridViewRow newRow = dtgrid_lich_su.Rows[index];

                    // Gán giá trị cho các cột dựa trên chỉ số cột
                    newRow.Cells[0].Value = dr["MaNV"];
                    newRow.Cells[0].ToolTipText = this.lich_su.lay_ten_nhan_vien(dr["MaNV"].ToString());
                    newRow.Cells[1].Value = dr["MaDH"];
                    newRow.Cells[2].Value = Convert.ToDateTime(dr["ThoiGianChinhSua"]).ToString("yyyy/MM/dd HH:mm:ss.fff");
                }
            }
            catch { return; }
        }

        private void lay_du_lieu_tim_kiem()
        {
            dtgrid_lich_su.Rows.Clear();
                
            DataTable dt = new DataTable();
            dt = this.lich_su.lay_du_lieu_tim_kiem(timkiem.Text);

            if (dt == null) { return; }

            foreach (DataRow dr in dt.Rows)
            {
                int index = dtgrid_lich_su.Rows.Add(); // Thêm một hàng mới và lấy chỉ số hàng
                DataGridViewRow newRow = dtgrid_lich_su.Rows[index];

                // Gán giá trị cho các cột dựa trên chỉ số cột
                newRow.Cells[0].Value = dr["MaNV"];
                newRow.Cells[0].ToolTipText = this.lich_su.lay_ten_nhan_vien(dr["MaNV"].ToString());
                newRow.Cells[1].Value = dr["MaDH"];
                newRow.Cells[2].Value = Convert.ToDateTime(dr["ThoiGianChinhSua"]).ToString("yyyy/MM/dd HH:mm:ss.fff");
            }
        }

        

        private void dtgrid_lich_su_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 3)
            {

                string manv = dtgrid_lich_su.CurrentRow.Cells[0].Value.ToString();
                string madh = dtgrid_lich_su.CurrentRow.Cells[1].Value.ToString();
                string thoigianStr = dtgrid_lich_su.CurrentRow.Cells[2].Value.ToString(); // Lấy giá trị thời gian từ cell
                MessageBox.Show(this.lich_su.noi_dung_chinh_sua(manv, madh, thoigianStr));
            }
        }
    }
}
