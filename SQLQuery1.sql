CREATE TABLE Nhanvien
(
  Hoten	NVARCHAR(50) NOT NULL,
  Ngaysinh DATE NOT NULL,
  MaNV VARCHAR(10) NOT NULL,
  Email VARCHAR(50) NOT NULL,
  MatKhau VARCHAR(20) NOT NULL,
  PRIMARY KEY (MaNV)
);

CREATE TABLE KhachHang
(
  MaKH VARCHAR(10) NOT NULL,
  TenCongty NVARCHAR(100) NOT NULL,
  Email VARCHAR(50) NOT NULL,
  DiaChi NVARCHAR(200) NOT NULL,
  SDT VARCHAR(10) NOT NULL,
  NguoiDaiDien NVARCHAR(50) NOT NULL,
  GhiChu NVARCHAR(200) NOT NULL,
  NganhCongNghiep NVARCHAR(50) NOT NULL,
  PRIMARY KEY (MaKH)
);

CREATE TABLE DonHang
(
  MaDH VARCHAR(10) NOT NULL,
  HanTraHang DATE NOT NULL,
  NgayXuatPhieuTraHang DATE NOT NULL,
  NgayTaoDH DATE NOT NULL,
  MaKH VARCHAR(10) NOT NULL,
  Quy INT NOT NULL,
  PhieuTraHang VARBINARY(MAX) NULL,
  PRIMARY KEY (MaDH),
  FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH)
);

CREATE TABLE Mau
(
  Trangthai NVARCHAR(20) NULL,
  Ketqua NVARCHAR(20) NULL,
  MaNV VARCHAR(10) NOT NULL,
  MaDH VARCHAR(10) NOT NULL,
  ViTriLayMau NVARCHAR(50) NOT NULL,
  FOREIGN KEY (MaNV) REFERENCES Nhanvien(MaNV),
  FOREIGN KEY (MaDH) REFERENCES DonHang(MaDH),
  PRIMARY KEY (MaNV, MaDH, ViTriLayMau)
);

CREATE TABLE LichSu
(
  MaNV VARCHAR(10) NOT NULL,
  ThoiGianChinhSua DATETIME NOT NULL,
  MaDH VARCHAR(10) NOT NULL,
  NoiDungChinhSua NVARCHAR(200) NOT NULL,
  FOREIGN KEY (MaNV) REFERENCES Nhanvien(MaNV),
  FOREIGN KEY (MaDH) REFERENCES DonHang(MaDH)
);

CREATE TABLE KhiThai
(
  CO FLOAT  NULL,
  NhietDo FLOAT  NULL,
  NO2 FLOAT  NULL,
  O2 FLOAT  NULL,
  Hg FLOAT  NULL,
  PM FLOAT  NULL,
  NH3 FLOAT  NULL,
  N_O FLOAT  NULL,
  ApSuat FLOAT  NULL,
  SO2 FLOAT  NULL,
  H2S FLOAT  NULL,
  MaNV VARCHAR(10) NOT NULL,
  MaDH VARCHAR(10) NOT NULL,
  ViTriLayMau NVARCHAR(50) NOT NULL,
  PRIMARY KEY (MaNV, MaDH, ViTriLayMau),
  FOREIGN KEY (MaNV, MaDH, ViTriLayMau) REFERENCES Mau(MaNV, MaDH, ViTriLayMau)
);

CREATE TABLE KhongKhi
(
  CO FLOAT  NULL,
  SO2 FLOAT  NULL,
  O3 FLOAT  NULL,
  PM10 FLOAT  NULL,
  PM2dot5 FLOAT  NULL,
  NhietDo FLOAT  NULL,
  NO2 FLOAT  NULL,
  MaNV VARCHAR(10) NOT NULL,
  MaDH VARCHAR(10) NOT NULL,
  ViTriLayMau NVARCHAR(50) NOT NULL,
  PRIMARY KEY (MaNV, MaDH, ViTriLayMau),
  FOREIGN KEY (MaNV, MaDH, ViTriLayMau) REFERENCES Mau(MaNV, MaDH, ViTriLayMau)
);

CREATE TABLE NuocMat
(
  TDS FLOAT  NULL,
  NhietDo FLOAT  NULL,
  pH FLOAT  NULL,
  PO4 FLOAT  NULL,
  NH4 FLOAT  NULL,
  tongP FLOAT  NULL,
  tongN FLOAT  NULL,
  TOC FLOAT  NULL,
  TSS FLOAT  NULL,
  COD FLOAT  NULL,
  DO FLOAT  NULL,
  NO3 FLOAT  NULL,
  MaNV VARCHAR(10) NOT NULL,
  MaDH VARCHAR(10) NOT NULL,
  ViTriLayMau NVARCHAR(50) NOT NULL,
  PRIMARY KEY (MaNV, MaDH, ViTriLayMau),
  FOREIGN KEY (MaNV, MaDH, ViTriLayMau) REFERENCES Mau(MaNV, MaDH, ViTriLayMau)
);

