using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace QuanLyThuVienQuocGia
{
    /// <summary>
    /// Tạo báo cáo chuẩn A4 giống 100% form mẫu "Báo cáo Danh sách Sách.pdf"
    /// Hỗ trợ nạp dữ liệu động từ CSDL hoặc nạp dữ liệu mẫu, có thể in ấn và xuất PDF.
    /// </summary>
    public static class BaoCaoChuanHelper
    {
        public static string SinhHtmlBaoCao(DataTable dtSach, bool laDuLieuMau = false)
        {
            DateTime now = DateTime.Now;
            string thoiGianTrichXuat = now.ToString("HH:mm") + ", Ngày " + now.ToString("dd/MM/yyyy");
            string ngayThangNam = string.Format("Hà Nội, ngày {0:00} tháng {1:00} năm {2}", now.Day, now.Month, now.Year);
            int tongSo = dtSach != null ? dtSach.Rows.Count : 0;
            string tongSoStr = tongSo.ToString("D2");

            StringBuilder sbRows = new StringBuilder();
            if (dtSach != null && dtSach.Rows.Count > 0)
            {
                int stt = 1;
                foreach (DataRow r in dtSach.Rows)
                {
                    string ma = r.Table.Columns.Contains("MaSach") ? r["MaSach"].ToString() : stt.ToString();
                    string ten = r.Table.Columns.Contains("TenSach") ? r["TenSach"].ToString() : "";
                    string tg = r.Table.Columns.Contains("TenTG") ? r["TenTG"].ToString() : "";
                    string nxb = r.Table.Columns.Contains("NhaXuatBan") ? r["NhaXuatBan"].ToString()
                               : (r.Table.Columns.Contains("TenNXB") ? r["TenNXB"].ToString() : "");
                    string tl = r.Table.Columns.Contains("TenTheLoai") ? r["TenTheLoai"].ToString() : "";
                    string nn = r.Table.Columns.Contains("TenNgonNgu") ? r["TenNgonNgu"].ToString() : "";

                    sbRows.AppendLine("    <tr>");
                    sbRows.AppendLine(string.Format("      <td class=\"text-center\">{0}</td>", stt));
                    sbRows.AppendLine(string.Format("      <td class=\"text-center\">{0}</td>", ma));
                    sbRows.AppendLine(string.Format("      <td class=\"text-left\">{0}</td>", ten));
                    sbRows.AppendLine(string.Format("      <td class=\"text-left\">{0}</td>", tg));
                    sbRows.AppendLine(string.Format("      <td class=\"text-left\">{0}</td>", nxb));
                    sbRows.AppendLine(string.Format("      <td class=\"text-center\">{0}</td>", tl));
                    sbRows.AppendLine(string.Format("      <td class=\"text-center\">{0}</td>", nn));
                    sbRows.AppendLine("    </tr>");
                    stt++;
                }
            }
            else
            {
                sbRows.AppendLine("    <tr><td colspan=\"7\" class=\"text-center\" style=\"padding: 20px;\">Không có dữ liệu sách</td></tr>");
            }

            string html = @"<!DOCTYPE html>
<html>
<head>
<meta charset=""utf-8"">
<meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
<title>Báo cáo Danh sách Sách</title>
<style>
  @page {
    size: A4 portrait;
    margin: 15mm 15mm 15mm 15mm;
  }
  @media print {
    body { padding: 0 !important; background: #fff !important; }
    .no-print { display: none !important; }
    .page-sheet { box-shadow: none !important; margin: 0 !important; border: none !important; }
  }
  * { box-sizing: border-box; }
  body {
    font-family: 'Times New Roman', Times, serif;
    font-size: 13pt;
    line-height: 1.35;
    color: #000;
    margin: 0;
    padding: 20px;
    background: #eef2f7;
  }
  .page-sheet {
    background: #ffffff;
    max-width: 210mm;
    min-height: 297mm;
    margin: 0 auto;
    padding: 20mm 18mm 18mm 18mm;
    box-shadow: 0 4px 15px rgba(0,0,0,0.12);
    position: relative;
  }
  .header-table {
    width: 100%;
    border-collapse: collapse;
    margin-bottom: 25px;
  }
  .header-table td {
    vertical-align: top;
    border: none;
    padding: 0;
  }
  .org-name {
    text-align: center;
    font-size: 12pt;
    font-weight: bold;
    text-transform: uppercase;
  }
  .org-sub {
    text-align: center;
    font-size: 12pt;
    font-weight: bold;
    text-transform: uppercase;
    margin-top: 3px;
  }
  .nation-name {
    text-align: center;
    font-size: 12.5pt;
    font-weight: bold;
    text-transform: uppercase;
  }
  .motto {
    text-align: center;
    font-size: 12pt;
    font-weight: bold;
    margin-top: 3px;
    border-bottom: 1.5px solid #000;
    display: inline-block;
    padding-bottom: 3px;
  }
  .title-block {
    text-align: center;
    margin: 30px 0 20px 0;
  }
  .report-title {
    font-size: 17pt;
    font-weight: bold;
    text-transform: uppercase;
    letter-spacing: 0.5px;
    margin-bottom: 6px;
  }
  .report-meta {
    font-size: 11pt;
    font-style: italic;
    color: #222;
  }
  .data-table {
    width: 100%;
    border-collapse: collapse;
    margin-top: 15px;
    margin-bottom: 15px;
  }
  .data-table th, .data-table td {
    border: 1px solid #000;
    padding: 7px 8px;
    font-size: 11.5pt;
  }
  .data-table th {
    font-weight: bold;
    text-align: center;
    background-color: #fafafa;
  }
  .text-center { text-align: center; }
  .text-left { text-align: left; }
  .text-right { text-align: right; }
  .summary-block {
    margin-top: 12px;
    font-size: 12pt;
  }
  .summary-count {
    font-weight: bold;
  }
  .summary-note {
    font-style: italic;
    margin-top: 4px;
    color: #333;
  }
  .signature-date {
    text-align: right;
    font-style: italic;
    font-size: 12pt;
    margin-top: 25px;
    margin-bottom: 10px;
    padding-right: 30px;
  }
  .signature-table {
    width: 100%;
    border-collapse: collapse;
    margin-top: 5px;
  }
  .signature-table td {
    text-align: center;
    vertical-align: top;
    border: none;
    width: 33.33%;
    padding: 0 4px;
    font-size: 12pt;
  }
  .sig-title {
    font-weight: bold;
  }
  .sig-sub {
    font-style: italic;
    font-size: 10pt;
    color: #333;
    margin-top: 3px;
  }
  .sig-space {
    height: 80px;
  }
  .footer-bar {
    margin-top: 40px;
    padding-top: 8px;
    border-top: 1px solid #bbb;
    font-size: 10pt;
    color: #555;
    display: table;
    width: 100%;
  }
  .footer-left { display: table-cell; text-align: left; }
  .footer-right { display: table-cell; text-align: right; }
</style>
</head>
<body>
<div class=""page-sheet"">
  <table class=""header-table"">
    <tr>
      <td style=""width: 48%;"">
        <div class=""org-name"">THƯ VIỆN QUỐC GIA VIỆT NAM</div>
        <div class=""org-sub"">BỘ PHẬN QUẢN LÝ TÀI LIỆU</div>
      </td>
      <td style=""width: 52%; text-align: center;"">
        <div class=""nation-name"">CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM</div>
        <div><span class=""motto"">Độc lập - Tự do - Hạnh phúc</span></div>
      </td>
    </tr>
  </table>

  <div class=""title-block"">
    <div class=""report-title"">BÁO CÁO DANH SÁCH SÁCH HIỆN CÓ</div>
    <div class=""report-meta"">Thời gian trích xuất: " + thoiGianTrichXuat + @"</div>
    <div class=""report-meta"">Người xuất: Quản trị viên</div>
  </div>

  <table class=""data-table"">
    <thead>
      <tr>
        <th style=""width: 45px;"">STT</th>
        <th style=""width: 65px;"">Mã sách</th>
        <th>Tên sách</th>
        <th>Tác giả</th>
        <th>Nhà xuất bản</th>
        <th style=""width: 95px;"">Thể loại</th>
        <th style=""width: 95px;"">Ngôn ngữ</th>
      </tr>
    </thead>
    <tbody>
" + sbRows.ToString() + @"    </tbody>
  </table>

  <div class=""summary-block"">
    <div class=""summary-count"">Tổng số lượng đầu sách trong danh sách: " + tongSoStr + @" cuốn.</div>
    <div class=""summary-note"">Ghi chú: Số liệu được tổng hợp tính đến thời điểm xuất báo cáo.</div>
  </div>

  <div class=""signature-date"">" + ngayThangNam + @"</div>

  <table class=""signature-table"">
    <tr>
      <td>
        <div class=""sig-title"">Người lập biểu</div>
        <div class=""sig-sub"">(Ký, ghi rõ họ tên)</div>
        <div class=""sig-space""></div>
      </td>
      <td>
        <div class=""sig-title"">Trưởng bộ phận</div>
        <div class=""sig-sub"">(Ký, ghi rõ họ tên)</div>
        <div class=""sig-space""></div>
      </td>
      <td>
        <div class=""sig-title"">Giám đốc Thư viện</div>
        <div class=""sig-sub"">(Ký, đóng dấu, ghi rõ họ tên)</div>
        <div class=""sig-space""></div>
      </td>
    </tr>
  </table>

  <div class=""footer-bar"">
    <span class=""footer-left"">Hệ thống Quản lý Thư viện Quốc gia</span>
    <span class=""footer-right"">Trang 1/1</span>
  </div>
</div>
</body>
</html>";
            return html;
        }

        public static DataTable TaoDataTableMau()
        {
            DataTable dt = new DataTable("tblDMSach");
            dt.Columns.Add("MaSach", typeof(int));
            dt.Columns.Add("TenSach", typeof(string));
            dt.Columns.Add("TenTG", typeof(string));
            dt.Columns.Add("NhaXuatBan", typeof(string));
            dt.Columns.Add("TenTheLoai", typeof(string));
            dt.Columns.Add("TenNgonNgu", typeof(string));

            dt.Rows.Add(1, "Lập trình C++ căn bản", "Nguyễn Văn A", "NXB Giáo dục", "Tin học", "Tiếng Việt");
            dt.Rows.Add(2, "Cơ sở dữ liệu nâng cao", "Trần Văn B", "NXB Giáo dục", "Giáo trình", "Tiếng Việt");
            dt.Rows.Add(3, "Dế Mèn phiêu lưu ký", "Tô Hoài", "NXB Kim Đồng", "Văn học", "Tiếng Việt");
            dt.Rows.Add(4, "Introduction to Programming", "John Smith", "NXB Trẻ", "Tin học", "Tiếng Anh");
            dt.Rows.Add(5, "Lão Hạc và truyện ngắn chọn lọc", "Nam Cao", "NXB Văn học", "Văn học", "Tiếng Việt");
            dt.Rows.Add(6, "Thiết kế thuật toán", "Lê Văn C", "NXB Khoa học", "Tin học", "Tiếng Việt");
            dt.Rows.Add(7, "Đắc Nhân Tâm", "Dale Carnegie", "NXB Trẻ", "Kỹ năng sống", "Tiếng Việt");
            dt.Rows.Add(8, "Data Structures and Algorithms", "Robert Lafore", "NXB Giáo dục", "Tin học", "Tiếng Anh");
            dt.Rows.Add(9, "Truyện Kiều", "Nguyễn Du", "NXB Văn học", "Văn học", "Tiếng Việt");
            dt.Rows.Add(10, "Lập trình Web với ASP.NET", "Phạm Văn D", "NXB Thống kê", "Tin học", "Tiếng Việt");
            dt.Rows.Add(11, "Vợ nhặt", "Kim Lân", "NXB Văn học", "Văn học", "Tiếng Việt");
            dt.Rows.Add(12, "Clean Code - Nghệ thuật viết code sạch", "Robert C. Martin", "NXB Trẻ", "Tin học", "Tiếng Việt");

            return dt;
        }

        public static string LuuHtmlTam(string html)
        {
            try
            {
                string dir = Path.Combine(Path.GetTempPath(), "QuanLyThuVienQuocGia");
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                string file = Path.Combine(dir, "BaoCaoHienTai.html");
                File.WriteAllText(file, html, Encoding.UTF8);
                return file;
            }
            catch
            {
                string fallback = Path.GetTempFileName() + ".html";
                File.WriteAllText(fallback, html, Encoding.UTF8);
                return fallback;
            }
        }
    }
}
