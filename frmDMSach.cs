namespace QuanLyThuVienQuocGia
{
    public partial class frmDMSach : CatalogFormBase
    {
        public frmDMSach()
        {
            InitializeComponent();
            ConfigureCatalog("sach");
        }
        private void Catalog_Shown(object sender,System.EventArgs e) { HandleCatalog_Shown(sender,e); }
        private void Search_Click(object sender,System.EventArgs e) { HandleSearch_Click(sender,e); }
        private void Export_Click(object sender,System.EventArgs e) { HandleExport_Click(sender,e); }
        private void StaffLoans_Click(object sender,System.EventArgs e) { HandleStaffLoans_Click(sender,e); }
        private void New_Click(object sender,System.EventArgs e) { HandleNew_Click(sender,e); }
        private void Save_Click(object sender,System.EventArgs e) { HandleSave_Click(sender,e); }
        private void Delete_Click(object sender,System.EventArgs e) { HandleDelete_Click(sender,e); }
        private void Refresh_Click(object sender,System.EventArgs e) { HandleRefresh_Click(sender,e); }
        private void Grid_SelectionChanged(object sender,System.EventArgs e) { HandleGrid_SelectionChanged(sender,e); }
        private void Search_TextChanged(object sender,System.EventArgs e) { HandleSearch_TextChanged(sender,e); }
    }
}