CREATE TABLE ChiTieuNuocMat
(
  TDS FLOAT NOT NULL,
  NhietDo FLOAT NOT NULL,
  pH FLOAT NOT NULL,
  PO4 FLOAT NOT NULL,
  NH4 FLOAT NOT NULL,
  tongP FLOAT NOT NULL,
  tongN FLOAT NOT NULL,
  TOC FLOAT NOT NULL,
  TSS FLOAT NOT NULL,
  COD FLOAT NOT NULL,
  DO FLOAT NOT NULL,
  NO3 FLOAT NOT NULL,
  MaNV VARCHAR(10) NOT NULL,
  MaDH VARCHAR(10) NOT NULL,
  ViTriLayMau NVARCHAR(50) NOT NULL,
  PRIMARY KEY (MaNV, MaDH, ViTriLayMau),
  FOREIGN KEY (MaNV, MaDH, ViTriLayMau) REFERENCES NuocMat(MaNV, MaDH, ViTriLayMau)
);

CREATE TABLE ChiTieuKK
(
  CO FLOAT NOT NULL,
  SO2 FLOAT NOT NULL,
  O3 FLOAT NOT NULL,
  PM10 FLOAT NOT NULL,
  PM2dot5 FLOAT NOT NULL,
  NhietDo FLOAT NOT NULL,
  NO2 FLOAT NOT NULL,
  MaNV VARCHAR(10) NOT NULL,
  MaDH VARCHAR(10) NOT NULL,
  ViTriLayMau NVARCHAR(50) NOT NULL,
  PRIMARY KEY (MaNV, MaDH, ViTriLayMau),
  FOREIGN KEY (MaNV, MaDH, ViTriLayMau) REFERENCES KhongKhi(MaNV, MaDH, ViTriLayMau)
);

CREATE TABLE ChiTieuKhiThai
(
  CO FLOAT NOT NULL,
  NhietDo FLOAT NOT NULL,
  NO2 FLOAT NOT NULL,
  O2 FLOAT NOT NULL,
  Hg FLOAT NOT NULL,
  PM FLOAT NOT NULL,
  NH3 FLOAT NOT NULL,
  N_O FLOAT NOT NULL,
  ApSuat FLOAT NOT NULL,
  SO2 FLOAT NOT NULL,
  H2S FLOAT NOT NULL,
  MaNV VARCHAR(10) NOT NULL,
  MaDH VARCHAR(10) NOT NULL,
  ViTriLayMau NVARCHAR(50) NOT NULL,
  PRIMARY KEY (MaNV, MaDH, ViTriLayMau),
  FOREIGN KEY (MaNV, MaDH, ViTriLayMau) REFERENCES KhiThai(MaNV, MaDH, ViTriLayMau)
);

INSERT INTO KhachHang (MaKH, TenCongty, Email, DiaChi, SDT, NguoiDaiDien, GhiChu, NganhCongNghiep)
VALUES 
('DLA', N'Công ty TNHH Điện Lực A', 'dienluc.a@gmail.com', N'123 Đường Trần Phú, Phường 7, Quận Ba Đình, TP. Hà Nội', '0912345678', N'Nguyễn Văn A', N'Khách quen', N'Điện lực'),
('NSB', N'Công ty Cổ phần Nước Sạch B', 'nuocsach.b@gmail.com', N'456 Đường Nguyễn Trãi, Phường An Biên, Quận Lê Chân, TP. Hải Phòng', '0923456789', N'Trần Thị B', N'Khách mới', N'Cấp nước'),
('TPC', N'Công ty TNHH Thực Phẩm C', 'thucpham.c@gmail.com', N'789 Đường Điện Biên Phủ, Phường Chính Gián, Quận Thanh Khê, TP. Đà Nẵng', '0934567890', N'Lê Văn C', N'', N'Thực phẩm'),
('HCD', N'Công ty CP Hóa Chất D', 'hoachat.d@gmail.com', N'23 Đường Nguyễn Văn Linh, Phường An Bình, Quận Ninh Kiều, TP. Cần Thơ', '0945678901', N'Phạm Văn D', N'Đại lý', N'Hóa chất'),
('MME', N'Công ty TNHH May Mặc E', 'maymac.e@gmail.com', N'55 Đường Phù Đổng, Phường 2, TP. Đà Lạt, Tỉnh Lâm Đồng', '0956789012', N'Nguyễn Thị E', N'Mới ký kết', N'May mặc'),
('NSF', N'Công ty Cổ phần Nông Sản F', 'nongsan.f@gmail.com', N'789 Đường Lê Hồng Phong, Phường Phước Hải, TP. Nha Trang, Tỉnh Khánh Hòa', '0967890123', N'Trần Văn F', N'Khách quen', N'Nông sản'),
('CNG', N'Công ty TNHH Công Nghệ G', 'congnghe.g@gmail.com', N'123 Đường Võ Văn Ngân, Phường Linh Chiểu, TP. Thủ Đức, TP. Hồ Chí Minh', '0978901234', N'Lê Thị G', N'', N'Công nghệ'),
('XDH', N'Công ty CP Xây Dựng H', 'xaydung.h@gmail.com', N'456 Đường Ba Cu, Phường 3, TP. Vũng Tàu, Tỉnh Bà Rịa - Vũng Tàu', '0989012345', N'Hoàng Văn H', N'Đại lý', N'Xây dựng'),
('TPI', N'Công ty TNHH Thực Phẩm I', 'thucpham.i@gmail.com', N'789 Đường Hùng Vương, Phường Phú Nhuận, TP. Huế, Tỉnh Thừa Thiên Huế', '0990123456', N'Phạm Thị I', N'', N'Thực phẩm'),
('DLJ', N'Công ty TNHH Du Lịch J', 'dulich.j@gmail.com', N'67 Đường Nguyễn Huệ, Phường Hải Cảng, TP. Quy Nhơn, Tỉnh Bình Định', '0901234567', N'Nguyễn Văn K', N'Khách mới', N'Du lịch');

