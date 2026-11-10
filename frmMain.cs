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

        public frmMain()
        {
            InitializeComponent();
            UiTheme.Apply(this);
            // Keep the same title layout as the drag-and-drop Designer.
            lblTieuDe.AutoSize=false;lblTieuDe.Font=UiTheme.F(30F,FontStyle.Bold);lblTieuDe.Height=60;
            lblPhuDe.AutoSize=false;lblPhuDe.Font=UiTheme.F(14F);lblPhuDe.Height=36;
            GanChucNang();

            tmrDongHo.Tick += delegate { CapNhatThoiGian(); };
            if(LicenseManager.UsageMode!=LicenseUsageMode.Designtime && !DesignMode)tmrDongHo.Start();
            CapNhatThoiGian();
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
            mnuThuThu.Click += delegate { MoDanhMuc("thuthu"); };
            mnuLapPhieuMuon.Click += delegate { LibraryUi.Run(() => { using(var f = new frmCirculation(false)) f.ShowDialog(this); }); };
            mnuLapPhieuTra.Click += delegate { LibraryUi.Run(() => { using(var f = new frmCirculation(true)) f.ShowDialog(this); }); };
            mnuChiTietMuon.Click += delegate { MoBaoCao("loans"); };
            mnuThongKeMuon.Text = "Hồ sơ mượn theo thẻ / thủ thư";
            mnuThongKeMuon.Click += delegate { MoBaoCao("loans"); };
            mnuThongKeTra.Text = "Hồ sơ trả và thanh toán";
            mnuThongKeTra.Click += delegate { MoBaoCao("returns"); };
            mnuThongKeChuaTra.Click += delegate { MoBaoCao("outstanding"); };
            mnuThongKeViPham.Click += delegate { MoBaoCao("violations"); };
            mnuTienThu.Click += delegate { MoBaoCao("revenue"); };
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
        { LibraryUi.Run(() => { using(Form f = key=="sach" ? (Form)new frmDMSach() : new frmCatalog(key)) f.ShowDialog(this); }); }
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
