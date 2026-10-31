QUẢN LÝ THƯ VIỆN QUỐC GIA — BẢN HOÀN THIỆN THEO ĐỀ TÀI 18
Cập nhật: 26/10/2026

1. PHẠM VI

Nguồn yêu cầu: 18_Quan ly thu vien QG (1).xls, Sheet1.
Thực hiện đủ 9 nhóm nghiệp vụ mượn/trả, tìm kiếm và báo cáo trong đề.
Tài liệu “YÊU CẦU FORM HÓA ĐƠN” không được dùng làm yêu cầu cho dự án này.
Đề không yêu cầu đăng nhập; các mục tài khoản cũ được ẩn khỏi menu.

2. KHỞI ĐỘNG

- Mở QuanLyThuVienQuocGia.slnx hoặc QuanLyThuVienQuocGia.csproj.
- Build Debug hoặc Release, nền tảng x64, .NET Framework 4.7.2.
- Máy cần SQL Server LocalDB instance MSSQLLocalDB, .NET Framework 4.7.2,
  Crystal Reports runtime x64 để dùng báo cáo danh sách sách cũ.
- Giữ các NuGet package hiện có trong packages (EPPlus và các dependency).
- Chạy bin\Debug\QuanLyThuVienQuocGia.exe hoặc bản trong bin\Release.
- Khi chạy trong cây thư mục dự án, cả Debug và Release cùng dùng
  Database\DuLieu.mdf. Không dùng bản cũ trong bin\Debug\Database nữa.
- Khi chuyển máy: sao chép toàn bộ dự án, cài các dependency, rồi build lại.
  Nếu chỉ phân phối thư mục chương trình, đặt Database\DuLieu.mdf và nhật ký
  đi kèm bên cạnh EXE. Sao chép MDF khi đã đóng kết nối SQL Server hoặc dùng
  sao lưu/phục hồi .bak; không copy đè tệp CSDL đang mở.
- CSDL tự nâng cấp một lần khi khởi động; chạy lại không xóa tồn kho hay hồ sơ.
- Chương trình tự khởi động LocalDB trước khi kết nối, đợi server sẵn sàng và
  dùng địa chỉ pipe thực tế. Nếu LocalDB tự dừng khi không sử dụng, lần kết nối
  tiếp theo sẽ khởi động lại; không tự chạy lại giao dịch ghi dữ liệu.

3. DỮ LIỆU CŨ SAU NÂNG CẤP

- Đã giữ đủ 6 sách và danh mục NXB tương ứng từ bản chạy Debug cũ.
  Tác giả của sách mã 1 giữ giá trị mới trong bản chạy: Nguyễn Văn An.
- Giữ nguyên hồ sơ mượn cũ và các sách chưa trả; số sách đang mượn trên thẻ
  được đồng bộ từ các chi tiết chưa trả.
- Đề cũ chưa có cột số lượng: SoLuong mới được đặt bằng 0, không tự đoán tồn.
  Vào Danh mục → Quản lý sách, chọn từng sách, nhập tồn thực tế rồi Lưu.
- Sách mã 5 và 6 chưa có đơn giá thuê trong dữ liệu cũ: cần bổ sung đơn giá
  trước khi cho mượn. Các thông tin chưa biết khác vẫn giữ trống.
- Cột vật lý MaXB được giữ để tương thích DataSet; trên giao diện là Năm XB.
  KhoSach được trình bày là Khổ sách.
- Đơn giá của hồ sơ mới được chốt lúc mượn. Hồ sơ cũ không có đơn giá chốt
  sẽ dùng đơn giá hiện tại khi trả; không thể khôi phục giá lịch sử đã mất.
- Bản sao trước nâng cấp nằm tại thư mục Backups\BeforeUpgrade_20261026_140340
  bên ngoài thư mục dự án: SourceBeforeUpgrade.zip và hai bản .bak.

4. QUẢN LÝ DANH MỤC

