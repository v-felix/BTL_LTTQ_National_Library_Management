# Quản lý Thư viện Quốc gia

Bài tập lớn Lập trình trực quan, xây dựng bằng **C# Windows Forms, .NET Framework 4.7.2, SQL Server LocalDB**. Chức năng thực hiện theo [đề tài 18](<Docs/18_Quan ly thu vien QG (1).xls>).

## Chức năng

- Quản lý sách với đầy đủ thông tin xuất bản, tồn kho và đơn giá thuê.
- Quản lý thẻ mượn, thủ thư, thể loại, NXB, ngôn ngữ, dân tộc, quê quán và mức phạt.
- Mượn sách: kiểm tra hạn thẻ, tồn kho và giới hạn 3 cuốn đang mượn/thẻ.
- Trả sách nhiều lần, chốt đơn giá lúc mượn, tính tiền thuê/phạt và quyết toán tạm ứng một lần ở lần trả cuối.
- Tự cập nhật tồn kho và số sách đang mượn trong một giao dịch CSDL.
- Tìm sách theo tên/thể loại/NXB; tìm thủ thư và hồ sơ cho mượn.
- Báo cáo tháng/quý/năm, sách chưa trả và hồ sơ mượn theo thẻ/thủ thư.
- Xuất Excel, xem trước/in PDF, sao lưu và phục hồi CSDL.

## Yêu cầu môi trường

- Windows 64 bit; Visual Studio có workload **.NET desktop development**.
- .NET Framework **4.7.2 Developer Pack**.
- SQL Server Express **LocalDB**, instance `MSSQLLocalDB`; công cụ `SqlLocalDB` có trong `PATH`.
- SAP Crystal Reports for Visual Studio và runtime **x64** tương thích các assembly `13.0.4000.0` của dự án.
- Microsoft Excel bản desktop cài trên máy và reference Microsoft Office Interop Excel 15.0.
- Restore các package còn lại trong `packages.config`.

## Clone và chạy

```powershell
git clone https://github.com/v-felix/BTL_LTTQ_National_Library_Management.git
cd BTL_LTTQ_National_Library_Management
```

1. Tạo CSDL lần đầu bằng PowerShell:

   ```powershell
   .\Database\SetupDatabase.ps1
   ```

   Script tạo **CSDL trống gồm 13 bảng**, khóa và ràng buộc từ `Database/Schema.sql`. Script từ chối ghi đè nếu đã có `DuLieu.mdf` hoặc `DuLieu_log.ldf`. Nhập danh mục, sách, thủ thư và thẻ mượn trên giao diện trước khi lập hồ sơ mượn.

2. Mở `QuanLyThuVienQuocGia.csproj` hoặc solution `QuanLyThuVienQuocGia.slnx` bằng Visual Studio.
3. Restore NuGet Packages để tải lại thư mục `packages`. Có thể dùng **Developer PowerShell for Visual Studio**:

   ```powershell
   MSBuild .\QuanLyThuVienQuocGia.csproj /t:Restore /p:RestorePackagesConfig=true
   MSBuild .\QuanLyThuVienQuocGia.csproj /t:Build /p:Configuration=Debug
   ```

4. Chạy **Debug / AnyCPU** (dự án đã đặt `PlatformTarget=x64`) bằng F5, hoặc mở `bin\Debug\QuanLyThuVienQuocGia.exe`.

### Dữ liệu mẫu 30 đầu sách

Sau khi tạo CSDL, mở `Database/SeedBooks.sql` trong SQL Server Object Explorer hoặc SQL Server Management Studio và chạy trên CSDL `DuLieu` vừa tạo. Script bổ sung 30 đầu sách thuộc 17 thể loại, 4 ngôn ngữ, có đầy đủ thông tin và tồn kho để thử mượn/trả, tìm kiếm và xuất Excel. Chạy lại không tạo trùng tên sách và tác giả, đồng thời giữ nguyên sách đã có.

