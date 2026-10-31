using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace QuanLyThuVienQuocGia
{
    public partial class frmBaoCaoSach : Form
    {
        private ProcessDataBase data = new ProcessDataBase();
        private rptDanhSachSach currentReport = null;
        private DataTable dtHienTai = null;
        private string duongDanHtmlHienTai = "";

        public frmBaoCaoSach()
        {
            InitializeComponent();
            btnThoat.CausesValidation = false;
            UiTheme.Apply(this);
        }

        private void frmBaoCaoSach_Load(object sender, EventArgs e)
        {
            // Tự động tải dữ liệu thực tế từ cơ sở dữ liệu và hiển thị ngay trên Crystal Reports
            TaiDuLieuBaoCao();
        }

        /// <summary>Nút Tải lại từ CSDL</summary>
        private void btnIn_Click(object sender, EventArgs e)
        {
            TaiDuLieuBaoCao();
        }

        /// <summary>Đọc dữ liệu thực tế từ CSDL DuLieu.mdf và hiển thị vào Crystal Reports</summary>
        private void TaiDuLieuBaoCao()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                string strSQL = @"
                    SELECT 
                        S.MaSach,
                        S.TenSach,
                        ISNULL(S.TenTG, N'') AS TenTG,
                        ISNULL(NXB.TenNXB, N'') AS NhaXuatBan,
                        ISNULL(TL.TenTheLoai, N'') AS TenTheLoai,
                        ISNULL(NN.TenNgonNgu, N'') AS TenNgonNgu
                    FROM tblDMSach S
                    LEFT JOIN tblNXB NXB ON S.MaNXB = NXB.MaNXB
                    LEFT JOIN tblTheLoai TL ON S.MaTheLoai = TL.MaTheLoai
                    LEFT JOIN tblNgonNgu NN ON S.MaNgonNgu = NN.MaNgonNgu
                    WHERE S.MaSach > 0
                    ORDER BY S.MaSach";

                dtHienTai = data.DocBang(strSQL);

                if (dtHienTai == null)
                {
                    dtHienTai = new DataTable();
                }

                NapCrystalReport(dtHienTai);
                CapNhatHtmlPreview(dtHienTai);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đọc CSDL: " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Nạp dữ liệu vào Crystal Reports và tự động căn chỉnh toàn bộ layout:
        /// Tiêu đề nhỏ gọn, căn giữa thẳng hàng, các cột sắp xếp ngay ngắn không bị lệch và không bị cắt cột Ngôn ngữ.
        /// </summary>
        private void NapCrystalReport(DataTable dt)
        {
            DonDepReport();

            dsDanhSachSach ds = new dsDanhSachSach();
            ds.EnforceConstraints = false;

            int stt = 1;
            foreach (DataRow r in dt.Rows)
            {
                int ma = Convert.ToInt32(r["MaSach"]);
                string ten = r["TenSach"].ToString();
                string tg = r["TenTG"].ToString();
                string nxbStr = r.Table.Columns.Contains("NhaXuatBan") ? r["NhaXuatBan"].ToString() : (r.Table.Columns.Contains("TenNXB") ? r["TenNXB"].ToString() : "");
                string tlStr = r["TenTheLoai"].ToString();
                string nnStr = r["TenNgonNgu"].ToString();

                ds.tblNgonNgu.AddtblNgonNguRow(stt, nnStr);
                ds.tblTheLoai.AddtblTheLoaiRow(stt, tlStr);
                ds.tblNXB.AddtblNXBRow(stt, nxbStr, "", "", "");

                dsDanhSachSach.tblNXBRow nxbRow = ds.tblNXB.FindByMaNXB(stt);
                dsDanhSachSach.tblTheLoaiRow tlRow = ds.tblTheLoai.FindByMaTheLoai(stt);
                dsDanhSachSach.tblNgonNguRow nnRow = ds.tblNgonNgu.FindByMaNgonNgu(stt);

                ds.tblDMSach.AddtblDMSachRow(ma, ten, tg, nxbRow, stt, "16x24", 50000m, 1, tlRow, nnRow, 200, "", 3000m);
                stt++;
            }

            currentReport = new rptDanhSachSach();
            currentReport.SetDataSource(ds);
            currentReport.PrintOptions.PaperOrientation = PaperOrientation.Portrait;

            // Định dạng Crystal Reports: Tiêu đề nhỏ gọn, căn giữa chuẩn chỉ, căn lề 6 cột ngay ngắn
            DinhDangCrystalReport(currentReport, dt.Rows.Count);

            rptvDanhSachSach.ReportSource = currentReport;
            rptvDanhSachSach.Visible = true;
            wbBaoCao.Visible = false;
            rptvDanhSachSach.BringToFront();
            btnDoiCheDo.Text = "XEM DẠNG WEB A4";
        }

        private void DinhDangCrystalReport(rptDanhSachSach rpt, int tongSoSach)
        {
            try
            {
                DateTime now = DateTime.Now;
                string thoiGian = now.ToString("dd/MM/yyyy HH:mm");

                // 1. Tiêu đề báo cáo (Section 1): Nhỏ gọn, căn giữa thẳng hàng, màu sắc nhã nhặn
                Section sec1 = rpt.ReportDefinition.Sections[0];
                sec1.Height = 1100;

                TextObject t7 = sec1.ReportObjects["Text7"] as TextObject;
                if (t7 != null)
                {
                    t7.Left = 0;
                    t7.Top = 150;
                    t7.Width = 10450;
                    t7.Height = 850;
                    t7.ObjectFormat.HorizontalAlignment = Alignment.HorizontalCenterAlign;
                    t7.ApplyFont(new Font("Times New Roman", 13.5F, FontStyle.Bold));
                    t7.Color = Color.FromArgb(0, 45, 95);

                    t7.Text = "THƯ VIỆN QUỐC GIA VIỆT NAM\r\n" +
                              "BÁO CÁO DANH SÁCH SÁCH\r\n" +
                              "(Thời gian: " + thoiGian + " - Tổng số: " + tongSoSach + " cuốn)";
                }

                // 2. Định dạng Cột (Section 2 - Header và Section 3 - Details): Căn chỉnh đúng hàng lối, khít khổ A4
                Section sec2 = rpt.ReportDefinition.Sections[1];
                Section sec3 = rpt.ReportDefinition.Sections[2];

                int[] lefts = { 200, 1150, 3900, 5850, 7800, 9150 };
                int[] widths = { 950, 2750, 1950, 1950, 1350, 1300 };
                string[] headNames = { "Text1", "Text2", "Text3", "Text4", "Text5", "Text6" };
                string[] headTexts = { "Mã sách", "Tên sách", "Tác giả", "Nhà xuất bản", "Thể loại", "Ngôn ngữ" };
                string[] fieldNames = { "MaSach1", "TenSach1", "TenTG1", "TenNXB1", "TenTheLoai1", "TenNgonNgu1" };

                Font headFont = new Font("Times New Roman", 10.5F, FontStyle.Bold);
                Font dataFont = new Font("Times New Roman", 10F, FontStyle.Regular);

                for (int i = 0; i < 6; i++)
                {
                    FieldHeadingObject h = sec2.ReportObjects[headNames[i]] as FieldHeadingObject;
                    if (h != null)
                    {
                        h.Left = lefts[i];
                        h.Width = widths[i];
                        h.Text = headTexts[i];
                        h.ApplyFont(headFont);
                        h.Color = Color.FromArgb(0, 45, 95);
                    }

                    FieldObject f = sec3.ReportObjects[fieldNames[i]] as FieldObject;
                    if (f != null)
                    {
                        f.Left = lefts[i];
                        f.Width = widths[i];
                        f.ApplyFont(dataFont);
                    }
                }
            }
            catch { }
        }

        private void CapNhatHtmlPreview(DataTable dt)
        {
            string html = BaoCaoChuanHelper.SinhHtmlBaoCao(dt, laDuLieuMau: false);
            duongDanHtmlHienTai = BaoCaoChuanHelper.LuuHtmlTam(html);
            if (File.Exists(duongDanHtmlHienTai))
            {
                wbBaoCao.Navigate(duongDanHtmlHienTai);
            }
            else
            {
                wbBaoCao.DocumentText = html;
            }
        }

        /// <summary>Chuyển đổi linh hoạt giữa giao diện Crystal Reports và Web A4</summary>
        private void btnDoiCheDo_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                if (rptvDanhSachSach.Visible)
                {
                    wbBaoCao.Visible = true;
                    rptvDanhSachSach.Visible = false;
                    wbBaoCao.BringToFront();
                    btnDoiCheDo.Text = "XEM QUA CRYSTAL";
                }
                else
                {
                    if (currentReport == null && dtHienTai != null)
                    {
                        NapCrystalReport(dtHienTai);
                    }
                    wbBaoCao.Visible = false;
                    rptvDanhSachSach.Visible = true;
                    rptvDanhSachSach.BringToFront();
                    btnDoiCheDo.Text = "XEM DẠNG WEB A4";
                }
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        /// <summary>In báo cáo</summary>
        private void btnInAn_Click(object sender, EventArgs e)
        {
            try
            {
                if (rptvDanhSachSach.Visible)
                {
                    rptvDanhSachSach.PrintReport();
                }
                else
                {
                    wbBaoCao.ShowPrintPreviewDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể mở hộp thoại in: " + ex.Message, "Lỗi in", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>Xuất PDF từ báo cáo thực tế hiện tại</summary>
        private void btnMoPdf_Click(object sender, EventArgs e)
        {
            try
            {
                string thuMuc = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");
                if (!Directory.Exists(thuMuc)) Directory.CreateDirectory(thuMuc);
                string outPdf = Path.Combine(thuMuc, "BaoCaoDanhSachSach_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf");

                if (currentReport == null && dtHienTai != null)
                {
                    NapCrystalReport(dtHienTai);
                }

                if (currentReport != null)
                {
                    currentReport.ExportToDisk(ExportFormatType.PortableDocFormat, outPdf);
                    if (File.Exists(outPdf))
                    {
                        Process.Start(new ProcessStartInfo(outPdf) { UseShellExecute = true });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể mở file PDF: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmBaoCaoSach_FormClosing(object sender, FormClosingEventArgs e)
        {
            DonDepReport();
        }

        private void DonDepReport()
        {
            try
            {
                if (rptvDanhSachSach != null)
                {
                    rptvDanhSachSach.ReportSource = null;
                }
                if (currentReport != null)
                {
                    currentReport.Close();
                    currentReport.Dispose();
                    currentReport = null;
                }
            }
            catch { }
        }
    }
}