INSERT INTO Nhanvien (Hoten, Ngaysinh, MaNV, Email, MatKhau)
VALUES 
(N'Nguyễn Văn A', '1990-01-01', 'NV001', 'a.nguyen@gmail.com', 'pass123'),
(N'Trần Thị B', '1985-05-15', 'NV002', 'b.tran@gmail.com', 'pass123'),
(N'Lê Văn C', '1992-08-22', 'NV003', 'c.le@gmail.com', 'pass123'),
(N'Phạm Văn D', '1988-12-12', 'NV004', 'd.pham@gmail.com', 'pass123'),
(N'Nguyễn Thị E', '1993-03-23', 'NV005', 'e.nguyen@gmail.com', 'pass123'),
(N'Trần Văn F', '1991-07-17', 'NV006', 'f.tran@gmail.com', 'pass123'),
(N'Lê Thị G', '1987-09-09', 'NV007', 'g.le@gmail.com', 'pass123'),
(N'Hoàng Văn H', '1995-11-25', 'NV008', 'h.hoang@gmail.com', 'pass123'),
(N'Phạm Thị I', '1983-04-14', 'NV009', 'i.pham@gmail.com', 'pass123'),
(N'Nguyễn Văn K', '1990-06-10', 'NV010', 'k.nguyen@gmail.com', 'pass123');

INSERT INTO DonHang (MaDH, HanTraHang, NgayXuatPhieuTraHang, NgayTaoDH, MaKH, Quy, PhieuTraHang)
VALUES
('24.001', '2024-08-15', '2024-08-10', '2024-08-01', 'XDH', 3, NULL),
('24.002', '2024-09-20', '2024-09-15', '2024-09-05', 'TPI', 3, NULL),
('24.003', '2024-10-15', '2024-10-10', '2024-10-01', 'CNG', 4, NULL),
('24.004', '2024-10-25', '2024-10-20', '2024-10-10', 'DLJ', 4, NULL),
('24.005', '2024-11-05', '2024-11-01', '2024-10-25', 'DLA', 4, NULL),
('24.006', '2024-11-15', '2024-11-10', '2024-11-05', 'HCD', 4, NULL),
('24.007', '2024-11-25', '2024-11-20', '2024-11-10', 'MME', 4, NULL),
('24.008', '2024-12-05', '2024-12-01', '2024-11-25', 'NSB', 4, NULL),
('24.009', '2024-12-15', '2024-12-10', '2024-12-05', 'NSF', 4, NULL),
('24.010', '2024-12-25', '2024-12-20', '2024-12-10', 'TPC', 4, NULL);
		
INSERT INTO Mau (Trangthai, Ketqua, MaNV, MaDH, ViTriLayMau)
VALUES
-- Samples for Order 24.001 (HanTraHang: 2024-08-15, Past Date - Completed)
(N'Đã hoàn thành', N'Đạt yêu cầu', 'NV001', '24.001', N'Hồ chứa nước thải đã qua xử lý'),
(N'Đã hoàn thành', N'Không đạt yêu cầu', 'NV002', '24.001', N'Khí thải tại nhà máy'),

-- Samples for Order 24.002 (HanTraHang: 2024-09-20, Past Date - Completed)
(N'Đã hoàn thành', N'Đạt yêu cầu', 'NV003', '24.002', N'Bể lắng xử lý thứ cấp'),
(N'Đã hoàn thành', N'Không đạt yêu cầu', 'NV004', '24.002', N'Không khí khu vực xử lý'),

-- Samples for Order 24.003 (HanTraHang: 2024-10-15, Past Date - Completed)
(N'Đã hoàn thành', N'Đạt yêu cầu', 'NV001', '24.003', N'Khí thải gần khu công nghiệp'),
(N'Đã hoàn thành', N'Không đạt yêu cầu', 'NV002', '24.003', N'Không khí gần khu xử lý nước'),

-- Samples for Order 24.004 (HanTraHang: 2024-10-25, Past Date - Completed)
(N'Đã hoàn thành', N'Không đạt yêu cầu', 'NV003', '24.004', N'Sông thoát nước chính'),
(N'Đã hoàn thành', N'Đạt yêu cầu', 'NV004', '24.004', N'Khí thải tại xưởng sản xuất'),

-- Samples for Order 24.005 (HanTraHang: 2024-11-05, Current Date - In Progress)
(N'Đang xử lý mẫu', N'Chưa có kết quả', 'NV005', '24.005', N'Hồ nước ngọt gần khu dân cư'),
(N'Đang lấy mẫu', N'Chưa có kết quả', 'NV001', '24.005', N'Khí thải khu dân cư'),

-- Samples for Order 24.006 (HanTraHang: 2024-11-15, Future Date - In Progress)
(N'Đang xử lý mẫu', N'Chưa có kết quả', 'NV002', '24.006', N'Khu vực xử lý nước thải sinh hoạt'),
(N'Đang lấy mẫu', N'Chưa có kết quả', 'NV003', '24.006', N'Không khí gần khu công nghiệp'),

-- Samples for Order 24.007 (HanTraHang: 2024-11-25, Future Date - In Progress)
(N'Đang lấy mẫu', N'Chưa có kết quả', 'NV004', '24.007', N'Hệ thống lọc bùn công nghiệp'),
(N'Đang lấy mẫu', N'Chưa có kết quả', 'NV005', '24.007', N'Khí thải tại hệ thống xử lý'),