Năm xuất bản, NXB, số trang, giá, tồn kho và đơn giá thuê trong bộ mẫu là dữ liệu mô phỏng phục vụ bài tập, không phải thông tin xác thực của từng ấn bản.

Ứng dụng tự khởi động LocalDB và nâng cấp schema khi cần. Debug và Release cùng dùng `Database\DuLieu.mdf` ở thư mục dự án. Tệp CSDL đang sử dụng, thông tin bạn đọc, bản sao lưu và kết quả chạy thử **không được đưa vào Git**.

## Xuất Excel theo bài giảng Tuần 7

Các nút Xuất Excel dùng `Microsoft.Office.Interop.Excel`: tạo `Application`, `Workbook`, `Worksheet`, thao tác `Range`, gọi `Activate`, `SaveAs`, đóng workbook và `Quit` trong `finally`.

File có thông tin đầu trang màu xanh, tiêu đề báo cáo màu đỏ căn giữa ở dòng 5, font Times New Roman, dòng tiêu đề bảng ở dòng 7 và cột STT. Ngày/tiền giữ giá trị số; tên sách bắt đầu bằng dấu `=` vẫn là văn bản. Hộp thoại lưu cho phép chọn nơi lưu `.xlsx`. Excel được tạo trong instance riêng và tự đóng sau khi lưu.

## Quy tắc tính tiền

- Số ngày thuê = ngày trả − ngày mượn; trả cùng ngày tính 0 ngày.
- Thành tiền sách = số ngày thuê × đơn giá lúc mượn + tiền phạt.
- Khi trả từng phần, các lần đầu thu phí của sách vừa trả. Tạm ứng được trừ **đúng một lần ở lần trả cuối**; số thanh toán âm là tiền hoàn bạn đọc.
- Tồn kho là số cuốn hiện còn trên kệ. Chương trình không tự suy đoán tồn kho của dữ liệu cũ.

## Kiểm thử

Sau khi tạo CSDL và cài các dependency:

```powershell
.\Tests\RunTests.ps1 -MSBuildPath 'C:\duong-dan-den\MSBuild.exe'
```

Nếu không truyền `-MSBuildPath`, script dùng đường dẫn Visual Studio 18 Insiders của máy phát triển ban đầu. Bộ kiểm thử tạo bản sao CSDL riêng và instance LocalDB riêng cho kiểm tra khởi động; không thêm dữ liệu thử vào CSDL đang dùng. Kết quả được ghi trong `Tests/Work/` (được Git bỏ qua).

Để kiểm tra riêng phần xuất Excel mà không cần CSDL, chạy `.\Tests\RunExcelTests.ps1` (cũng hỗ trợ tham số `-MSBuildPath`). Cần Microsoft Excel desktop. Kết quả và file xuất thử nằm trong `Tests/Work/`.

## Mã nguồn

| Thành phần | Vai trò |
| --- | --- |
| `LibraryDb.cs` | Truy vấn có tham số, giao dịch mượn/trả và cập nhật tồn |
| `LocalDbStartup.cs` | Khởi động LocalDB, kiểm tra kết nối và lấy địa chỉ pipe |
| `Catalog.cs`, `frmCatalog.cs` | Định nghĩa, kiểm tra, quản lý và tìm kiếm danh mục |
| `frmCirculation.cs` | Lập hồ sơ mượn/trả, xem trước thanh toán |
| `frmReports.cs` | Tra cứu, báo cáo và tổng hợp tiền thu |
| `DatabaseMaintenance.cs` | Sao lưu và phục hồi có kiểm tra bản sao |
| `Database/Schema.sql` | Schema CSDL để tạo mới từ bản clone |
| `LibraryUpgrade.sql` | Migration giữ tương thích với CSDL đang có |

Xem [hướng dẫn sử dụng chi tiết](README_QuanLyThuVienQuocGia.txt). Tài liệu này cũng ghi lại việc nâng cấp dữ liệu trên máy phát triển ban đầu; bản clone bắt đầu với CSDL trống do script tạo.
