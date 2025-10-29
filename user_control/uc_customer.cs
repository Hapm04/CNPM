using BLL;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace EcoProject.user_control
{
    public partial class uc_customer : UserControl
    {
        QuanLyKhachHang_BLL qlkh = new QuanLyKhachHang_BLL();
        public uc_customer()
        {
            InitializeComponent();
            lay_du_lieu();
        }

        private void lay_du_lieu()
        {
            ShowDanhSach.Rows.Clear();
            DataTable dt = qlkh.lay_du_lieu();
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                ShowDanhSach.Rows.Add();
                ShowDanhSach.Rows[i].Cells[0].Value = dr["TenCongTy"].ToString();
                ShowDanhSach.Rows[i].Cells[1].Value = dr["MaDH"].ToString();
                ShowDanhSach.Rows[i].Cells[2].Value = dr["NguoiDaiDien"].ToString();
                ShowDanhSach.Rows[i].Cells[3].Value = dr["SDT"].ToString();
                ShowDanhSach.Rows[i].Cells[4].Value = dr["DiaChi"].ToString();
                i++;
            }
        }

        private void Btn_Them_Khach_Hang(object sender, EventArgs e)
        {
            Add_customer add_Customer = new Add_customer();
            ((dashboard)this.ParentForm).ShowFormOnPanel(add_Customer);
        }

        private void TimKiem(object sender, EventArgs e)
        {
                string ten_ct = TB_TimKimCTy.Text;
                string ma_dh = TB_TimKiemDH.Text;
                string tinh = ChonTinh.Text;

                if (tinh == "Chọn tỉnh") { tinh = ""; }

                if (string.IsNullOrWhiteSpace(ten_ct) && string.IsNullOrWhiteSpace(ma_dh) && string.IsNullOrWhiteSpace(tinh))
                {
                    lay_du_lieu();
                }
                else
                {
                    DataTable dt = qlkh.tim_kiem(ten_ct, ma_dh, tinh);
                    if (dt == null)
                    {
                        return;
                    }
                    int i = 0;
                    ShowDanhSach.Rows.Clear();
                    foreach (DataRow dr in dt.Rows)
                    {
                        ShowDanhSach.Rows.Add();
                        ShowDanhSach.Rows[i].Cells[0].Value = dr["TenCongTy"].ToString();
                        ShowDanhSach.Rows[i].Cells[1].Value = dr["MaDH"].ToString();
                        ShowDanhSach.Rows[i].Cells[2].Value = dr["NguoiDaiDien"].ToString();
                        ShowDanhSach.Rows[i].Cells[3].Value = dr["SDT"].ToString();
                        ShowDanhSach.Rows[i].Cells[4].Value = dr["DiaChi"].ToString();
                        i++;
                    }
                }
        }
        

        private void ShowDanhSach_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Lb_loi_sua_tt.Visible = false;
            if (e.RowIndex >= 0 && e.ColumnIndex == 5)
            {
                string ma_dh = ShowDanhSach.Rows[e.RowIndex].Cells[1].Value.ToString();
                int result = qlkh.kiem_tra_don_hang(ma_dh);
                if (result > 0)
                {
                    Lb_loi_sua_tt.Visible = true;
                }
                else
                {
                    DataTable dt = qlkh.lay_thong_tin_khach_hang(ma_dh);
                    foreach (DataRow dr in dt.Rows)
                    {
                        Update_customer update_Customer = new Update_customer
                        (
                            dr["MaKH"].ToString(),
                            dr["NguoiDaiDien"].ToString(),
                            dr["TenCongTy"].ToString(),
                            dr["Email"].ToString(),
                            dr["DiaChi"].ToString(),
                            dr["NganhCongNghiep"].ToString(),
                            dr["SDT"].ToString(),
                            dr["GhiChu"].ToString()
                        );
                        Form parentForm = this.FindForm();

                        if (parentForm != null)
                        {
                            // Đăng ký sự kiện khi form con (up) đóng
                            update_Customer.FormClosed += (s, args) =>
                            {
                                parentForm.Show(); // Hiển thị lại form cha
                                lay_du_lieu();        // Gọi lại hàm load dữ liệu
                            };

                            // Hiển thị form con trong panel của dashboard
                            ((dashboard)this.ParentForm).ShowFormOnPanel(update_Customer);
                        }
                    }
                }
            }
        }

        private void TB_TimKimCTy_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
    }
}
