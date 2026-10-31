-- National Library Management: empty, reproducible schema. No personal records.
-- Run against a NEW empty database using Database/SetupDatabase.ps1.
SET XACT_ABORT ON;
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
BEGIN TRANSACTION;

CREATE TABLE [dbo].[tblChiTietHSMuon] (
    [MaHSM] [int] NOT NULL,
    [MaSach] [int] NOT NULL,
    [ChuaTra] [bit] NOT NULL,
    [DonGiaThueLucMuon] [decimal](18,2) NULL,
    CONSTRAINT [PK_tblChiTietHSMuon] PRIMARY KEY ([MaHSM], [MaSach])
);

CREATE TABLE [dbo].[tblChiTietHSTra] (
    [MaHSTra] [int] NOT NULL,
    [MaSach] [int] NOT NULL,
    [MaViPham] [int] NULL,
    [ThanhTien] [decimal](18,2) NULL,
    [SoNgayThue] [int] NULL,
    [DonGiaThue] [decimal](18,2) NULL,
    [TienPhat] [decimal](18,2) NULL,
    CONSTRAINT [PK_tblChiTietHSTra] PRIMARY KEY ([MaHSTra], [MaSach])
);

CREATE TABLE [dbo].[tblDanToc] (
    [MaDanToc] [int] NOT NULL,
    [TenDanToc] [nvarchar](100) NOT NULL,
    CONSTRAINT [PK__tblDanTo__A5FA097086ECDB1E] PRIMARY KEY ([MaDanToc])
);

CREATE TABLE [dbo].[tblDMSach] (
    [MaSach] [int] NOT NULL,
    [TenSach] [nvarchar](200) NOT NULL,
    [TenTG] [nvarchar](100) NULL,
    [MaNXB] [int] NULL,
    [MaXB] [int] NULL,
    [KhoSach] [nvarchar](50) NULL,
    [GiaSach] [decimal](18,2) NULL,
    [LanTaiBan] [int] NULL,
    [MaTheLoai] [int] NULL,
    [MaNgonNgu] [int] NULL,
    [SoTrang] [int] NULL,
    [TomTatND] [nvarchar](max) NULL,
    [DonGiaThue] [decimal](18,2) NULL,
    [SoLuong] [int] NOT NULL CONSTRAINT [DF_DMSach_SoLuong] DEFAULT ((0)),
    CONSTRAINT [PK__tblDMSac__B235742D7D64465E] PRIMARY KEY ([MaSach])
);

CREATE TABLE [dbo].[tblHoSoMuon] (
    [MaHSM] [int] NOT NULL,
    [MaThe] [int] NOT NULL,
    [MaThuThu] [int] NOT NULL,
    [NgayMuon] [date] NOT NULL,
    [TamUng] [decimal](18,2) NULL,
    CONSTRAINT [PK__tblHoSoM__3C901127C251D991] PRIMARY KEY ([MaHSM])
);

CREATE TABLE [dbo].[tblHoSoTra] (
    [MaHSTra] [int] NOT NULL,
    [MaHSM] [int] NOT NULL,
    [NgayTra] [date] NULL,
    [TongTien] [decimal](18,2) NULL,
    [SoTienThanhToan] [decimal](18,2) NULL,
    [MaThuThu] [int] NULL,
    [TamUngApDung] [decimal](18,2) NOT NULL CONSTRAINT [DF_HSTra_TamUng] DEFAULT ((0)),
    CONSTRAINT [PK__tblHoSoT__B57CC1C5E0A6A71B] PRIMARY KEY ([MaHSTra])
);

CREATE TABLE [dbo].[tblNgonNgu] (
    [MaNgonNgu] [int] NOT NULL,
    [TenNgonNgu] [nvarchar](100) NOT NULL,
    CONSTRAINT [PK__tblNgonN__131924EE02B08699] PRIMARY KEY ([MaNgonNgu])
);

CREATE TABLE [dbo].[tblNXB] (
    [MaNXB] [int] NOT NULL,
    [TenNXB] [nvarchar](100) NOT NULL,
    [DiaChi] [nvarchar](200) NULL,
    [DienThoai] [varchar](20) NULL,
    [Email] [varchar](100) NULL,
    CONSTRAINT [PK__tblNXB__3A19482CA0DF98B8] PRIMARY KEY ([MaNXB])
);

CREATE TABLE [dbo].[tblQue] (
    [MaQue] [int] NOT NULL,
    [TenQue] [nvarchar](100) NOT NULL,
    CONSTRAINT [PK__tblQue__39A2C8EA03C86288] PRIMARY KEY ([MaQue])
);

CREATE TABLE [dbo].[tblTheLoai] (
    [MaTheLoai] [int] NOT NULL,
    [TenTheLoai] [nvarchar](100) NOT NULL,
    CONSTRAINT [PK__tblTheLo__D73FF34A186CC982] PRIMARY KEY ([MaTheLoai])
);

CREATE TABLE [dbo].[tblTheMuon] (
    [MaThe] [int] NOT NULL,
    [HoTen] [nvarchar](100) NOT NULL,
    [GioiTinh] [nvarchar](10) NULL,
    [NgaySinh] [date] NULL,
    [MaQue] [int] NULL,
    [DiaChiHienTai] [nvarchar](200) NULL,
    [MaDanToc] [int] NULL,
    [CMTND] [varchar](20) NULL,
    [NgayLamThe] [date] NULL,
    [NgayHetHan] [date] NULL,
    [SoLuongMuon] [int] NULL,
    CONSTRAINT [PK__tblTheMu__314EEAAF288112F6] PRIMARY KEY ([MaThe])
);

