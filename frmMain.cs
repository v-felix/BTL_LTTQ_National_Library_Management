using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyThuVienQuocGia
{
    public partial class frmMain : Form
    {
        private Label lblTieuDe;
        private Label lblPhuDe;
        private Timer tmrDongHo;

        public frmMain()
        {
            InitializeComponent();
            UiTheme.Apply(this);
            TaoTieuDe();
            GanChucNang();

            // Đồng hồ ở thanh trạng thái
            tmrDongHo = new Timer { Interval = 1000 };
            tmrDongHo.Tick += delegate { CapNhatThoiGian(); };
            tmrDongHo.Start();
            CapNhatThoiGian();
        }

        private void TaoTieuDe()
        {
            lblTieuDe = new Label
            {
                Text = "QUẢN LÝ THƯ VIỆN QUỐC GIA",
                AutoSize = true,
                BackColor = Color.Transparent,
                ForeColor = Color.FromArgb(0, 70, 140),
                Font = UiTheme.F(30F, FontStyle.Bold)
            };
            lblPhuDe = new Label
            {
                Text = "Hệ thống quản lý sách, bạn đọc và mượn trả",
                AutoSize = true,
                BackColor = Color.Transparent,
                ForeColor = UiTheme.Navy,
                Font = UiTheme.F(14F)
            };
            Controls.Add(lblTieuDe);
            Controls.Add(lblPhuDe);
            lblTieuDe.BringToFront();
            lblPhuDe.BringToFront();
            Resize += delegate { CanGiuaTieuDe(); };
            Load += delegate { CanGiuaTieuDe(); };
            CanGiuaTieuDe();
        }

        private void CanGiuaTieuDe()
        {
            lblTieuDe.Left = (ClientSize.Width - lblTieuDe.Width) / 2;
            lblTieuDe.Top = menuStripMain.Height + 40;
            lblPhuDe.Left = (ClientSize.Width - lblPhuDe.Width) / 2;
            lblPhuDe.Top = lblTieuDe.Bottom + 6;
        }

        private void CapNhatThoiGian()
        {
            lblThoiGian.Text = DateTime.Now.ToString("dd/MM/yyyy  HH:mm:ss");
        }

        private void GanChucNang()
        {
            // Authentication is outside the supplied library specification.
            mnuDangNhap.Visible = mnuDoiMatKhau.Visible = mnuQuanLyTaiKhoan.Visible = false;
            mnuSaoLuu.Click += delegate { LibraryUi.Run(DatabaseMaintenance.BackupDialog); };
            mnuKhoiPhuc.Click += delegate { LibraryUi.Run(DatabaseMaintenance.RestoreDialog); };
            mnuTheLoai.Click += delegate { MoDanhMuc("theloai"); };
            mnuNXB.Click += delegate { MoDanhMuc("nxb"); };
            mnuNgonNgu.Click += delegate { MoDanhMuc("ngonngu"); };
            mnuDanToc.Click += delegate { MoDanhMuc("dantoc"); };
            mnuQue.Click += delegate { MoDanhMuc("que"); };
            mnuTheMuon.Click += delegate { MoDanhMuc("the"); };
            mnuViPham.Click += delegate { MoDanhMuc("vipham"); };
            mnuViPham.Text = "Danh mục vi phạm / tiền phạt";
            var thuThu = new ToolStripMenuItem("Quản lý thủ thư");
            thuThu.Click += delegate { MoDanhMuc("thuthu"); };
            qUẢNLÝDANHMỤCToolStripMenuItem.DropDownItems.Add(thuThu);
            mnuLapPhieuMuon.Click += delegate { LibraryUi.Run(() => { using(var f = new frmCirculation(false)) f.ShowDialog(this); }); };
            mnuLapPhieuTra.Click += delegate { LibraryUi.Run(() => { using(var f = new frmCirculation(true)) f.ShowDialog(this); }); };
            mnuChiTietMuon.Click += delegate { MoBaoCao("loans"); };
            mnuThongKeMuon.Text = "Hồ sơ mượn theo thẻ / thủ thư";
            mnuThongKeMuon.Click += delegate { MoBaoCao("loans"); };
            mnuThongKeTra.Text = "Hồ sơ trả và thanh toán";
            mnuThongKeTra.Click += delegate { MoBaoCao("returns"); };
            mnuThongKeChuaTra.Click += delegate { MoBaoCao("outstanding"); };
            mnuThongKeViPham.Click += delegate { MoBaoCao("violations"); };
            var doanhThu = new ToolStripMenuItem("Tiền thu theo tháng / quý / năm");
            doanhThu.Click += delegate { MoBaoCao("revenue"); };
            mnuBaoCao.DropDownItems.Add(doanhThu);
            lblTrangThai.Text = "Sẵn sàng • Dữ liệu thư viện dùng chung Debug / Release";

            mnuHuongDan.Click += delegate
            {
                frmHuongDan frm = new frmHuongDan();
                frm.ShowDialog();
            };
            mnuThongTin.Click += delegate
            {
                MessageBox.Show("Quản lý thư viện quốc gia\nC# Windows Forms - .NET Framework 4.7.2",
                    "Thông tin chương trình", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            mnuThoat.Click += delegate
            {
                if (MessageBox.Show("Bạn có chắc chắn muốn thoát chương trình?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    Close();
            };
        }

        private void MoDanhMuc(string key)
        { LibraryUi.Run(() => { using(var f = new frmCatalog(key)) f.ShowDialog(this); }); }
        private void MoBaoCao(string key)
        { LibraryUi.Run(() => { using(var f = new frmReports(key)) f.ShowDialog(this); }); }

        private void mnuSach_Click(object sender, EventArgs e)
        {
            MoDanhMuc("sach");
        }

        private void mnuTraCuuDuLieu_Click(object sender, EventArgs e)
        {
            MoDanhMuc("sach");
        }

        private void mnuThongKeSach_Click(object sender, EventArgs e)
        {
            frmBaoCaoSach frm = new frmBaoCaoSach();
            frm.ShowDialog();
        }
    }
}