Danh mục → Quản lý sách / thể loại / NXB / ngôn ngữ / dân tộc / quê / thủ thư.
Bạn đọc → Quản lý thẻ mượn / Danh mục vi phạm và tiền phạt.

- Chọn dòng để sửa; Lưu cập nhật dòng đó. Thêm mới xóa vùng nhập và tạo mã
  mới khi Lưu. Xóa có xác nhận và bị chặn nếu bản ghi đang được tham chiếu.
- Form sách hỗ trợ toàn bộ 14 trường của đề, bao gồm SoLuong và DonGiaThue.
- Tìm kiếm sách theo tên, thể loại hoặc NXB. Có thể xuất bảng ra Excel.
- Form thẻ chứa đủ thông tin bạn đọc, ngày làm thẻ và hết hạn. SoLuongMuon
  chỉ đọc; được tự tính khi mượn và trả.
- Form thủ thư tìm theo tên, chọn thủ thư rồi nhấn Hồ sơ đã cho mượn để lọc.

5. MƯỢN SÁCH

Bạn đọc → Lập phiếu mượn.

- Chọn thẻ, thủ thư, ngày mượn, tiền tạm ứng; đánh dấu các sách rồi Lưu.
- Thẻ phải còn hạn ở ngày hiện tại, đã đến ngày làm thẻ ở ngày mượn.
  Không được chọn ngày mượn trong tương lai.
- Trước giao dịch số đang mượn phải < 3; sau giao dịch tối đa 3 cuốn/thẻ.
  Không chọn trùng một đầu sách trong cùng hồ sơ.
- Mỗi sách phải còn trong kho và có đơn giá không âm. Giá 0 cho phép mượn
  miễn phí; giá trống bị chặn. Tạm ứng không âm.
- Lưu hồ sơ, chi tiết, giảm tồn và tăng số lượng mượn trong một transaction.
  Nếu một sách không hợp lệ thì toàn bộ lần lưu được rollback.

6. TRẢ SÁCH, PHẠT VÀ TẠM ỨNG

Bạn đọc → Lập phiếu trả.

- Combo hồ sơ chỉ có hồ sơ còn sách chưa trả. Bảng chỉ có sách chưa trả của
  hồ sơ được chọn. Chọn các sách trả ở lần này, ngày trả, thủ thư và vi phạm.
- Số ngày thuê = Ngày trả - Ngày mượn, tính theo ngày lịch. Trả cùng ngày
  tính 0 ngày. Không cho ngày trả trước ngày mượn hoặc sau ngày hiện tại.
- Thành tiền mỗi sách = Số ngày thuê × Đơn giá lúc mượn + Tiền phạt.
  Mỗi chi tiết lưu số ngày, đơn giá và phạt áp dụng để giữ lịch sử.
- Một hồ sơ có thể trả nhiều lần. Các lần trước thu phí của sách vừa trả.
  Tạm ứng chỉ trừ đúng một lần ở lần trả cuối: SoTienThanhToan = TongTien
  của lần trả - TamUngApDung. Nếu âm, chương trình ghi số cần hoàn bạn đọc.
  Quy ước này tránh trừ lặp tạm ứng của cùng một hồ sơ.
- Mỗi lần lưu tạo phiếu trả riêng, đánh dấu đã trả, tăng tồn, giảm số đang
  mượn của thẻ trong cùng transaction. Chặn trả trùng và trả sách hồ sơ khác.

7. BÁO CÁO

Báo cáo → Tiền thu theo tháng / quý / năm:
- Chi tiết gồm mã sách, ngày mượn, ngày trả, tiền thuê/phạt và tiền thanh toán.
- Khoảng ngày lọc theo ngày trả; bật/tắt Lọc ngày để chọn tất cả thời gian.
- Tab Tổng hợp chứa các cấp tháng, quý, năm. Không cộng ba cấp với nhau.
- Tổng phí và số thu thêm/hoàn sau tạm ứng được trình bày riêng. Tạm ứng
  được phân bổ đúng một lần mỗi phiếu để không nhân tổng theo số sách.