-- Samples for Order 24.008 (HanTraHang: 2024-12-05, Future Date - In Progress)
(N'Đang lấy mẫu', N'Chưa có kết quả', 'NV001', '24.008', N'Bể chứa nước chưa xử lý'),
(N'Đang lấy mẫu', N'Chưa có kết quả', 'NV002', '24.008', N'Khí thải khu vực lưu trữ'),

-- Samples for Order 24.009 (HanTraHang: 2024-12-15, Future Date - In Progress)
(N'Đang lấy mẫu', N'Chưa có kết quả', 'NV003', '24.009', N'Hồ chứa nước dự phòng'),
(N'Đang lấy mẫu', N'Chưa có kết quả', 'NV004', '24.009', N'Không khí gần hồ dự phòng'),

-- Samples for Order 24.010 (HanTraHang: 2024-12-25, Future Date - In Progress)
(N'Đang lấy mẫu', N'Chưa có kết quả', 'NV005', '24.010', N'Kênh dẫn nước mưa gần khu chế xuất'),
(N'Đang lấy mẫu', N'Chưa có kết quả', 'NV001', '24.010', N'Không khí xung quanh khu chế xuất');


--INSERT INTO LichSu (MaNV, ThoiGianChinhSua, MaDH, NoiDungChinhSua)
--VALUES
--('NV001', '2024-01-05 10:30:00', '24.001', N'Cập nhật thông tin khách hàng từ "Nguyễn Văn A" thành "Trần Thị B"'),
--('NV002', '2024-01-06 11:15:00', '24.002', N'Thay đổi trạng thái đơn hàng từ "Đang xử lý mẫu" sang "Đã hoàn thành"'),
--('NV003', '2024-01-07 09:45:00', '24.003', N'Thêm ghi chú kiểm định: "Cần kiểm tra lại chất lượng sản phẩm"'),
--('NV001', '2024-01-08 14:00:00', '24.004', N'Cập nhật thời gian trả hàng từ "2024-01-20" sang "2024-01-25"'),
--('NV002', '2024-01-09 16:20:00', '24.005', N'Thay đổi địa chỉ giao hàng từ "456 Đường Nguyễn Trãi, TP. Hải Phòng" sang "789 Đường Điện Biên Phủ, TP. Đà Nẵng"');

INSERT INTO KhiThai (CO, NhietDo, NO2, O2, Hg, PM, NH3, N_O, ApSuat, SO2, H2S, MaNV, MaDH, ViTriLayMau)
VALUES
-- Dữ liệu cho mẫu khí thải tại nhà máy (Order 24.001)
(0.02, 25.5, 0.005, 20.9, 0.0002, 150, 0.01, 0.003, 1013.25, 0.01, 0.0005, 'NV002', '24.001', N'Khí thải tại nhà máy'),

-- Dữ liệu cho mẫu khí thải gần khu công nghiệp (Order 24.003)
(0.04, 26.0, 0.007, 20.8, 0.0003, 200, 0.015, 0.004, 1012.00, 0.02, 0.0006, 'NV001', '24.003', N'Khí thải gần khu công nghiệp'),

-- Dữ liệu cho mẫu khí thải tại xưởng sản xuất (Order 24.004)
(0.03, 24.0, 0.006, 20.7, 0.0004, 180, 0.012, 0.005, 1011.50, 0.015, 0.0004, 'NV004', '24.004', N'Khí thải tại xưởng sản xuất'),

-- Dữ liệu cho mẫu khí thải khu dân cư (Order 24.005)
(0.01, 27.0, 0.002, 20.9, 0.0001, 100, 0.008, 0.002, 1013.00, 0.005, 0.0002, 'NV001', '24.005', N'Khí thải khu dân cư'),

-- Dữ liệu cho mẫu khí thải tại hệ thống xử lý (Order 24.007)
(0.05, 23.0, 0.009, 20.5, 0.0003, 220, 0.016, 0.006, 1010.00, 0.03, 0.0007, 'NV005', '24.007', N'Khí thải tại hệ thống xử lý'),

-- Dữ liệu cho mẫu khí thải khu vực lưu trữ (Order 24.008)
(0.02, 28.0, 0.004, 21.0, 0.0002, 130, 0.01, 0.003, 1013.50, 0.012, 0.0003, 'NV002', '24.008', N'Khí thải khu vực lưu trữ');

INSERT INTO ChiTieuKhiThai (CO, NhietDo, NO2, O2, Hg, PM, NH3, N_O, ApSuat, SO2, H2S, MaNV, MaDH, ViTriLayMau)
VALUES
-- Dữ liệu cho mẫu khí thải tại nhà máy (Order 24.001)
(0.018, 24.5, 0.006, 21.0, 0.00025, 140, 0.009, 0.0025, 1012.0, 0.015, 0.0004, 'NV002', '24.001', N'Khí thải tại nhà máy'),

-- Dữ liệu cho mẫu khí thải gần khu công nghiệp (Order 24.003)
(0.045, 27.0, 0.008, 20.6, 0.00035, 190, 0.013, 0.0045, 1010.5, 0.025, 0.00065, 'NV001', '24.003', N'Khí thải gần khu công nghiệp'),

-- Dữ liệu cho mẫu khí thải tại xưởng sản xuất (Order 24.004)
(0.032, 25.0, 0.007, 20.8, 0.0005, 170, 0.011, 0.0052, 1011.0, 0.018, 0.0005, 'NV004', '24.004', N'Khí thải tại xưởng sản xuất'),