CREATE TABLE [dbo].[tblThuThu] (
    [MaThuThu] [int] NOT NULL,
    [TenThuThu] [nvarchar](100) NOT NULL,
    [DiaChi] [nvarchar](200) NULL,
    [DienThoaiCD] [varchar](20) NULL,
    [DienThoaiDD] [varchar](20) NULL,
    [MaQue] [int] NULL,
    CONSTRAINT [PK__tblThuTh__3A1552A421B050D5] PRIMARY KEY ([MaThuThu])
);

CREATE TABLE [dbo].[tblViPham] (
    [MaViPham] [int] NOT NULL,
    [TenViPham] [nvarchar](200) NOT NULL,
    [TienPhat] [decimal](18,2) NULL,
    CONSTRAINT [PK__tblViPha__F1921D89116F6548] PRIMARY KEY ([MaViPham])
);
ALTER TABLE [dbo].[tblChiTietHSMuon] WITH CHECK ADD CONSTRAINT [FK_ChiTietHSMuon_DMSach] FOREIGN KEY ([MaSach]) REFERENCES [dbo].[tblDMSach] ([MaSach]);
ALTER TABLE [dbo].[tblChiTietHSMuon] WITH CHECK ADD CONSTRAINT [FK_ChiTietHSMuon_HoSoMuon] FOREIGN KEY ([MaHSM]) REFERENCES [dbo].[tblHoSoMuon] ([MaHSM]);
ALTER TABLE [dbo].[tblChiTietHSTra] WITH CHECK ADD CONSTRAINT [FK_ChiTietHSTra_DMSach] FOREIGN KEY ([MaSach]) REFERENCES [dbo].[tblDMSach] ([MaSach]);
ALTER TABLE [dbo].[tblChiTietHSTra] WITH CHECK ADD CONSTRAINT [FK_ChiTietHSTra_HoSoTra] FOREIGN KEY ([MaHSTra]) REFERENCES [dbo].[tblHoSoTra] ([MaHSTra]);
ALTER TABLE [dbo].[tblChiTietHSTra] WITH CHECK ADD CONSTRAINT [FK_ChiTietHSTra_ViPham] FOREIGN KEY ([MaViPham]) REFERENCES [dbo].[tblViPham] ([MaViPham]);
ALTER TABLE [dbo].[tblDMSach] WITH CHECK ADD CONSTRAINT [FK_DMSach_NgonNgu] FOREIGN KEY ([MaNgonNgu]) REFERENCES [dbo].[tblNgonNgu] ([MaNgonNgu]);
ALTER TABLE [dbo].[tblDMSach] WITH CHECK ADD CONSTRAINT [FK_DMSach_NXB] FOREIGN KEY ([MaNXB]) REFERENCES [dbo].[tblNXB] ([MaNXB]);
ALTER TABLE [dbo].[tblDMSach] WITH CHECK ADD CONSTRAINT [FK_DMSach_TheLoai] FOREIGN KEY ([MaTheLoai]) REFERENCES [dbo].[tblTheLoai] ([MaTheLoai]);
ALTER TABLE [dbo].[tblHoSoMuon] WITH CHECK ADD CONSTRAINT [FK_HoSoMuon_TheMuon] FOREIGN KEY ([MaThe]) REFERENCES [dbo].[tblTheMuon] ([MaThe]);
ALTER TABLE [dbo].[tblHoSoMuon] WITH CHECK ADD CONSTRAINT [FK_HoSoMuon_ThuThu] FOREIGN KEY ([MaThuThu]) REFERENCES [dbo].[tblThuThu] ([MaThuThu]);
ALTER TABLE [dbo].[tblHoSoTra] WITH CHECK ADD CONSTRAINT [FK_HoSoTra_HoSoMuon] FOREIGN KEY ([MaHSM]) REFERENCES [dbo].[tblHoSoMuon] ([MaHSM]);
ALTER TABLE [dbo].[tblHoSoTra] WITH CHECK ADD CONSTRAINT [FK_HoSoTra_ThuThu] FOREIGN KEY ([MaThuThu]) REFERENCES [dbo].[tblThuThu] ([MaThuThu]);
ALTER TABLE [dbo].[tblTheMuon] WITH CHECK ADD CONSTRAINT [FK_TheMuon_DanToc] FOREIGN KEY ([MaDanToc]) REFERENCES [dbo].[tblDanToc] ([MaDanToc]);
ALTER TABLE [dbo].[tblTheMuon] WITH CHECK ADD CONSTRAINT [FK_TheMuon_Que] FOREIGN KEY ([MaQue]) REFERENCES [dbo].[tblQue] ([MaQue]);
ALTER TABLE [dbo].[tblThuThu] WITH CHECK ADD CONSTRAINT [FK_ThuThu_Que] FOREIGN KEY ([MaQue]) REFERENCES [dbo].[tblQue] ([MaQue]);
ALTER TABLE [dbo].[tblDMSach] WITH CHECK ADD CONSTRAINT [CK_DMSach_SoLuong] CHECK ([SoLuong]>=(0));
COMMIT TRANSACTION;
