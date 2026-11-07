using System;
using System.Windows.Forms;

namespace QuanLyThuVienQuocGia
{
    public partial class frmHuongDan : Form
    {
        public frmHuongDan(){InitializeComponent();btnDong.CausesValidation=false;UiTheme.Apply(this);}
        private void frmHuongDan_Load(object sender,EventArgs e){wbNoiDung.DocumentText=SinhNoiDungHtml();}
        private void btnDong_Click(object sender,EventArgs e){Close();}
        private void lblTieuDe_Click(object sender,EventArgs e) { }
        private void btnIn_Click(object sender,EventArgs e){LibraryUi.Run(()=>wbNoiDung.ShowPrintPreviewDialog());}
        private string SinhNoiDungHtml()
        {
            return @"<!doctype html><html><head><meta charset='utf-8'><meta http-equiv='X-UA-Compatible' content='IE=edge'>
<style>body{font:15px 'Segoe UI',Arial;line-height:1.7;color:#203044;background:#f1f5fa;margin:28px}main{background:white;padding:28px;max-width:900px;margin:auto}h1,h2{color:#0067c0}h2{margin-top:28px}li{margin-bottom:6px}code{background:#edf2f7;padding:2px 5px}@media print{body{margin:0;background:white}}</style></head><body><main>
<h1>Hướng dẫn quản lý thư viện quốc gia</h1><p>Chức năng được xây dựng theo đề tài 18 trong file Excel.</p>
<h2>1. Chuẩn bị danh mục</h2><ol><li>Vào <b>Danh mục</b> để khai báo nhà xuất bản, thể loại, ngôn ngữ, quê quán, dân tộc và thủ thư.</li>
<li>Vào <b>Quản lý sách</b>, nhập đủ thông tin cần quản lý. Số lượng là số cuốn còn trên kệ; đơn giá thuê tính theo ngày.</li>
<li>Dữ liệu cũ chưa có số lượng được để 0. Cần kiểm kê và nhập tồn thực tế trước khi cho mượn. Các sách chưa có đơn giá cũng cần được bổ sung.</li>
<li>Chọn dòng để sửa, nhấn Lưu. Nhấn Thêm mới để tạo bản ghi khác. Mã tự sinh khi lưu. Không xóa được danh mục đã được hồ sơ tham chiếu.</li></ol>
<h2>2. Thẻ mượn và thủ thư</h2><p>Vào <b>Bạn đọc → Quản lý thẻ mượn</b>, nhập họ tên, ngày làm thẻ, ngày hết hạn và các thông tin liên hệ. Số sách đang mượn tự cập nhật và không sửa bằng tay.</p>
<p>Trong <b>Quản lý thủ thư</b>, tìm theo tên; chọn thủ thư rồi nhấn <b>Hồ sơ đã cho mượn</b> để xem danh sách liên quan.</p>
<h2>3. Lập hồ sơ mượn</h2><ol><li>Chọn thẻ, thủ thư, ngày mượn và tiền tạm ứng.</li><li>Đánh dấu các sách cần mượn rồi nhấn Lưu phiếu mượn.</li>
<li>Thẻ phải còn hạn, đã đến ngày làm thẻ, sách còn trong kho và có đơn giá. Trước khi mượn, thẻ phải đang giữ dưới 3 cuốn; sau khi mượn tối đa 3 cuốn.</li>
<li>Khi lưu, số lượng trong kho giảm và số sách đang mượn tăng cùng một giao dịch. Đơn giá lúc mượn được lưu lại để việc đổi giá sau đó không thay tiền của hồ sơ.</li></ol>
<h2>4. Trả từng phần và thanh toán</h2><ol><li>Chọn hồ sơ mượn; bảng chỉ hiện những sách chưa trả.</li><li>Chọn thủ thư, ngày trả, các sách trả ở lần này và vi phạm cho từng sách nếu có.</li>
<li>Thành tiền = số ngày thuê × đơn giá lúc mượn + tiền phạt. Số ngày là chênh lệch ngày trả và ngày mượn; trả cùng ngày có 0 ngày thuê. Ngày trả không được trước ngày mượn hoặc ở tương lai.</li>
<li>Trả nhiều lần: các lần trước thu phí của sách vừa trả; <b>tạm ứng chỉ trừ một lần ở lần trả cuối</b>. Số thanh toán âm là tiền hoàn lại bạn đọc.</li>
<li>Khi lưu, sách được đánh dấu đã trả, tồn kho tăng và số đang mượn của thẻ giảm. Không thể trả lại sách đã trả.</li></ol>
<h2>5. Tìm kiếm và báo cáo</h2><ul><li>Quản lý sách/Tra cứu dữ liệu: tìm theo tên sách, thể loại hoặc nhà xuất bản.</li>
<li>Báo cáo tiền thu: chọn khoảng ngày trả, xem Chi tiết hoặc Tổng hợp tháng/quý/năm. Tổng tiền thuê và phạt được trình bày riêng với thu thêm/hoàn sau tạm ứng; không cộng ba cấp tổng hợp với nhau.</li>
<li>Báo cáo sách chưa trả, hồ sơ mượn theo thẻ/thủ thư, hồ sơ trả và vi phạm: lọc theo thẻ, thủ thư; có thể bật lọc ngày.</li>
<li>Nhấp đúp hồ sơ để xem sách chi tiết. Xuất Excel (cần cài Microsoft Excel desktop) hoặc nhấn In/PDF và chọn máy in PDF khi cần. File Excel theo mẫu bài Tuần 7: thông tin đầu trang, tiêu đề đỏ căn giữa, font Times New Roman và cột STT.</li></ul>
<h2>6. Sao lưu và dữ liệu</h2><p>Hệ thống hỗ trợ sao lưu .bak và phục hồi. Trước khi phục hồi, chương trình kiểm tra bản sao trên CSDL tạm và tự sao lưu dữ liệu đang dùng.</p>
<p>Khi chạy từ dự án, Debug và Release cùng dùng <code>Database\DuLieu.mdf</code>. Không sao chép đè MDF đang chạy; dùng chức năng sao lưu/phục hồi.</p>
<p>Đề Excel không yêu cầu đăng nhập. Các menu tài khoản chưa triển khai được ẩn để chỉ hiển thị chức năng sử dụng được.</p>
</main></body></html>";
        }
    }
}