-- Dữ liệu cho mẫu khí thải khu dân cư (Order 24.005)
(0.011, 29.0, 0.003, 21.1, 0.00015, 110, 0.007, 0.0015, 1013.2, 0.007, 0.00025, 'NV001', '24.005', N'Khí thải khu dân cư'),

-- Dữ liệu cho mẫu khí thải tại hệ thống xử lý (Order 24.007)
(0.053, 23.5, 0.010, 20.4, 0.0004, 220, 0.017, 0.007, 1009.8, 0.035, 0.00075, 'NV005', '24.007', N'Khí thải tại hệ thống xử lý'),

-- Dữ liệu cho mẫu khí thải khu vực lưu trữ (Order 24.008)
(0.019, 30.0, 0.005, 21.3, 0.0002, 125, 0.008, 0.0035, 1014.0, 0.010, 0.00035, 'NV002', '24.008', N'Khí thải khu vực lưu trữ');

INSERT INTO KhongKhi (CO, SO2, O3, PM10, PM2dot5, NhietDo, NO2, MaNV, MaDH, ViTriLayMau)
VALUES
-- Dữ liệu cho mẫu không khí khu vực xử lý (Order 24.002)
(0.0045, 0.02, 0.035, 30, 12, 28.0, 0.005, 'NV004', '24.002', N'Không khí khu vực xử lý'),

-- Dữ liệu cho mẫu không khí gần khu xử lý nước (Order 24.003)
(0.0065, 0.015, 0.028, 25, 10, 27.0, 0.007, 'NV002', '24.003', N'Không khí gần khu xử lý nước'),

-- Dữ liệu cho mẫu không khí gần khu công nghiệp (Order 24.006)
(0.0050, 0.018, 0.030, 35, 15, 29.0, 0.006, 'NV003', '24.006', N'Không khí gần khu công nghiệp'),

-- Dữ liệu cho mẫu không khí gần hồ dự phòng (Order 24.009)
(0.0030, 0.010, 0.025, 20, 8, 30.0, 0.004, 'NV004', '24.009', N'Không khí gần hồ dự phòng'),

-- Dữ liệu cho mẫu không khí xung quanh khu chế xuất (Order 24.010)
(0.0070, 0.022, 0.032, 28, 11, 31.0, 0.009, 'NV001', '24.010', N'Không khí xung quanh khu chế xuất');

INSERT INTO ChiTieuKK (CO, SO2, O3, PM10, PM2dot5, NhietDo, NO2, MaNV, MaDH, ViTriLayMau)
VALUES
-- Dữ liệu cho mẫu không khí khu vực xử lý (Order 24.002)
(0.0050, 0.018, 0.033, 32, 13, 27.5, 0.0065, 'NV004', '24.002', N'Không khí khu vực xử lý'),

-- Dữ liệu cho mẫu không khí gần khu xử lý nước (Order 24.003)
(0.0070, 0.017, 0.030, 27, 11, 29.0, 0.0050, 'NV002', '24.003', N'Không khí gần khu xử lý nước'),

-- Dữ liệu cho mẫu không khí gần khu công nghiệp (Order 24.006)
(0.0048, 0.021, 0.025, 34, 14, 28.5, 0.0075, 'NV003', '24.006', N'Không khí gần khu công nghiệp'),

-- Dữ liệu cho mẫu không khí gần hồ dự phòng (Order 24.009)
(0.0025, 0.009, 0.020, 22, 9, 31.0, 0.0035, 'NV004', '24.009', N'Không khí gần hồ dự phòng'),

-- Dữ liệu cho mẫu không khí xung quanh khu chế xuất (Order 24.010)
(0.0080, 0.020, 0.028, 29, 12, 30.0, 0.0080, 'NV001', '24.010', N'Không khí xung quanh khu chế xuất');


INSERT INTO NuocMat (TDS, NhietDo, pH, PO4, NH4, tongP, tongN, TOC, TSS, COD, DO, NO3, MaNV, MaDH, ViTriLayMau)
VALUES
-- Dữ liệu cho mẫu hồ chứa nước thải đã qua xử lý (Order 24.001)
(250, 25.5, 7.5, 0.5, 0.1, 1.0, 2.5, 3.2, 15, 40, 6.0, 0.3, 'NV001', '24.001', N'Hồ chứa nước thải đã qua xử lý'),

-- Dữ liệu cho mẫu bể lắng xử lý thứ cấp (Order 24.002)
(180, 24.0, 7.2, 0.4, 0.2, 0.8, 2.0, 2.5, 12, 30, 7.5, 0.1, 'NV003', '24.002', N'Bể lắng xử lý thứ cấp'),

-- Dữ liệu cho mẫu sông thoát nước chính (Order 24.004)
(350, 22.0, 6.8, 0.6, 0.4, 1.5, 3.0, 4.0, 25, 55, 5.5, 0.6, 'NV003', '24.004', N'Sông thoát nước chính'),

-- Dữ liệu cho mẫu hồ nước ngọt gần khu dân cư (Order 24.005)
(150, 26.0, 7.8, 0.2, 0.05, 0.3, 1.0, 1.0, 10, 20, 8.0, 0.2, 'NV005', '24.005', N'Hồ nước ngọt gần khu dân cư'),

