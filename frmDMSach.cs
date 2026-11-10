namespace QuanLyThuVienQuocGia
{
    public partial class frmDMSach : System.Windows.Forms.Form
    {
        CatalogController controller;
        public frmDMSach()
        {
            InitializeComponent();
            controller=new CatalogController(this,"sach");
        }
        private void Catalog_Shown(object sender,System.EventArgs e) { if(controller!=null)controller.HandleCatalog_Shown(sender,e); }
        private void Search_Click(object sender,System.EventArgs e) { if(controller!=null)controller.HandleSearch_Click(sender,e); }
        private void Export_Click(object sender,System.EventArgs e) { if(controller!=null)controller.HandleExport_Click(sender,e); }
        private void StaffLoans_Click(object sender,System.EventArgs e) { if(controller!=null)controller.HandleStaffLoans_Click(sender,e); }
        private void New_Click(object sender,System.EventArgs e) { if(controller!=null)controller.HandleNew_Click(sender,e); }
        private void Save_Click(object sender,System.EventArgs e) { if(controller!=null)controller.HandleSave_Click(sender,e); }
        private void Delete_Click(object sender,System.EventArgs e) { if(controller!=null)controller.HandleDelete_Click(sender,e); }
        private void Refresh_Click(object sender,System.EventArgs e) { if(controller!=null)controller.HandleRefresh_Click(sender,e); }
        private void Grid_SelectionChanged(object sender,System.EventArgs e) { if(controller!=null)controller.HandleGrid_SelectionChanged(sender,e); }
        private void Search_TextChanged(object sender,System.EventArgs e) { if(controller!=null)controller.HandleSearch_TextChanged(sender,e); }
        private void LoadLookups() { controller.LoadLookups(); }
        private void LoadRows() { controller.LoadRows(); }
        private void NewRecord() { controller.NewRecord(); }
        private void Save() { controller.Save(); }
    }
}
