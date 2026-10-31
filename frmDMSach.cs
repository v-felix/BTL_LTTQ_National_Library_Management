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
    public partial class frmDMSach : Form
    {
        ProcessDataBase data = new ProcessDataBase();

        public frmDMSach()
        {
            InitializeComponent();
            UiTheme.Apply(this);
            UiTheme.AddRecordCounter(this, dgvDMSach);
            dgvDMSach.CellClick += dataGridView1_CellContentClick;
            dgvDMSach.CellContentClick -= dataGridView1_CellContentClick;
        }

        private void frmDMSach_Load(object sender, EventArgs e)
        {
            dgvDMSach.AllowUserToAddRows = false;
            HienThiDuLieu();
            CapNhatGoiY();

            // Tự động gợi ý mã sách mới nếu đang trống
            if (string.IsNullOrWhiteSpace(txtMaSach.Text))
            {
                txtMaSach.Text = LayMaSachMoi().ToString();
            }
        }

        private int LayMaSachMoi()
        {
            try
            {
                DataTable dt = data.DocBang("SELECT ISNULL(MAX(MaSach), 0) + 1 AS NextID FROM tblDMSach");
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0]["NextID"] != DBNull.Value)
                {
                    return Convert.ToInt32(dt.Rows[0]["NextID"]);
                }
            }
            catch { }
            return 1;
        }

        private void CapNhatGoiY()
        {
            try
            {
                // Gợi ý Nhà xuất bản
                DataTable dtNXB = data.DocBang("SELECT DISTINCT TenNXB FROM tblNXB WHERE TenNXB IS NOT NULL AND TenNXB <> ''");
                AutoCompleteStringCollection colNXB = new AutoCompleteStringCollection();
                foreach (DataRow r in dtNXB.Rows)
                {
                    colNXB.Add(r["TenNXB"].ToString());
                }
                txtNXB.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                txtNXB.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtNXB.AutoCompleteCustomSource = colNXB;

                // Gợi ý Thể loại
                DataTable dtTL = data.DocBang("SELECT DISTINCT TenTheLoai FROM tblTheLoai WHERE TenTheLoai IS NOT NULL AND TenTheLoai <> ''");
                AutoCompleteStringCollection colTL = new AutoCompleteStringCollection();
                foreach (DataRow r in dtTL.Rows)
                {
                    colTL.Add(r["TenTheLoai"].ToString());
                }
                txtTheLoai.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                txtTheLoai.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtTheLoai.AutoCompleteCustomSource = colTL;

                // Gợi ý Ngôn ngữ
                DataTable dtNN = data.DocBang("SELECT DISTINCT TenNgonNgu FROM tblNgonNgu WHERE TenNgonNgu IS NOT NULL AND TenNgonNgu <> ''");
                AutoCompleteStringCollection colNN = new AutoCompleteStringCollection();
                foreach (DataRow r in dtNN.Rows)
                {
                    colNN.Add(r["TenNgonNgu"].ToString());
                }
                txtNgonNgu.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                txtNgonNgu.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtNgonNgu.AutoCompleteCustomSource = colNN;
            }
            catch { }
        }

        private int? LayHoacTaoMaNXB(string tenNXB)
        {
            if (string.IsNullOrWhiteSpace(tenNXB)) return null;
            tenNXB = tenNXB.Trim();

            try
            {
                DataTable dt = data.DocBang("SELECT MaNXB FROM tblNXB WHERE LOWER(TenNXB) = LOWER(N'" + tenNXB.Replace("'", "''") + "')");
                if (dt != null && dt.Rows.Count > 0)
                {
                    return Convert.ToInt32(dt.Rows[0]["MaNXB"]);
                }

                // Tự động thêm mới vào danh mục NXB nếu chưa có
                DataTable dtMax = data.DocBang("SELECT ISNULL(MAX(MaNXB), 0) + 1 AS NextID FROM tblNXB");
                int nextId = (dtMax != null && dtMax.Rows.Count > 0) ? Convert.ToInt32(dtMax.Rows[0]["NextID"]) : 1;
                data.CapNhatDuLieu($"INSERT INTO tblNXB (MaNXB, TenNXB) VALUES ({nextId}, N'{tenNXB.Replace("'", "''")}')");
                return nextId;
            }
            catch
            {
                return null;
            }
        }

        private int? LayHoacTaoMaTheLoai(string tenTheLoai)
        {
            if (string.IsNullOrWhiteSpace(tenTheLoai)) return null;
            tenTheLoai = tenTheLoai.Trim();

            try
            {
                DataTable dt = data.DocBang("SELECT MaTheLoai FROM tblTheLoai WHERE LOWER(TenTheLoai) = LOWER(N'" + tenTheLoai.Replace("'", "''") + "')");
                if (dt != null && dt.Rows.Count > 0)
                {
                    return Convert.ToInt32(dt.Rows[0]["MaTheLoai"]);
                }

                DataTable dtMax = data.DocBang("SELECT ISNULL(MAX(MaTheLoai), 0) + 1 AS NextID FROM tblTheLoai");
                int nextId = (dtMax != null && dtMax.Rows.Count > 0) ? Convert.ToInt32(dtMax.Rows[0]["NextID"]) : 1;
                data.CapNhatDuLieu($"INSERT INTO tblTheLoai (MaTheLoai, TenTheLoai) VALUES ({nextId}, N'{tenTheLoai.Replace("'", "''")}')");
                return nextId;
            }
            catch
            {
                return null;
            }
        }

        private int? LayHoacTaoMaNgonNgu(string tenNgonNgu)
        {
            if (string.IsNullOrWhiteSpace(tenNgonNgu)) return null;
            tenNgonNgu = tenNgonNgu.Trim();

            try
            {
                DataTable dt = data.DocBang("SELECT MaNgonNgu FROM tblNgonNgu WHERE LOWER(TenNgonNgu) = LOWER(N'" + tenNgonNgu.Replace("'", "''") + "')");
                if (dt != null && dt.Rows.Count > 0)
                {
                    return Convert.ToInt32(dt.Rows[0]["MaNgonNgu"]);
                }

                DataTable dtMax = data.DocBang("SELECT ISNULL(MAX(MaNgonNgu), 0) + 1 AS NextID FROM tblNgonNgu");
                int nextId = (dtMax != null && dtMax.Rows.Count > 0) ? Convert.ToInt32(dtMax.Rows[0]["NextID"]) : 1;
                data.CapNhatDuLieu($"INSERT INTO tblNgonNgu (MaNgonNgu, TenNgonNgu) VALUES ({nextId}, N'{tenNgonNgu.Replace("'", "''")}')");
                return nextId;
            }
            catch
            {
                return null;
            }
        }

        private void HienThiDuLieu()
        {
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

            DataTable dt = data.DocBang(strSQL);
            dgvDMSach.DataSource = dt;
            UiTheme.SetBookHeaders(dgvDMSach);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvDMSach.Rows.Count)
            {
                DataGridViewRow row = dgvDMSach.Rows[e.RowIndex];
                if (row.IsNewRow) return;

                txtMaSach.Text = row.Cells["MaSach"].Value != null ? row.Cells["MaSach"].Value.ToString() : "";
                txtTenSach.Text = row.Cells["TenSach"].Value != null ? row.Cells["TenSach"].Value.ToString() : "";
                txtTacGia.Text = row.Cells["TenTG"].Value != null ? row.Cells["TenTG"].Value.ToString() : "";
                txtNXB.Text = row.Cells["NhaXuatBan"].Value != null ? row.Cells["NhaXuatBan"].Value.ToString() : "";
                txtTheLoai.Text = row.Cells["TenTheLoai"].Value != null ? row.Cells["TenTheLoai"].Value.ToString() : "";
                txtNgonNgu.Text = row.Cells["TenNgonNgu"].Value != null ? row.Cells["TenNgonNgu"].Value.ToString() : "";
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            // Tự động cấp mã sách mới tiếp theo để người dùng không phải đoán số
            txtMaSach.Text = LayMaSachMoi().ToString();
            txtTenSach.Clear();
            txtTacGia.Clear();
            txtNXB.Clear();
            txtTheLoai.Clear();
            txtNgonNgu.Clear();

            txtTenSach.Focus();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string tenSach = txtTenSach.Text.Trim();
            if (string.IsNullOrEmpty(tenSach))
            {
                MessageBox.Show("Vui lòng nhập tên sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenSach.Focus();
                return;
            }

            int maSach;
            if (string.IsNullOrWhiteSpace(txtMaSach.Text))
            {
                maSach = LayMaSachMoi();
                txtMaSach.Text = maSach.ToString();
            }
            else if (!int.TryParse(txtMaSach.Text.Trim(), out maSach) || maSach <= 0)
            {
                MessageBox.Show("Mã sách phải là một số nguyên dương hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaSach.Focus();
                return;
            }

            // Xử lý mã Nhà xuất bản, Thể loại, Ngôn ngữ (tự động liên kết hoặc tạo mới an toàn)
            int? maNXB = LayHoacTaoMaNXB(txtNXB.Text);
            int? maTheLoai = LayHoacTaoMaTheLoai(txtTheLoai.Text);
            int? maNgonNgu = LayHoacTaoMaNgonNgu(txtNgonNgu.Text);

            string strNXB = maNXB.HasValue ? maNXB.Value.ToString() : "NULL";
            string strTL = maTheLoai.HasValue ? maTheLoai.Value.ToString() : "NULL";
            string strNN = maNgonNgu.HasValue ? maNgonNgu.Value.ToString() : "NULL";

            // Kiểm tra mã sách đã có trong database chưa
            DataTable dtCheck = data.DocBang($"SELECT MaSach FROM tblDMSach WHERE MaSach = {maSach}");
            bool daTonTai = (dtCheck != null && dtCheck.Rows.Count > 0);

            if (daTonTai)
            {
                DialogResult dr = MessageBox.Show(
                    $"Mã sách {maSach} đã tồn tại trong danh mục.\nBạn có muốn cập nhật thông tin cho cuốn sách này không?",
                    "Xác nhận cập nhật",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (dr != DialogResult.Yes) return;

                string strSQL = $@"
                    UPDATE tblDMSach
                    SET 
                        TenSach = N'{tenSach.Replace("'", "''")}',
                        TenTG = N'{txtTacGia.Text.Trim().Replace("'", "''")}',
                        MaNXB = {strNXB},
                        MaTheLoai = {strTL},
                        MaNgonNgu = {strNN}
                    WHERE MaSach = {maSach}";

                if (data.CapNhatDuLieu(strSQL))
                {
                    MessageBox.Show("Cập nhật sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    HienThiDuLieu();
                    CapNhatGoiY();
                }
            }
            else
            {
                string strSQL = $@"
                    INSERT INTO tblDMSach (MaSach, TenSach, TenTG, MaNXB, MaTheLoai, MaNgonNgu)
                    VALUES (
                        {maSach},
                        N'{tenSach.Replace("'", "''")}',
                        N'{txtTacGia.Text.Trim().Replace("'", "''")}',
                        {strNXB},
                        {strTL},
                        {strNN}
                    )";

                if (data.CapNhatDuLieu(strSQL))
                {
                    MessageBox.Show("Thêm sách mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    HienThiDuLieu();
                    CapNhatGoiY();

                    // Chuẩn bị sẵn mã tiếp theo cho lần thêm kế tiếp
                    txtMaSach.Text = LayMaSachMoi().ToString();
                    txtTenSach.Clear();
                    txtTacGia.Clear();
                    txtNXB.Clear();
                    txtTheLoai.Clear();
                    txtNgonNgu.Clear();
                    txtTenSach.Focus();
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSach.Text) || !int.TryParse(txtMaSach.Text.Trim(), out int maSach) || maSach <= 0)
            {
                MessageBox.Show("Vui lòng chọn hoặc nhập mã sách cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tenSach = txtTenSach.Text.Trim();
            if (string.IsNullOrEmpty(tenSach))
            {
                MessageBox.Show("Vui lòng nhập tên sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenSach.Focus();
                return;
            }

            int? maNXB = LayHoacTaoMaNXB(txtNXB.Text);
            int? maTheLoai = LayHoacTaoMaTheLoai(txtTheLoai.Text);
            int? maNgonNgu = LayHoacTaoMaNgonNgu(txtNgonNgu.Text);

            string strNXB = maNXB.HasValue ? maNXB.Value.ToString() : "NULL";
            string strTL = maTheLoai.HasValue ? maTheLoai.Value.ToString() : "NULL";
            string strNN = maNgonNgu.HasValue ? maNgonNgu.Value.ToString() : "NULL";

            string strSQL = $@"
                UPDATE tblDMSach
                SET 
                    TenSach = N'{tenSach.Replace("'", "''")}',
                    TenTG = N'{txtTacGia.Text.Trim().Replace("'", "''")}',
                    MaNXB = {strNXB},
                    MaTheLoai = {strTL},
                    MaNgonNgu = {strNN}
                WHERE MaSach = {maSach}";

            if (data.CapNhatDuLieu(strSQL))
            {
                MessageBox.Show("Cập nhật sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                HienThiDuLieu();
                CapNhatGoiY();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSach.Text) || !int.TryParse(txtMaSach.Text.Trim(), out int maSach) || maSach <= 0)
            {
                MessageBox.Show("Vui lòng chọn sách cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa sách '{txtTenSach.Text}' (Mã {maSach}) không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                string strSQL = $"DELETE FROM tblDMSach WHERE MaSach = {maSach}";

                if (data.CapNhatDuLieu(strSQL))
                {
                    MessageBox.Show("Xóa sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    HienThiDuLieu();
                    CapNhatGoiY();

                    txtMaSach.Text = LayMaSachMoi().ToString();
                    txtTenSach.Clear();
                    txtTacGia.Clear();
                    txtNXB.Clear();
                    txtTheLoai.Clear();
                    txtNgonNgu.Clear();
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaSach.Text = LayMaSachMoi().ToString();
            txtTenSach.Clear();
            txtTacGia.Clear();
            txtNXB.Clear();
            txtTheLoai.Clear();
            txtNgonNgu.Clear();

            HienThiDuLieu();
            CapNhatGoiY();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