-- Dữ liệu cho mẫu khu vực xử lý nước thải sinh hoạt (Order 24.006)
(280, 27.0, 7.0, 0.7, 0.3, 1.2, 2.8, 3.0, 20, 45, 5.0, 0.4, 'NV002', '24.006', N'Khu vực xử lý nước thải sinh hoạt'),

-- Dữ liệu cho mẫu hệ thống lọc bùn công nghiệp (Order 24.007)
(300, 28.5, 7.6, 0.5, 0.1, 1.0, 2.3, 2.8, 18, 42, 6.5, 0.3, 'NV004', '24.007', N'Hệ thống lọc bùn công nghiệp'),

-- Dữ liệu cho mẫu bể chứa nước chưa xử lý (Order 24.008)
(400, 23.5, 6.5, 0.9, 0.5, 2.0, 3.5, 4.5, 30, 60, 4.5, 0.8, 'NV001', '24.008', N'Bể chứa nước chưa xử lý'),

-- Dữ liệu cho mẫu hồ chứa nước dự phòng (Order 24.009)
(210, 25.0, 7.3, 0.3, 0.2, 0.6, 1.8, 2.2, 14, 36, 7.0, 0.2, 'NV003', '24.009', N'Hồ chứa nước dự phòng'),

-- Dữ liệu cho mẫu kênh dẫn nước mưa gần khu chế xuất (Order 24.010)
(180, 24.5, 7.4, 0.4, 0.1, 0.9, 2.1, 2.0, 15, 33, 6.8, 0.3, 'NV005', '24.010', N'Kênh dẫn nước mưa gần khu chế xuất');

INSERT INTO ChiTieuNuocMat (TDS, NhietDo, pH, PO4, NH4, tongP, tongN, TOC, TSS, COD, DO, NO3, MaNV, MaDH, ViTriLayMau)
VALUES
-- Dữ liệu cho mẫu hồ chứa nước thải đã qua xử lý (Order 24.001)
(260, 26.0, 7.6, 0.6, 0.15, 1.1, 2.6, 3.5, 16, 42, 6.2, 0.35, 'NV001', '24.001', N'Hồ chứa nước thải đã qua xử lý'),

-- Dữ liệu cho mẫu bể lắng xử lý thứ cấp (Order 24.002)
(190, 24.5, 7.3, 0.45, 0.25, 0.85, 2.1, 2.7, 13, 32, 7.3, 0.12, 'NV003', '24.002', N'Bể lắng xử lý thứ cấp'),

-- Dữ liệu cho mẫu sông thoát nước chính (Order 24.004)
(360, 22.5, 6.9, 0.7, 0.45, 1.6, 3.1, 4.3, 26, 58, 5.3, 0.65, 'NV003', '24.004', N'Sông thoát nước chính'),

-- Dữ liệu cho mẫu hồ nước ngọt gần khu dân cư (Order 24.005)
(160, 25.8, 7.9, 0.25, 0.06, 0.35, 1.2, 1.2, 9, 21, 8.2, 0.15, 'NV005', '24.005', N'Hồ nước ngọt gần khu dân cư'),

-- Dữ liệu cho mẫu khu vực xử lý nước thải sinh hoạt (Order 24.006)
(290, 27.2, 7.1, 0.75, 0.35, 1.3, 2.9, 3.2, 21, 47, 4.9, 0.45, 'NV002', '24.006', N'Khu vực xử lý nước thải sinh hoạt'),

-- Dữ liệu cho mẫu hệ thống lọc bùn công nghiệp (Order 24.007)
(310, 29.0, 7.7, 0.55, 0.15, 1.1, 2.4, 2.9, 17, 43, 6.3, 0.35, 'NV004', '24.007', N'Hệ thống lọc bùn công nghiệp'),

-- Dữ liệu cho mẫu bể chứa nước chưa xử lý (Order 24.008)
(410, 24.0, 6.6, 1.0, 0.55, 2.2, 3.6, 4.8, 32, 62, 4.4, 0.85, 'NV001', '24.008', N'Bể chứa nước chưa xử lý'),

-- Dữ liệu cho mẫu hồ chứa nước dự phòng (Order 24.009)
(220, 25.3, 7.4, 0.35, 0.25, 0.7, 1.9, 2.3, 13, 37, 6.9, 0.25, 'NV003', '24.009', N'Hồ chứa nước dự phòng'),

-- Dữ liệu cho mẫu kênh dẫn nước mưa gần khu chế xuất (Order 24.010)
(190, 24.7, 7.5, 0.45, 0.12, 0.95, 2.2, 2.1, 14, 35, 6.7, 0.32, 'NV005', '24.010', N'Kênh dẫn nước mưa gần khu chế xuất');


--PROCEDURE
--TẠO MÃ NGẪU NHIÊN
CREATE PROCEDURE TaoMaKhachHang
    @TenCongTy NVARCHAR(255),
    @MaKH VARCHAR(10) OUTPUT
AS
BEGIN
    DECLARE @MaTam NVARCHAR(50) = ''
    DECLARE @ViTri INT = 1
    DECLARE @Tu NVARCHAR(50)
    
    WHILE @ViTri > 0
    BEGIN
        SET @ViTri = CHARINDEX(' ', @TenCongTy)
        
        IF @ViTri > 0
        BEGIN
            SET @Tu = LEFT(@TenCongTy, @ViTri - 1)
            SET @TenCongTy = LTRIM(SUBSTRING(@TenCongTy, @ViTri + 1, LEN(@TenCongTy)))
        END
        ELSE
        BEGIN
            SET @Tu = @TenCongTy
        END
        
        SET @MaTam = @MaTam + LEFT(@Tu, 1)
    END
    
    SET @MaKH = UPPER(@MaTam)
