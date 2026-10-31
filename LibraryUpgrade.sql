SET XACT_ABORT ON;
IF COL_LENGTH('dbo.tblDMSach','SoLuong') IS NULL
    ALTER TABLE dbo.tblDMSach ADD SoLuong int NOT NULL CONSTRAINT DF_DMSach_SoLuong DEFAULT(0) WITH VALUES;
IF NOT EXISTS(SELECT 1 FROM sys.check_constraints WHERE name='CK_DMSach_SoLuong')
    EXEC(N'ALTER TABLE dbo.tblDMSach WITH CHECK ADD CONSTRAINT CK_DMSach_SoLuong CHECK(SoLuong>=0)');
IF COL_LENGTH('dbo.tblChiTietHSMuon','DonGiaThueLucMuon') IS NULL
    ALTER TABLE dbo.tblChiTietHSMuon ADD DonGiaThueLucMuon decimal(18,2) NULL;
IF COL_LENGTH('dbo.tblHoSoTra','TamUngApDung') IS NULL
    ALTER TABLE dbo.tblHoSoTra ADD TamUngApDung decimal(18,2) NOT NULL CONSTRAINT DF_HSTra_TamUng DEFAULT(0) WITH VALUES;
IF COL_LENGTH('dbo.tblChiTietHSTra','SoNgayThue') IS NULL
    ALTER TABLE dbo.tblChiTietHSTra ADD SoNgayThue int NULL, DonGiaThue decimal(18,2) NULL, TienPhat decimal(18,2) NULL;
-- Existing stock is unknown: leave zero until the librarian counts the shelf.
-- Existing loans without a snapshot use the book's current tariff when returned.
UPDATE c SET SoLuongMuon=(SELECT COUNT(*) FROM dbo.tblChiTietHSMuon d JOIN dbo.tblHoSoMuon h ON h.MaHSM=d.MaHSM WHERE h.MaThe=c.MaThe AND d.ChuaTra=1)
FROM dbo.tblTheMuon c;
