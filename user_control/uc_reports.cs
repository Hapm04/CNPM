using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextFont = iTextSharp.text.Font;
using System.Collections.Generic;
using BLL;

namespace EcoProject.user_control
{
    public partial class uc_reports : UserControl
    {
        QuanLyPhieuTraHang_BLL qlpth = new QuanLyPhieuTraHang_BLL();
        public uc_reports()
        {
            InitializeComponent();
            lay_du_lieu();
        }

        private void lay_du_lieu()
        {
            DS_TH.Rows.Clear();
            DataTable dt = qlpth.lay_du_lieu();
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                DS_TH.Rows.Add();
                DS_TH.Rows[i].Cells[1].Value = dr["TenCongTy"].ToString();
                DS_TH.Rows[i].Cells[0].Value = dr["MaDH"].ToString();
                DS_TH.Rows[i].Cells[2].Value = Convert.ToDateTime(dr["NgayTaoDH"]).ToString("dd-MM-yyyy"); i++;
            }
        }

        private void CBB_Tim_Kiem_Theo_Don_Hang_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CBB_Tim_Kiem_Theo_Don_Hang.Text)) { lay_du_lieu(); }
            else
            {
                DS_TH.Rows.Clear();
                string ma_dh = '%' + CBB_Tim_Kiem_Theo_Don_Hang.Text + '%';
                DataTable dt = qlpth.tim_kiem( ma_dh );
                int i = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    DS_TH.Rows.Add();
                    DS_TH.Rows[i].Cells[1].Value = dr["TenCongTy"].ToString();
                    DS_TH.Rows[i].Cells[0].Value = dr["MaDH"].ToString();
                    DS_TH.Rows[i].Cells[2].Value = Convert.ToDateTime(dr["NgayTaoDH"]).ToString("dd-MM-yyyy");
                    i++;
                }
            }
        }

        private void DS_TH_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 3)
                {
                    string madh = DS_TH.Rows[e.RowIndex].Cells[0].Value.ToString();
                    xuat_PDF(madh);
                }
            }
        }

        PdfPCell can_giua_du_lieu(Phrase t, string text, iTextFont font)
        {
            t.Add(new Phrase(text, font));
            PdfPCell cell = new PdfPCell(t);
            cell.HorizontalAlignment = Element.ALIGN_CENTER; // Căn giữa ngang
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;  // Căn giữa dọc
            return cell;
        }

        private void xuat_PDF(string ma_dh)
        {
            Boolean check = lay_PDF(ma_dh);
            if (check == false)
            {
                DataTable dt1 = qlpth.thong_tin_khach_hang( ma_dh );
                DataTable dt2 = qlpth.loai_mau(ma_dh);
                DataTable dt3 = qlpth.so_mau(ma_dh);
                DataTable dt4 = qlpth.ngay_xuat_phieu(ma_dh);
                DataTable dt5 = qlpth.vi_tri_lay_mau( ma_dh );
                DataTable dt6 = qlpth.lay_du_lieu_kiem_dinh( ma_dh );
                string ten_mau = "";
                int i = 1;
                foreach (DataRow dr in dt2.Rows)
                {
                    if (i < dt2.Rows.Count)
                    {
                        ten_mau += (dr["LoaiMau"].ToString() + " - ");
                    }
                    else
                    {
                        ten_mau += dr["LoaiMau"].ToString();
                    }
                    i++;
                }
                // Hiển thị hộp thoại cho người dùng chọn nơi lưu file PDF
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "PDF file (*.pdf)|*.pdf",
                    Title = "Chọn nơi lưu file PDF",
                    FileName = $"PhieuTraHang_{ma_dh}.pdf"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string duong_dan = saveFileDialog.FileName;

                    // Khởi tạo document PDF
                    Document document = new Document(PageSize.A4);
                    PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(duong_dan, FileMode.Create));

                    string relativePath = @"Fonts\RBT\Roboto-Regular.ttf";
                    string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                    string fontPath = Path.Combine(projectDirectory, relativePath);
                    BaseFont bf = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    iTextFont robotoFont = new iTextFont(bf, 12, iTextFont.NORMAL); // Sử dụng alias iTextFont

                    // Mở document để ghi dữ liệu
                    document.Open();

                    Paragraph head = new Paragraph();
                    head.Add(new Phrase("CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM", new iTextFont(bf, 10, iTextFont.BOLD)));
                    head.Add(new Phrase("                              ", new iTextFont(bf, 10)));
                    head.Add(new Phrase("TRUNG TÂM MÔI TRƯỜNG VÀ KHOÁNG SẢN", new iTextFont(bf, 10)));
                    document.Add(head);

                    Paragraph head1 = new Paragraph();
                    head1.Add(new Phrase("             ", robotoFont));
                    head1.Add(new Phrase("Độc lập - Tự do - Hạnh phúc", new iTextFont(bf, 9, iTextFont.UNDERLINE)));
                    head1.Add(new Phrase("                                           ", new iTextFont(bf, 10)));
                    head1.Add(new Phrase("PHÒNG PHÂN TÍCH CHẤT LƯỢNG MÔI TRƯỜNG", new iTextFont(bf, 10, iTextFont.BOLD)));
                    document.Add(head1);

                    Paragraph dc = new Paragraph("Đ/c: LK423, Khu đất dịch vụ Yên Lộ, P.Yên Nghĩa, Q. Hà Đông, TP. Hà Nội", new iTextFont(bf, 10));
                    dc.Alignment = Element.ALIGN_RIGHT;
                    document.Add(dc);

                    Paragraph tel = new Paragraph("Tel: 024.32007660 - Hotline: 0775.034034                            ", new iTextFont(bf, 10));
                    tel.Alignment = Element.ALIGN_RIGHT;
                    document.Add(tel);
                    document.Add(new Paragraph("------------------------------------------------------------------------------------------------------------" +
                                                "--------------------------------------------------", robotoFont));

                    Image watermarkImage1 = Image.GetInstance(projectDirectory + @"\Logo_CongTy.png");

                    // Điều chỉnh kích thước hình ảnh
                    watermarkImage1.ScaleToFit(50f, 50f); // Điều chỉnh kích thước cho phù hợp với bảng
                    watermarkImage1.SetAbsolutePosition(100f, 725f); // Vị trí hình ảnh trên trang

                    // Lấy PdfContentByte để vẽ hình ảnh vào nền
                    PdfContentByte canvas1 = writer.DirectContentUnder;
                    PdfGState gState1 = new PdfGState();
                    gState1.FillOpacity = 1f; // Độ trong suốt: 0.0 (hoàn toàn trong suốt) -> 1.0 (hoàn toàn không trong suốt)

                    // Áp dụng trạng thái đồ họa vào canvas
                    canvas1.SaveState();
                    canvas1.SetGState(gState1);

                    // Thêm hình ảnh vào canvas
                    canvas1.AddImage(watermarkImage1);

                    // Khôi phục trạng thái đồ họa gốc
                    canvas1.RestoreState();

                    // Thêm tiêu đề
                    Paragraph title = new Paragraph("PHIẾU KẾT QUẢ PHÂN TÍCH\n", new iTextFont(bf, 16, iTextFont.BOLD));
                    title.Alignment = Element.ALIGN_CENTER;
                    document.Add(title);

                    // Thêm nội dung khác (phần đầu, địa chỉ, ngày, …)
                    document.Add(new Paragraph("\nTên khách hàng          : " + dt1.Rows[0]["TenCongTy"].ToString(), robotoFont));
                    Paragraph dia_chi = new Paragraph();
                    dia_chi.Add(new Phrase("Địa chỉ                          : ", robotoFont));
                    dia_chi.Add(new Phrase(dt1.Rows[0]["DiaChi"].ToString(), new iTextFont(bf, 10)));
                    document.Add(dia_chi);
                    Paragraph dia_chi_quan_trac = new Paragraph();
                    dia_chi_quan_trac.Add(new Phrase("Địa điểm quan trắc    : ", robotoFont));
                    dia_chi_quan_trac.Add(new Phrase(dt1.Rows[0]["DiaChi"].ToString(), new iTextFont(bf, 10)));
                    document.Add(dia_chi_quan_trac);
                    document.Add(new Paragraph("Tên mẫu                      : " + ten_mau, robotoFont));
                    document.Add(new Paragraph("Số mẫu                        : " + dt3.Rows[0]["SoLuongMau"].ToString(), robotoFont));
                    document.Add(new Paragraph("Ngày lấy mẫu             : " + Convert.ToDateTime(dt1.Rows[0]["NgayTaoDH"]).ToString("dd-MM-yyyy"), robotoFont));
                    document.Add(new Paragraph("Thời gian hoàn thành: " + Convert.ToDateTime(dt4.Rows[0]["NgayXuatPhieuTraHang"]).ToString("dd-MM-yyyy"), robotoFont));
                    document.Add(new Paragraph("\n", robotoFont));

                    Paragraph Ten_Bang = new Paragraph("BẢNG KẾT QUẢ KIỂM NGHIỆM\n\n", new iTextFont(bf, 14, iTextFont.BOLD));
                    Ten_Bang.Alignment = Element.ALIGN_CENTER;
                    document.Add(Ten_Bang);

                    PdfContentByte cb1 = writer.DirectContent;
                    string footerText1 = $"Trang 1/2";
                    cb1.BeginText();
                    cb1.SetFontAndSize(bf, 10);

                    // Tính toán chiều dài của text
                    float textWidth1 = bf.GetWidthPoint(footerText1, 10);

                    // Đặt vị trí cho footer, căn phải
                    float xPosition1 = document.PageSize.Width - document.RightMargin - textWidth1; // Lề phải - chiều dài text
                    float yPosition1 = document.BottomMargin - 20; // Khoảng cách từ đáy

                    cb1.SetTextMatrix(xPosition1, yPosition1); // Thiết lập vị trí
                    cb1.ShowText(footerText1);
                    cb1.EndText();

                    // Tạo bảng
                    PdfPTable table = new PdfPTable(5); // Số cột trong bảng

                    // Định nghĩa độ rộng cho các cột
                    float[] columnWidths = { 1f, 2f, 1f, 2f, 2f };
                    table.SetWidths(columnWidths);
                    table.WidthPercentage = 100;

                    // Thêm các tiêu đề cột
                    table.AddCell(can_giua_du_lieu(new Phrase(), "STT", new iTextFont(bf, 13, iTextFont.BOLD)));
                    table.AddCell(can_giua_du_lieu(new Phrase(), "Thông số", new iTextFont(bf, 13, iTextFont.BOLD)));
                    table.AddCell(can_giua_du_lieu(new Phrase(), "Đơn vị", new iTextFont(bf, 13, iTextFont.BOLD)));
                    table.AddCell(can_giua_du_lieu(new Phrase(), "Kết quả phân tích", new iTextFont(bf, 13, iTextFont.BOLD)));
                    table.AddCell(can_giua_du_lieu(new Phrase(), "QCVN\n03:2019/BYT", new iTextFont(bf, 13, iTextFont.BOLD)));

                    // Tạo hình ảnh
                    Image watermarkImage = Image.GetInstance(projectDirectory + @"\logo.png");

                    // Điều chỉnh kích thước hình ảnh
                    watermarkImage.ScaleToFit(250f, 250f); // Điều chỉnh kích thước cho phù hợp với bảng
                    watermarkImage.SetAbsolutePosition(140f, 240f); // Vị trí hình ảnh trên trang

                    // Lấy PdfContentByte để vẽ hình ảnh vào nền
                    PdfContentByte canvas = writer.DirectContentUnder;
                    PdfGState gState = new PdfGState();
                    gState.FillOpacity = 0.3f; // Độ trong suốt: 0.0 (hoàn toàn trong suốt) -> 1.0 (hoàn toàn không trong suốt)

                    // Áp dụng trạng thái đồ họa vào canvas
                    canvas.SaveState();
                    canvas.SetGState(gState);

                    // Thêm hình ảnh vào canvas
                    canvas.AddImage(watermarkImage);

                    // Khôi phục trạng thái đồ họa gốc
                    canvas.RestoreState();

                    Phrase do_C = new Phrase();
                    Chunk s_do_C = new Chunk("o", new iTextFont(bf, 7));
                    s_do_C.SetTextRise(5); // Giá trị âm để thụt xuống
                    do_C.Add(s_do_C);
                    do_C.Add(new Phrase("C", robotoFont));

                    Phrase no2 = new Phrase();
                    Chunk s_no2 = new Chunk("2", new iTextFont(bf, 7));
                    s_no2.SetTextRise(-2); // Giá trị âm để thụt xuống
                    no2.Add(new Phrase("NO", robotoFont));
                    no2.Add(s_no2);

                    Phrase so2 = new Phrase();
                    Chunk s_so2 = new Chunk("2", new iTextFont(bf, 7));
                    s_so2.SetTextRise(-2); // Giá trị âm để thụt xuống
                    so2.Add(new Phrase("SO", robotoFont));
                    so2.Add(s_so2);

                    Phrase o2 = new Phrase();
                    Chunk s_o2 = new Chunk("2", new iTextFont(bf, 7));
                    s_o2.SetTextRise(-2); // Giá trị âm để thụt xuống
                    o2.Add(new Phrase("O", robotoFont));
                    o2.Add(s_o2);

                    Phrase h2s = new Phrase();
                    Chunk s_h2s = new Chunk("2", new iTextFont(bf, 7));
                    s_h2s.SetTextRise(-2); // Giá trị âm để thụt xuống
                    h2s.Add(new Phrase("H", robotoFont));
                    h2s.Add(s_h2s);
                    h2s.Add(new Phrase("S", robotoFont));

                    Phrase nh3 = new Phrase();
                    Chunk s_nh3 = new Chunk("3", new iTextFont(bf, 7));
                    s_nh3.SetTextRise(-2); // Giá trị âm để thụt xuống
                    nh3.Add(new Phrase("NH", robotoFont));
                    nh3.Add(s_nh3);

                    Phrase o3 = new Phrase();
                    Chunk s_o3 = new Chunk("3", new iTextFont(bf, 7));
                    s_o3.SetTextRise(-2); // Giá trị âm để thụt xuống
                    o3.Add(new Phrase("O", robotoFont));
                    o3.Add(s_o3);

                    Phrase pm10 = new Phrase();
                    Chunk s_pm10 = new Chunk("10", new iTextFont(bf, 7));
                    s_pm10.SetTextRise(-2); // Giá trị âm để thụt xuống
                    pm10.Add(new Phrase("PM", robotoFont));
                    pm10.Add(s_pm10);

                    Phrase pm25 = new Phrase();
                    Chunk s_pm25 = new Chunk("2.5", new iTextFont(bf, 7));
                    s_pm25.SetTextRise(-2); // Giá trị âm để thụt xuống
                    pm25.Add(new Phrase("PM", robotoFont));
                    pm25.Add(s_pm25);

                    Phrase no3 = new Phrase();
                    Chunk s_no3_1 = new Chunk("3", new iTextFont(bf, 7));
                    s_no3_1.SetTextRise(-2);
                    Chunk s_no3_2 = new Chunk("-", new iTextFont(bf, 7));
                    s_no3_2.SetTextRise(5);
                    no3.Add(new Phrase("NO", robotoFont));
                    no3.Add(s_no3_1);
                    no3.Add(s_no3_2);

                    Phrase po4 = new Phrase();
                    Chunk s_po4_1 = new Chunk("4", new iTextFont(bf, 7));
                    s_po4_1.SetTextRise(-2);
                    Chunk s_po4_2 = new Chunk("3-", new iTextFont(bf, 7));
                    s_po4_2.SetTextRise(5);
                    po4.Add(new Phrase("PO", robotoFont));
                    po4.Add(s_po4_1);
                    po4.Add(s_po4_2);

                    Phrase nh4 = new Phrase();
                    Chunk s_nh4_1 = new Chunk("4", new iTextFont(bf, 7));
                    s_nh4_1.SetTextRise(-2);
                    Chunk s_nh4_2 = new Chunk("+", new iTextFont(bf, 7));
                    s_nh4_2.SetTextRise(5);
                    nh4.Add(new Phrase("NH", robotoFont));
                    nh4.Add(s_nh4_1);
                    nh4.Add(s_nh4_2);

                    int j = 1;
                    foreach (DataRow dr1 in dt6.Rows)
                    {
                        string loai_mau = dr1["LoaiMau"].ToString();
                        switch (loai_mau)
                        {
                            case "Không khí xung quanh":
                                Phrase[] chi_tieu_khong_khi = { new Phrase("CO", robotoFont), so2, o3, pm10, pm25, new Phrase("Nhiệt độ", robotoFont), no2 };
                                string[] ctkk = { "CO", "SO2", "O3", "PM10", "PM2dot5", "NhietDo", "NO2" };
                                int m = 0;
                                Phrase don_vi_khong_khi;

                                Dictionary<string, string> d = new Dictionary<string, string>
                                {
                                    { "NhietDo", "-" },
                                    { "NO2", "200" },
                                    { "CO", "30000" },
                                    { "SO2", "350" },
                                    { "O3", "180" },
                                    { "PM10", "150" },
                                    { "PM2dot5", "50" }
                                };

                                foreach (Phrase chi_tieu in chi_tieu_khong_khi)
                                {
                                    if (chi_tieu.Content == "Nhiệt độ") { don_vi_khong_khi = do_C; } else { don_vi_khong_khi = new Phrase("\u00B5g/Nm\u00B3", robotoFont); }
                                    table.AddCell(can_giua_du_lieu(new Phrase(), j.ToString(), robotoFont));
                                    j++;
                                    table.AddCell(can_giua_du_lieu(chi_tieu, " (KK)", robotoFont));
                                    table.AddCell(can_giua_du_lieu(don_vi_khong_khi, "", robotoFont));
                                    table.AddCell(can_giua_du_lieu(new Phrase(), dr1[$"KhongKhi_{ctkk[m]}"].ToString(), robotoFont));
                                    table.AddCell(can_giua_du_lieu(new Phrase(), d[ctkk[m]], robotoFont));
                                    m++;
                                }
                                break;

                            case "Khí thải":
                                Phrase[] chi_tieu_khi_thai = { new Phrase("NO", robotoFont), new Phrase("CO", robotoFont), so2, no2, o2,
                                                        new Phrase("Hg", robotoFont), new Phrase("PM", robotoFont), nh3, new Phrase("Áp suất", robotoFont), new Phrase("Nhiệt độ", robotoFont), h2s };
                                string[] ctkt = { "N_O", "CO", "SO2", "NO2", "O2", "Hg", "PM", "NH3", "ApSuat", "NhietDo", "H2S" };
                                int n = 0;
                                Phrase don_vi_khi_thai;
                                Dictionary<string, string> d1 = new Dictionary<string, string>
                                {
                                    { "NhietDo", "-" },
                                    { "ApSuat", "-" },
                                    { "N_O", "1000" },
                                    { "NO2", "800" },
                                    { "CO", "1000" },
                                    { "SO2", "1000"},
                                    { "O2", "-"},
                                    { "NH3", "5" },
                                    { "Hg", "1" },
                                    { "H2S", "14" },
                                    { "PM", "200" }
                                };


                                foreach (Phrase chi_tieu in chi_tieu_khi_thai)
                                {
                                    if (chi_tieu.Content == "Nhiệt độ") { don_vi_khi_thai = do_C; }
                                    else if (chi_tieu.Content == "Áp suất") { don_vi_khi_thai = new Phrase("kPa", robotoFont); }
                                    else if (chi_tieu.Content == "O2") { don_vi_khi_thai = new Phrase("%V", robotoFont); }
                                    else { don_vi_khi_thai = new Phrase("mg/m\u00B3", robotoFont); }
                                    table.AddCell(can_giua_du_lieu(new Phrase(), j.ToString(), robotoFont));
                                    j++;
                                    table.AddCell(can_giua_du_lieu(chi_tieu, " (KT)", robotoFont));
                                    table.AddCell(can_giua_du_lieu(don_vi_khi_thai, "", robotoFont));
                                    table.AddCell(can_giua_du_lieu(new Phrase(), dr1[$"KhiThai_{ctkt[n]}"].ToString(), robotoFont));
                                    table.AddCell(can_giua_du_lieu(new Phrase(), d1[ctkt[n]], robotoFont));
                                    n++;
                                }
                                break;

                            case "Nước mặt":
                                Phrase[] chi_tieu_nuoc_mat = { new Phrase("Nhiệt độ", robotoFont), new Phrase("pH", robotoFont), new Phrase("TSS", robotoFont), new Phrase("COD", robotoFont),
                                                        new Phrase("DO", robotoFont), no3, po4, nh4, new Phrase("Tổng P", robotoFont), new Phrase("Tổng N", robotoFont),
                                                        new Phrase("TOC", robotoFont), new Phrase("TDS", robotoFont) };
                                string[] ctnm = { "NhietDo", "pH", "TSS", "COD", "DO", "NO3", "PO4", "NH4", "TongP", "TongN", "TOC", "TDS" };
                                int k = 0;
                                Dictionary<string, string> d2 = new Dictionary<string, string>
                                {
                                    { "pH", "8.5" },
                                    { "TSS", "50" },
                                    { "COD", "30" },
                                    { "DO", "4" },
                                    { "NO3", "10" },
                                    { "PO4", "0.3" },
                                    { "NH4", "0.9" },
                                    { "TongP", "0.3" },
                                    { "TongN", "40" },
                                    { "TOC", "100" },
                                    { "TDS", "1000" },
                                    { "NhietDo", "-" }
                                };
                                Phrase don_vi_nuoc_mat;

                                foreach (Phrase chi_tieu in chi_tieu_nuoc_mat)
                                {
                                    if (chi_tieu.Content == "Nhiệt độ") { don_vi_nuoc_mat = do_C; } 
                                    else if (chi_tieu.Content == "pH") { don_vi_nuoc_mat = new Phrase("-", robotoFont); } 
                                    else { don_vi_nuoc_mat = new Phrase("mg/L", robotoFont); }

                                    table.AddCell(can_giua_du_lieu(new Phrase(), j.ToString(), robotoFont));
                                    j++;
                                    table.AddCell(can_giua_du_lieu(chi_tieu, " (NM)", robotoFont));
                                    table.AddCell(can_giua_du_lieu(don_vi_nuoc_mat, "", robotoFont));
                                    table.AddCell(can_giua_du_lieu(new Phrase(), dr1[$"NuocMat_{ctnm[k]}"].ToString(), robotoFont));
                                    table.AddCell(can_giua_du_lieu(new Phrase(), d2[ctnm[k]], robotoFont));
                                    k++;
                                }
                                break;
                        }
                    }
                    document.Add(table);

                    // Ghi chú
                    document.Add(new Paragraph("\nGhi chú:", new iTextFont(bf, 12, iTextFont.BOLD)));
                    document.Add(new Paragraph("     - Vị trí lấy mẫu:", new iTextFont(bf, 12, iTextFont.BOLD)));
                    foreach (DataRow dr2 in dt5.Rows)
                    {
                        document.Add(new Paragraph("        + " + dr2["ViTriLayMau"].ToString() + " (" + dr2["LoaiMau"].ToString() + ").", robotoFont));
                    }
                    document.Add(new Paragraph("     - Chú thích:", new iTextFont(bf, 12, iTextFont.BOLD)));
                    document.Add(new Paragraph("        + KK: Không khí xung quanh.", robotoFont));
                    document.Add(new Paragraph("        + KT: Khí thải.", robotoFont));
                    document.Add(new Paragraph("        + NM: Nước mặt.\n\n", robotoFont));

                    DateTime now = DateTime.Now;
                    Paragraph da = new Paragraph($"\n\nThành phố Hồ Chí Minh, ngày {now.Day} tháng {now.Month} năm {now.Year}", robotoFont);
                    da.Alignment = Element.ALIGN_RIGHT;
                    document.Add(da);
                    Paragraph gd = new Paragraph("               TM PHÒNG PHÂN TÍCH" +
                                                "                                             " +
                                                "GIÁM ĐỐC", new iTextFont(bf, 14, iTextFont.BOLD));
                    document.Add(gd);

                    PdfContentByte cb = writer.DirectContent;
                    string footerText = $"Trang 2/2";
                    cb.BeginText();
                    cb.SetFontAndSize(bf, 10);

                    // Tính toán chiều dài của text
                    float textWidth = bf.GetWidthPoint(footerText, 10);

                    // Đặt vị trí cho footer, căn phải
                    float xPosition = document.PageSize.Width - document.RightMargin - textWidth; // Lề phải - chiều dài text
                    float yPosition = document.BottomMargin - 20; // Khoảng cách từ đáy

                    cb.SetTextMatrix(xPosition, yPosition); // Thiết lập vị trí
                    cb.ShowText(footerText);
                    cb.EndText();

                    // Đóng tài liệu PDF
                    document.Close();

                    MessageBox.Show("File PDF đã được tạo thành công!", "Thông báo");

                    byte[] pdfBytes = File.ReadAllBytes(duong_dan);
                    qlpth.cap_nhat_phieu_tra_hang( pdfBytes, ma_dh );
                }
            }
        }

        private bool lay_PDF(string ma_dh)
        {
            DataTable dt = qlpth.lay_phieu_tra_hang( ma_dh );

            if (dt.Rows.Count > 0 && dt.Rows[0]["PhieuTraHang"] != DBNull.Value)
            {
                byte[] pdfBytes = (byte[])dt.Rows[0]["PhieuTraHang"];

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "PDF Files (*.pdf)|*.pdf",
                    FileName = $"PhieuTraHang_{ma_dh}.pdf"
                };

                // Nếu người dùng chọn vị trí và nhấn Save
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;
                    File.WriteAllBytes(filePath, pdfBytes);
                    System.Diagnostics.Process.Start(filePath);
                }
                return true;
            }
            else
            {
                return false; // Nếu không có dữ liệu
            }
        }
    }
}