END;

--TẠO MÃ ĐƠN HÀNG TĂNG DẦN THEO SỐ NĂM
CREATE PROCEDURE TaoMaDH
    @NewCode NVARCHAR(10) OUTPUT
AS
BEGIN
    DECLARE @CurrentYear NVARCHAR(2)
    DECLARE @LastCode NVARCHAR(10)
    DECLARE @NextNumber INT

    SET @CurrentYear = RIGHT(YEAR(GETDATE()), 2)

    SELECT @LastCode = MAX(MaDH)
    FROM DonHang
    WHERE LEFT(MaDH, 2) = @CurrentYear

    IF @LastCode IS NULL
    BEGIN
        SET @NextNumber = 1
    END
    ELSE
    BEGIN
        SET @NextNumber = CONVERT(INT, RIGHT(@LastCode, 3)) + 1
    END

    SET @NewCode = @CurrentYear + '.' + RIGHT('000' + CONVERT(VARCHAR, @NextNumber), 3)
END


--TRIGGER
--THÊM MÃ KHÁCH HÀNG NGẪU NHIÊN VÀ KIỂM TRA XEM ĐÃ CÓ MÃ ĐÓ CHƯA
CREATE TRIGGER trg_BeforeInsertKhachHang
ON KhachHang
INSTEAD OF INSERT
AS
BEGIN
    DECLARE @MaKH NVARCHAR(50)
    DECLARE @NguoiDaiDien NVARCHAR(50), @TenCongTy NVARCHAR(100), @Email VARCHAR(50)
    DECLARE @DiaChi NVARCHAR(100), @NganhCongNghiep NVARCHAR(50), @SDT CHAR(10)
    DECLARE @GhiChu NVARCHAR(100)
    DECLARE @IsDuplicate BIT

    DECLARE cur CURSOR FOR 
    SELECT NguoiDaiDien, TenCongTy, Email, DiaChi, NganhCongNghiep, SDT, GhiChu 
    FROM Inserted

    OPEN cur
    FETCH NEXT FROM cur INTO @NguoiDaiDien, @TenCongTy, @Email, @DiaChi, @NganhCongNghiep, @SDT, @GhiChu

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @IsDuplicate = 1
        WHILE @IsDuplicate = 1
        BEGIN
            -- Gọi procedure TaoMaKhachHang để tạo mã khách hàng từ tên công ty
            EXEC TaoMaKhachHang @TenCongTy, @MaKH OUTPUT

            -- Kiểm tra mã khách hàng đã tồn tại trong bảng KhachHang hay chưa
            IF NOT EXISTS (SELECT 1 FROM KhachHang WHERE MaKH = @MaKH)
            BEGIN
                SET @IsDuplicate = 0
            END
            ELSE
            BEGIN
                -- Nếu mã bị trùng, thêm một số ngẫu nhiên vào cuối mã để tạo mã duy nhất
                SET @MaKH = @MaKH + CAST((ABS(CHECKSUM(NEWID())) % 100) AS NVARCHAR(2))
            END
        END

        INSERT INTO KhachHang (MaKH, NguoiDaiDien, TenCongTy, Email, DiaChi, NganhCongNghiep, SDT, GhiChu)
        VALUES (@MaKH, @NguoiDaiDien, @TenCongTy, @Email, @DiaChi, @NganhCongNghiep, @SDT, @GhiChu)

        FETCH NEXT FROM cur INTO @NguoiDaiDien, @TenCongTy, @Email, @DiaChi, @NganhCongNghiep, @SDT, @GhiChu
    END

    CLOSE cur
    DEALLOCATE cur
END


--THÊM ĐƠN HÀNG MỚI VÀO BẢNG DONHANG
CREATE TRIGGER trg_BeforeInsert_DonHang
ON DonHang
INSTEAD OF INSERT
AS
BEGIN
    DECLARE @NewAutoCode NVARCHAR(10)
    DECLARE @HanTraHang DATE
    DECLARE @NgayXuatPhieuTraHang DATE
    DECLARE @NgayTaoDH DATE
    DECLARE @MaKH NVARCHAR(10)
    DECLARE @Quy INT
    DECLARE @PhieuTraHang VARBINARY(MAX)

    -- Tạo mã đơn hàng tự động bằng procedure TaoMaDH
    EXEC TaoMaDH @NewAutoCode OUTPUT

    -- Lấy dữ liệu từ bảng Inserted
    SELECT 
        @HanTraHang = HanTraHang,
        @NgayXuatPhieuTraHang = NgayXuatPhieuTraHang,
        @NgayTaoDH = NgayTaoDH,
        @MaKH = MaKH,
        @Quy = Quy,
        @PhieuTraHang = PhieuTraHang
    FROM INSERTED

    -- Chèn dữ liệu vào bảng DonHang
    INSERT INTO DonHang (MaDH, HanTraHang, NgayXuatPhieuTraHang, NgayTaoDH, MaKH, Quy, PhieuTraHang)
    VALUES (@NewAutoCode, @HanTraHang, @NgayXuatPhieuTraHang, @NgayTaoDH, @MaKH, @Quy, @PhieuTraHang)
END


