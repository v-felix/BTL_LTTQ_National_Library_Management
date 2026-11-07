-- 30 đầu sách mẫu cho bài tập. Năm XB, số trang, NXB, giá, tồn và phí là dữ liệu mô phỏng.
-- Chạy lại an toàn: không thêm trùng tên sách + tác giả và không sửa sách đang có.
SET NOCOUNT ON;
SET XACT_ABORT ON;
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
BEGIN TRY
BEGIN TRANSACTION;
DECLARE @Books TABLE (STT int,TenSach nvarchar(200),TenTG nvarchar(100),NXB nvarchar(100),TheLoai nvarchar(100),NgonNgu nvarchar(100),NamXB int,KhoSach nvarchar(50),GiaSach decimal(18,2),LanTaiBan int,SoTrang int,TomTatND nvarchar(max),SoLuong int,DonGiaThue decimal(18,2));
INSERT @Books VALUES
(1,N'Tôi thấy hoa vàng trên cỏ xanh',N'Nguyễn Nhật Ánh',N'NXB Trẻ',N'Văn học',N'Tiếng Việt',2015,N'16 x 24 cm',125000,1,378,N'Câu chuyện tuổi thơ và tình cảm gia đình ở một làng quê Việt Nam.',8,3000),
(2,N'Mắt biếc',N'Nguyễn Nhật Ánh',N'NXB Trẻ',N'Tiểu thuyết',N'Tiếng Việt',2019,N'16 x 24 cm',110000,1,300,N'Tiểu thuyết về tuổi học trò, tình bạn và những rung động đầu đời.',6,3000),
(3,N'Số đỏ',N'Vũ Trọng Phụng',N'NXB Văn học',N'Văn học',N'Tiếng Việt',2020,N'16 x 24 cm',85000,1,240,N'Tiểu thuyết trào phúng phản ánh đời sống xã hội Việt Nam đầu thế kỷ XX.',5,2000),
(4,N'Tắt đèn',N'Ngô Tất Tố',N'NXB Văn học',N'Văn học',N'Tiếng Việt',2021,N'16 x 24 cm',75000,1,220,N'Tác phẩm về thân phận người nông dân qua hình tượng chị Dậu.',7,2000),
(5,N'Truyện Kiều',N'Nguyễn Du',N'NXB Văn học',N'Thơ ca',N'Tiếng Việt',2020,N'16 x 24 cm',95000,1,320,N'Truyện thơ về cuộc đời Thúy Kiều và những giá trị nhân văn.',6,2000),
(6,N'Nhật ký trong tù',N'Hồ Chí Minh',N'NXB Văn học',N'Thơ ca',N'Tiếng Việt',2022,N'16 x 24 cm',65000,1,180,N'Tập thơ thể hiện nghị lực, tình yêu thiên nhiên và khát vọng tự do.',5,2000),
(7,N'Le Petit Prince',N'Antoine de Saint-Exupéry',N'NXB Thế giới',N'Thiếu nhi',N'Tiếng Pháp',2021,N'16 x 24 cm',150000,1,120,N'Ấn bản tiếng Pháp về hành trình khám phá tình bạn, tình yêu và trách nhiệm.',4,3500),
(8,N'Charlotte''s Web',N'E. B. White',N'NXB Thế giới',N'Thiếu nhi',N'Tiếng Anh',2020,N'16 x 24 cm',180000,1,192,N'Câu chuyện về tình bạn giữa chú lợn Wilbur và cô nhện Charlotte.',4,3500),
(9,N'Harry Potter and the Philosopher''s Stone',N'J. K. Rowling',N'NXB Thế giới',N'Giả tưởng',N'Tiếng Anh',2022,N'16 x 24 cm',250000,1,336,N'Cuộc phiêu lưu đầu tiên của Harry Potter tại trường phù thủy Hogwarts.',6,5000),
(10,N'The Hobbit',N'J. R. R. Tolkien',N'NXB Thế giới',N'Giả tưởng',N'Tiếng Anh',2021,N'16 x 24 cm',220000,1,310,N'Hành trình của Bilbo Baggins cùng những người lùn đến ngọn núi cô độc.',5,4500),
(11,N'A Study in Scarlet',N'Arthur Conan Doyle',N'NXB Thế giới',N'Trinh thám',N'Tiếng Anh',2020,N'16 x 24 cm',165000,1,176,N'Vụ án mở đầu cho sự hợp tác giữa Sherlock Holmes và bác sĩ Watson.',4,3500),
(12,N'Murder on the Orient Express',N'Agatha Christie',N'NXB Thế giới',N'Trinh thám',N'Tiếng Anh',2023,N'16 x 24 cm',190000,1,256,N'Thám tử Hercule Poirot điều tra một vụ án trên chuyến tàu tốc hành.',5,4000),
(13,N'Sapiens: Lược sử loài người',N'Yuval Noah Harari',N'NXB Thế giới',N'Lịch sử',N'Tiếng Việt',2021,N'16 x 24 cm',250000,1,544,N'Khái quát các bước phát triển của loài người từ tiền sử đến xã hội hiện đại.',6,4500),
(14,N'Việt Nam sử lược',N'Trần Trọng Kim',N'NXB Văn học',N'Lịch sử',N'Tiếng Việt',2020,N'16 x 24 cm',210000,1,620,N'Tổng quan lịch sử Việt Nam qua các triều đại và sự kiện quan trọng.',5,4000),
(15,N'A Brief History of Time',N'Stephen Hawking',N'NXB Thế giới',N'Khoa học',N'Tiếng Anh',2022,N'16 x 24 cm',230000,1,256,N'Giới thiệu vũ trụ, thời gian, lỗ đen và các câu hỏi nền tảng của vật lý.',4,4500),
(16,N'Cosmos',N'Carl Sagan',N'NXB Thế giới',N'Khoa học',N'Tiếng Anh',2021,N'16 x 24 cm',280000,1,396,N'Khám phá vũ trụ, lịch sử thiên văn và vị trí của con người trong tự nhiên.',4,5000),
(17,N'Clean Code',N'Robert C. Martin',N'NXB Khoa học và Kỹ thuật',N'Tin học',N'Tiếng Anh',2021,N'16 x 24 cm',420000,1,464,N'Nguyên tắc viết mã rõ ràng, đặt tên, tổ chức hàm và cải thiện chất lượng phần mềm.',7,6000),
(18,N'The Pragmatic Programmer',N'Andrew Hunt và David Thomas',N'NXB Khoa học và Kỹ thuật',N'Tin học',N'Tiếng Anh',2020,N'16 x 24 cm',390000,1,352,N'Các thực hành và tư duy giúp lập trình viên làm việc hiệu quả, có trách nhiệm.',6,5500),
(19,N'Introduction to Algorithms',N'Thomas H. Cormen và cộng sự',N'NXB Khoa học và Kỹ thuật',N'Tin học',N'Tiếng Anh',2022,N'16 x 24 cm',680000,1,1312,N'Giáo trình cấu trúc dữ liệu, phân tích thuật toán, sắp xếp và đồ thị.',3,7000),
(20,N'Tâm lý học đám đông',N'Gustave Le Bon',N'NXB Thế giới',N'Tâm lý học',N'Tiếng Việt',2021,N'16 x 24 cm',120000,1,240,N'Phân tích đặc điểm tâm lý và hành vi của cá nhân trong đám đông.',6,3000),
(21,N'Thinking, Fast and Slow',N'Daniel Kahneman',N'NXB Thế giới',N'Tâm lý học',N'Tiếng Anh',2020,N'16 x 24 cm',290000,1,512,N'Giới thiệu hai hệ thống tư duy và những sai lệch trong phán đoán.',5,4500),
(22,N'Đắc nhân tâm',N'Dale Carnegie',N'NXB Tổng hợp TP. Hồ Chí Minh',N'Kỹ năng sống',N'Tiếng Việt',2022,N'16 x 24 cm',115000,1,320,N'Các nguyên tắc giao tiếp, lắng nghe và xây dựng quan hệ với người khác.',10,2500),
(23,N'Atomic Habits',N'James Clear',N'NXB Thế giới',N'Kỹ năng sống',N'Tiếng Anh',2023,N'16 x 24 cm',260000,1,320,N'Phương pháp xây dựng thói quen nhỏ và cải thiện hành vi từng ngày.',8,4000),
(24,N'The Lean Startup',N'Eric Ries',N'NXB Thế giới',N'Kinh doanh',N'Tiếng Anh',2021,N'16 x 24 cm',275000,1,336,N'Mô hình xây dựng sản phẩm, đo lường và học hỏi trong khởi nghiệp.',5,4500),
(25,N'Nhà đầu tư thông minh',N'Benjamin Graham',N'NXB Lao động',N'Kinh tế - Tài chính',N'Tiếng Việt',2022,N'16 x 24 cm',280000,1,644,N'Giới thiệu tư duy đầu tư giá trị và cách phân tích rủi ro tài chính.',4,4500),
(26,N'The Psychology of Money',N'Morgan Housel',N'NXB Thế giới',N'Kinh tế - Tài chính',N'Tiếng Anh',2023,N'16 x 24 cm',240000,1,256,N'Những câu chuyện về hành vi, cảm xúc và cách con người nhìn nhận tiền bạc.',6,4000),
(27,N'The Story of Art',N'E. H. Gombrich',N'NXB Mỹ thuật',N'Nghệ thuật',N'Tiếng Anh',2020,N'16 x 24 cm',750000,1,688,N'Khảo lược các giai đoạn phát triển của hội họa, điêu khắc và kiến trúc.',3,7000),
(28,N'Bếp Việt mỗi ngày',N'Nhóm biên soạn Ẩm thực',N'NXB Phụ nữ Việt Nam',N'Ẩm thực',N'Tiếng Việt',2024,N'16 x 24 cm',135000,1,224,N'Sách mẫu: công thức món ăn gia đình, lựa chọn thực phẩm và cân đối thực đơn.',7,2500),
(29,N'Yoga và sức khỏe hằng ngày',N'Nhóm biên soạn Sức khỏe',N'NXB Thể thao và Du lịch',N'Sức khỏe',N'Tiếng Việt',2024,N'16 x 24 cm',145000,1,200,N'Sách mẫu: giới thiệu vận động nhẹ, thư giãn và thói quen sinh hoạt lành mạnh.',6,2500),
(30,N'Tiếng Nhật căn bản Việt - Nhật',N'Nhóm biên soạn Ngoại ngữ',N'NXB Đại học Quốc gia Hà Nội',N'Ngoại ngữ',N'Việt - Nhật',2024,N'16 x 24 cm',185000,1,360,N'Sách mẫu: bảng chữ cái, từ vựng, mẫu câu và hội thoại tiếng Nhật căn bản.',8,3500);
DECLARE @Max int;
SELECT @Max=ISNULL(MAX(MaTheLoai),0) FROM dbo.tblTheLoai WITH(UPDLOCK,HOLDLOCK);
INSERT dbo.tblTheLoai(MaTheLoai,TenTheLoai)
SELECT @Max+ROW_NUMBER() OVER(ORDER BY b.TheLoai),b.TheLoai
FROM (SELECT DISTINCT TheLoai FROM @Books) b WHERE NOT EXISTS(SELECT 1 FROM dbo.tblTheLoai t WHERE t.TenTheLoai=b.TheLoai);
SELECT @Max=ISNULL(MAX(MaNXB),0) FROM dbo.tblNXB WITH(UPDLOCK,HOLDLOCK);
INSERT dbo.tblNXB(MaNXB,TenNXB)
SELECT @Max+ROW_NUMBER() OVER(ORDER BY b.NXB),b.NXB
FROM (SELECT DISTINCT NXB FROM @Books) b WHERE NOT EXISTS(SELECT 1 FROM dbo.tblNXB n WHERE n.TenNXB=b.NXB);
SELECT @Max=ISNULL(MAX(MaNgonNgu),0) FROM dbo.tblNgonNgu WITH(UPDLOCK,HOLDLOCK);
INSERT dbo.tblNgonNgu(MaNgonNgu,TenNgonNgu)
SELECT @Max+ROW_NUMBER() OVER(ORDER BY b.NgonNgu),b.NgonNgu
FROM (SELECT DISTINCT NgonNgu FROM @Books) b WHERE NOT EXISTS(SELECT 1 FROM dbo.tblNgonNgu n WHERE n.TenNgonNgu=b.NgonNgu);
SELECT @Max=ISNULL(MAX(MaSach),0) FROM dbo.tblDMSach WITH(UPDLOCK,HOLDLOCK);
DECLARE @Added TABLE (MaSach int,TenSach nvarchar(200));
INSERT dbo.tblDMSach(MaSach,TenSach,TenTG,MaNXB,MaXB,KhoSach,GiaSach,LanTaiBan,MaTheLoai,MaNgonNgu,SoTrang,TomTatND,SoLuong,DonGiaThue)
OUTPUT inserted.MaSach,inserted.TenSach INTO @Added
SELECT @Max+ROW_NUMBER() OVER(ORDER BY b.STT),b.TenSach,b.TenTG,
 (SELECT MIN(n.MaNXB) FROM dbo.tblNXB n WHERE n.TenNXB=b.NXB),
 b.NamXB,b.KhoSach,b.GiaSach,b.LanTaiBan,
 (SELECT MIN(t.MaTheLoai) FROM dbo.tblTheLoai t WHERE t.TenTheLoai=b.TheLoai),
 (SELECT MIN(n.MaNgonNgu) FROM dbo.tblNgonNgu n WHERE n.TenNgonNgu=b.NgonNgu),
 b.SoTrang,b.TomTatND,b.SoLuong,b.DonGiaThue
FROM @Books b WHERE NOT EXISTS(SELECT 1 FROM dbo.tblDMSach s WHERE s.TenSach=b.TenSach AND s.TenTG=b.TenTG);
IF EXISTS(SELECT 1 FROM @Books b WHERE NOT EXISTS(SELECT 1 FROM dbo.tblDMSach s WHERE s.TenSach=b.TenSach AND s.TenTG=b.TenTG))
 THROW 50001,N'Chưa thêm đủ các đầu sách mẫu.',1;
COMMIT TRANSACTION;
SELECT COUNT(*) AS AddedBooks FROM @Added;
SELECT * FROM @Added ORDER BY MaSach;
SELECT COUNT(*) AS TotalBooks FROM dbo.tblDMSach;
END TRY
BEGIN CATCH
 IF @@TRANCOUNT>0 ROLLBACK TRANSACTION;
 THROW;
END CATCH;