Các báo cáo khác:
- Sách đang được mượn chưa trả.
- Hồ sơ mượn theo thẻ hoặc thủ thư; nhấp đúp để xem chi tiết sách.
- Hồ sơ trả và thanh toán; nhấp đúp để xem chi tiết tiền và vi phạm.
- Sách vi phạm khi trả.
- Báo cáo danh sách sách Crystal Reports cũ vẫn được giữ.

Các bảng báo cáo hỗ trợ Xuất Excel và In/PDF. Chọn máy in PDF trong hộp in
để lưu PDF. Excel giữ ngày và tiền ở dạng dữ liệu có kiểu.

8. SAO LƯU VÀ PHỤC HỒI

Hệ thống → Sao lưu dữ liệu: tạo bản .bak COPY_ONLY.
Hệ thống → Khôi phục dữ liệu:
- Yêu cầu xác nhận trên giao diện vì thay dữ liệu hiện tại.
- Phục hồi bản đã chọn vào CSDL tạm, kiểm tra 13 bảng và chạy migration.
- Tự tạo bản sao hiện tại ở Database\Backups\BeforeRestore_*.bak.
- Sau đó mới thay CSDL đang dùng; nếu bước restore lỗi, thử phục hồi bản tự lưu.
- Dùng bản .bak của thư viện; không chọn CSDL của ứng dụng khác.

9. MÃ NGUỒN CHÍNH

LibraryDb.cs             Truy vấn tham số hóa, transaction, mượn/trả.
LocalDbStartup.cs        Tìm và khởi động LocalDB, kiểm tra kết nối trước khi mở.
LibraryUpgrade.sql       Bổ sung tồn, đơn giá chốt và dữ liệu quyết toán.
Catalog.cs               Định nghĩa các danh mục, kiểm tra dữ liệu và CRUD.
frmCatalog.cs            Form danh mục dùng chung, tìm kiếm, xuất Excel.
frmCirculation.cs        Lập hồ sơ mượn, trả từng phần, xem trước tiền.
frmReports.cs            Truy vấn và tổng hợp báo cáo, chi tiết hồ sơ.
LibraryUi.cs             Giao diện dùng chung, xuất Excel, xem trước/in.
DatabaseMaintenance.cs   Sao lưu, kiểm tra và phục hồi CSDL.
ProcessDataBase.cs       Đường dẫn CSDL chung và lớp truy cập cũ.
frmMain.cs               Kết nối menu đến các chức năng đã hoàn thiện.
frmHuongDan.cs           Hướng dẫn sử dụng trong chương trình.

frmDMSach/frmDataSetDataTable cũ được giữ trong source để tham khảo bài thực
hành; menu quản lý/tra cứu sách hiện dùng frmCatalog có đầy đủ trường.

10. KIỂM THỬ

Chạy PowerShell tại thư mục dự án:
  .\Tests\RunTests.ps1
Nếu MSBuild cài ở vị trí khác, truyền -MSBuildPath với đường dẫn tương ứng.

Script tạo bản sao CSDL tên QLTV_Test_<GUID>, chạy thử rồi drop bản sao.
Không thêm dữ liệu thử vào CSDL thực. Kết quả trong Tests\Work:
  IntegrationResults.txt: kiểm tra migration, tìm kiếm, thẻ hết hạn, số lượng,
  rollback, trả từng phần, tiền phạt, chốt giá, tạm ứng, hoàn tiền, truy cập
  đồng thời, sao lưu và phục hồi.
  UiResults.txt: tải 9 danh mục, 5 báo cáo, mượn/trả, checkbox/xem trước,
  tổng hợp doanh thu và xuất Excel đúng kiểu.
  StartupResults.txt: khởi động instance thử từ trạng thái dừng và khởi động
  lặp lại khi đã chạy. Instance thử được xóa sau kiểm tra.

Bản nâng cấp đã build Debug và Release; đã kiểm tra giao diện chính, quản lý
sách và form trả sách trực tiếp. Các số liệu trả sách thử chỉ ở bản sao CSDL.