-- Insert data into the Mau table
INSERT INTO Mau (Trangthai, Ketqua, MaNV, MaDH, ViTriLayMau)
VALUES
    (N'Đang xử lý', N'Đạt yêu cầu', 'NV003', '24.003', N'Khu vực xử lý nước thải'),
    (N'Đã hoàn thành', N'Không đạt yêu cầu', 'NV004', '24.004', N'Khu vực nuôi trồng thủy sản'),
    (N'Đang xử lý', N'Đạt yêu cầu', 'NV005', '24.005', N'Khu vực rừng trồng công nghiệp'),
    (N'Đã hoàn thành', N'Đạt yêu cầu', 'NV006', '24.006', N'Khu vực đổ thải rắn'),
    (N'Đang xử lý', N'Không đạt yêu cầu', 'NV007', '24.007', N'Khu vực thu gom chất thải rắn'),
    (N'Đã hoàn thành', N'Đạt yêu cầu', 'NV008', '24.008', N'Khu vực khai thác khoáng sản'),
    (N'Đang xử lý', N'Đạt yêu cầu', 'NV009', '24.009', N'Khu vực đồi núi đất đá'),
    (N'Đã hoàn thành', N'Không đạt yêu cầu', 'NV010', '24.010', N'Khu vực trồng cây nông sản'),
    (N'Đang xử lý', N'Đạt yêu cầu', 'NV011', '24.011', N'Khu vực hồ nước tự nhiên'),
    (N'Đã hoàn thành', N'Đạt yêu cầu', 'NV012', '24.012', N'Khu vực kiểm tra chất lượng nước'),
    (N'Đang xử lý', N'Không đạt yêu cầu', 'NV013', '24.013', N'Khu vực chăn nuôi gia súc'),
    (N'Đã hoàn thành', N'Đạt yêu cầu', 'NV014', '24.014', N'Khu vực xử lý chất thải sinh hoạt'),
    (N'Đang xử lý', N'Đạt yêu cầu', 'NV015', '24.015', N'Khu vực xử lý chất thải công nghiệp'),
    (N'Đã hoàn thành', N'Không đạt yêu cầu', 'NV016', '24.016', N'Khu vực đất đai chưa qua sử dụng'),
    (N'Đang xử lý', N'Đạt yêu cầu', 'NV017', '24.017', N'Khu vực sông, suối tự nhiên'),
    (N'Đã hoàn thành', N'Đạt yêu cầu', 'NV018', '24.018', N'Khu vực biển và bờ biển'),
    (N'Đang xử lý', N'Không đạt yêu cầu', 'NV019', '24.019', N'Khu vực đất canh tác bỏ hoang'),
    (N'Đã hoàn thành', N'Đạt yêu cầu', 'NV020', '24.020', N'Khu vực đất có nguy cơ xói mòn'),
    (N'Đang xử lý', N'Đạt yêu cầu', 'NV021', '24.021', N'Khu vực quản lý chất thải xây dựng'),
    (N'Đã hoàn thành', N'Không đạt yêu cầu', 'NV022', '24.022', N'Khu vực mỏ đá khai thác');

-- Insert data into the KhongKhi table
INSERT INTO KhongKhi (CO, SO2, O3, PM10, PM2dot5, NhietDo, NO2, MaNV, MaDH, ViTriLayMau)
VALUES
    (0.0045, 0.020, 0.035, 30, 12, 28.0, 0.005, 'NV004', '24.002', N'Không khí khu vực xử lý'),
    (0.0065, 0.015, 0.028, 25, 10, 27.0, 0.007, 'NV002', '24.003', N'Không khí gần khu xử lý nước'),
    (0.0050, 0.018, 0.030, 35, 15, 29.0, 0.006, 'NV003', '24.006', N'Không khí gần khu công nghiệp'),
    (0.0030, 0.010, 0.025, 20, 8, 30.0, 0.004, 'NV004', '24.009', N'Không khí gần hồ dự phòng'),
    (0.0070, 0.022, 0.032, 28, 11, 31.0, 0.009, 'NV001', '24.010', N'Không khí xung quanh khu chế xuất'),
    (0.0052, 0.021, 0.037, 32, 14, 28.5, 0.008, 'NV005', '24.011', N'Không khí khu vực sản xuất'),
    (0.0048, 0.018, 0.033, 30, 13, 27.5, 0.006, 'NV006', '24.012', N'Không khí gần khu vực công viên'),
    (0.0035, 0.012, 0.029, 27, 9, 29.0, 0.005, 'NV007', '24.013', N'Không khí gần khu dân cư'),
    (0.0055, 0.020, 0.034, 33, 16, 28.8, 0.007, 'NV008', '24.014', N'Không khí khu vực bến tàu'),
    (0.0043, 0.016, 0.031, 29, 11, 28.2, 0.006, 'NV009', '24.015', N'Không khí khu công nghiệp nhỏ'),
    (0.0060, 0.019, 0.038, 34, 13, 29.3, 0.008, 'NV010', '24.016', N'Không khí khu vực ven biển'),
    (0.0041, 0.017, 0.026, 31, 12, 27.9, 0.005, 'NV011', '24.017', N'Không khí xung quanh bệnh viện'),
    (0.0056, 0.020, 0.039, 30, 14, 28.7, 0.007, 'NV012', '24.018', N'Không khí khu vực khu du lịch'),
    (0.0044, 0.014, 0.029, 28, 10, 28.3, 0.005, 'NV013', '24.019', N'Không khí gần khu trường học'),
    (0.0039, 0.015, 0.033, 26, 9, 29.2, 0.007, 'NV014', '24.020', N'Không khí gần khu vực chợ');
