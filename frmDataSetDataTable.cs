using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OfficeOpenXml;
using System.IO;

namespace QuanLyThuVienQuocGia
{
    public partial class frmDataSetDataTable : Form
    {
        // ứng dụng DataSet và DataTable để đọc dữ liệu từ cơ sở dữ liệu
        private DataSet ds;
        private DataTable dt;
        private ProcessDataBase data = new ProcessDataBase();
        public frmDataSetDataTable()
        {
            InitializeComponent();
            UiTheme.Apply(this);
            UiTheme.AddRecordCounter(this, dgvDuLieu);
        }

        private void frmDataSetDataTable_Load(object sender, EventArgs e)
        {
            DocDuLieu();
        }
        private void DocDuLieu()
        {
            string strSQL = @"
        SELECT 
            S.MaSach,
            S.TenSach,
            S.TenTG,
            NXB.TenNXB AS NhaXuatBan,
            TL.TenTheLoai,
            NN.TenNgonNgu
        FROM tblDMSach S
        INNER JOIN tblNXB NXB 
            ON S.MaNXB = NXB.MaNXB
        INNER JOIN tblTheLoai TL 
            ON S.MaTheLoai = TL.MaTheLoai
        INNER JOIN tblNgonNgu NN 
            ON S.MaNgonNgu = NN.MaNgonNgu";

            ds = data.DocDataSet(strSQL);

            dt = ds.Tables[0];

            dgvDuLieu.DataSource = dt;

            UiTheme.SetBookHeaders(dgvDuLieu);

            dgvDuLieu.AllowUserToAddRows = false;

            dgvDuLieu.ReadOnly = true;

            dgvDuLieu.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvDuLieu.MultiSelect = false;
        }

        private void btnDocDuLieu_Click(object sender, EventArgs e)
        {
            DocDuLieu();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            DocDuLieu();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            DataRow row = dt.NewRow();

            row["MaSach"] = 9999;
            row["TenSach"] = "Sách thử DataTable";
            row["TenTG"] = "Nguyễn Văn A";
            row["NhaXuatBan"] = "Nhà xuất bản Giáo dục";
            row["TenTheLoai"] = "Công nghệ thông tin";
            row["TenNgonNgu"] = "Tiếng Việt";

            dt.Rows.Add(row);
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvDuLieu.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn dòng cần sửa!");
                return;
            }

            DataRow row =
                ((DataRowView)dgvDuLieu.CurrentRow.DataBoundItem).Row;

            string tenSachMoi = Microsoft.VisualBasic.Interaction.InputBox(
                "Nhập tên sách mới:",
                "Sửa tên sách",
                row["TenSach"].ToString());

            if (tenSachMoi == "")
                return;

            string tacGiaMoi = Microsoft.VisualBasic.Interaction.InputBox(
                "Nhập tên tác giả mới:",
                "Sửa tác giả",
                row["TenTG"].ToString());

            if (tacGiaMoi == "")
                return;

            row["TenSach"] = tenSachMoi;
            row["TenTG"] = tacGiaMoi;

            MessageBox.Show("Sửa dữ liệu thành công!");
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvDuLieu.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn dòng cần xóa!");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa dòng này không?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                DataRow row =
                    ((DataRowView)dgvDuLieu.CurrentRow.DataBoundItem).Row;

                row.Delete();
            }
        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!");
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel Files|*.xlsx";
            saveFileDialog.Title = "Lưu dữ liệu Excel";
            saveFileDialog.FileName = "DanhSachSach.xlsx";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ExcelPackage.License.SetNonCommercialPersonal("Phan Huy Vinh");

                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet worksheet =
                        package.Workbook.Worksheets.Add("Danh sách sách");

                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        worksheet.Cells[1, i + 1].Value =
                            dt.Columns[i].ColumnName;
                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            worksheet.Cells[i + 2, j + 1].Value =
                                dt.Rows[i][j];
                        }
                    }

                    worksheet.Cells.AutoFitColumns();

                    FileInfo fileInfo =
                        new FileInfo(saveFileDialog.FileName);

                    package.SaveAs(fileInfo);
                }

                MessageBox.Show("Xuất dữ liệu ra Excel thành công!");
            }
        }
    }
}
