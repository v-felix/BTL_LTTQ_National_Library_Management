namespace QuanLyThuVienQuocGia
{
    partial class frmCatalog
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.grid = new System.Windows.Forms.DataGridView();
            this.status = new System.Windows.Forms.Label();
            this.searchBar = new System.Windows.Forms.FlowLayoutPanel();
            this.lblSearch = new System.Windows.Forms.Label();
            this.search = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnStaffLoans = new System.Windows.Forms.Button();
            this.buttons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.code = new System.Windows.Forms.Label();
            this.fields = new System.Windows.Forms.TableLayoutPanel();
            this.lblTenSach = new System.Windows.Forms.Label();
            this.editTenSach = new System.Windows.Forms.TextBox();
            this.lblTenTG = new System.Windows.Forms.Label();
            this.editTenTG = new System.Windows.Forms.TextBox();
            this.lblMaNXB = new System.Windows.Forms.Label();
            this.editMaNXB = new System.Windows.Forms.ComboBox();
            this.lblMaXB = new System.Windows.Forms.Label();
            this.editMaXB = new System.Windows.Forms.TextBox();
            this.lblKhoSach = new System.Windows.Forms.Label();
            this.editKhoSach = new System.Windows.Forms.TextBox();
            this.lblGiaSach = new System.Windows.Forms.Label();
            this.editGiaSach = new System.Windows.Forms.TextBox();
            this.lblLanTaiBan = new System.Windows.Forms.Label();
            this.editLanTaiBan = new System.Windows.Forms.TextBox();
            this.lblMaTheLoai = new System.Windows.Forms.Label();
            this.editMaTheLoai = new System.Windows.Forms.ComboBox();
            this.lblMaNgonNgu = new System.Windows.Forms.Label();
            this.editMaNgonNgu = new System.Windows.Forms.ComboBox();
            this.lblSoTrang = new System.Windows.Forms.Label();
            this.editSoTrang = new System.Windows.Forms.TextBox();
            this.lblTomTatND = new System.Windows.Forms.Label();
            this.editTomTatND = new System.Windows.Forms.TextBox();
            this.lblSoLuong = new System.Windows.Forms.Label();
            this.editSoLuong = new System.Windows.Forms.TextBox();
            this.lblDonGiaThue = new System.Windows.Forms.Label();
            this.editDonGiaThue = new System.Windows.Forms.TextBox();
            this.editorPanel = new System.Windows.Forms.Panel();
            this.note = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.searchBar.SuspendLayout();
            this.buttons.SuspendLayout();
            this.fields.SuspendLayout();
            this.editorPanel.SuspendLayout();
            this.gridMaSachColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridTenSachColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridTenTGColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridMaNXBColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridMaXBColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridKhoSachColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridGiaSachColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridLanTaiBanColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridMaTheLoaiColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridMaNgonNguColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridSoTrangColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridTomTatNDColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridSoLuongColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridDonGiaThueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SuspendLayout();
            //
            // grid
            //
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.grid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.grid.BackgroundColor = System.Drawing.Color.White;
            this.grid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.Location = new System.Drawing.Point(0, 722);
            this.grid.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grid.MultiSelect = false;
            this.grid.Name = "grid";
            this.grid.ReadOnly = true;
            this.grid.RowHeadersVisible = false;
            this.grid.RowHeadersWidth = 62;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.Size = new System.Drawing.Size(1746, 308);
            this.grid.TabIndex = 0;
            this.grid.SelectionChanged += new System.EventHandler(this.Grid_SelectionChanged);
            //
            // status
            //
            this.status.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.status.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(32)))), ((int)(((byte)(64)))));
            this.status.Location = new System.Drawing.Point(0, 1030);
            this.status.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.status.Name = "status";
            this.status.Padding = new System.Windows.Forms.Padding(15, 15, 15, 15);
            this.status.Size = new System.Drawing.Size(1746, 52);
            this.status.TabIndex = 1;
            this.status.Text = "Sẵn sàng • Quản lý sách";
            //
            // searchBar
            //
            this.searchBar.AutoSize = true;
            this.searchBar.BackColor = System.Drawing.Color.White;
            this.searchBar.Controls.Add(this.lblSearch);
            this.searchBar.Controls.Add(this.search);
            this.searchBar.Controls.Add(this.btnSearch);
            this.searchBar.Controls.Add(this.btnExport);
            this.searchBar.Controls.Add(this.btnStaffLoans);
            this.searchBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.searchBar.Location = new System.Drawing.Point(0, 0);
            this.searchBar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.searchBar.Name = "searchBar";
            this.searchBar.Padding = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.searchBar.Size = new System.Drawing.Size(1746, 87);
            this.searchBar.TabIndex = 5;
            //
            // lblSearch
            //
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(20, 28);
            this.lblSearch.Margin = new System.Windows.Forms.Padding(8, 16, 8, 8);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(91, 28);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "Tìm kiếm";
            //
            // search
            //
            this.search.Location = new System.Drawing.Point(125, 24);
            this.search.Margin = new System.Windows.Forms.Padding(6, 12, 6, 6);
            this.search.Name = "search";
            this.search.Size = new System.Drawing.Size(493, 34);
            this.search.TabIndex = 1;
            this.search.TextChanged += new System.EventHandler(this.Search_TextChanged);
            //
            // btnSearch
            //
            this.btnSearch.AutoSize = true;
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(103)))), ((int)(((byte)(192)))));
            this.btnSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(630, 18);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnSearch.MinimumSize = new System.Drawing.Size(150, 51);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Padding = new System.Windows.Forms.Padding(12, 3, 12, 3);
            this.btnSearch.Size = new System.Drawing.Size(162, 51);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "Tìm / tải lại";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.Search_Click);
            //
            // btnExport
            //
            this.btnExport.AutoSize = true;
            this.btnExport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(103)))), ((int)(((byte)(192)))));
            this.btnExport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExport.FlatAppearance.BorderSize = 0;
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.Location = new System.Drawing.Point(804, 18);
            this.btnExport.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnExport.MinimumSize = new System.Drawing.Size(150, 51);
            this.btnExport.Name = "btnExport";
            this.btnExport.Padding = new System.Windows.Forms.Padding(12, 3, 12, 3);
            this.btnExport.Size = new System.Drawing.Size(166, 51);
            this.btnExport.TabIndex = 3;
            this.btnExport.Text = "Xuất Excel";
            this.btnExport.UseVisualStyleBackColor = false;
            this.btnExport.Click += new System.EventHandler(this.Export_Click);
            //
            // btnStaffLoans
            //
            this.btnStaffLoans.AutoSize = true;
            this.btnStaffLoans.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(103)))), ((int)(((byte)(192)))));
            this.btnStaffLoans.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStaffLoans.FlatAppearance.BorderSize = 0;
            this.btnStaffLoans.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStaffLoans.ForeColor = System.Drawing.Color.White;
            this.btnStaffLoans.Location = new System.Drawing.Point(982, 18);
            this.btnStaffLoans.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnStaffLoans.MinimumSize = new System.Drawing.Size(150, 51);
            this.btnStaffLoans.Name = "btnStaffLoans";
            this.btnStaffLoans.Padding = new System.Windows.Forms.Padding(12, 3, 12, 3);
            this.btnStaffLoans.Size = new System.Drawing.Size(260, 51);
            this.btnStaffLoans.TabIndex = 4;
            this.btnStaffLoans.Text = "Hồ sơ đã cho mượn";
            this.btnStaffLoans.UseVisualStyleBackColor = false;
            this.btnStaffLoans.Visible = false;
            this.btnStaffLoans.Click += new System.EventHandler(this.StaffLoans_Click);
            //
            // buttons
            //
            this.buttons.AutoSize = true;
            this.buttons.BackColor = System.Drawing.Color.White;
            this.buttons.Controls.Add(this.btnNew);
            this.buttons.Controls.Add(this.btnSave);
            this.buttons.Controls.Add(this.btnDelete);
            this.buttons.Controls.Add(this.btnRefresh);
            this.buttons.Controls.Add(this.code);
            this.buttons.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttons.Location = new System.Drawing.Point(0, 635);
            this.buttons.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttons.Name = "buttons";
            this.buttons.Padding = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.buttons.Size = new System.Drawing.Size(1746, 87);
            this.buttons.TabIndex = 2;
            //
            // btnNew
            //
            this.btnNew.AutoSize = true;
            this.btnNew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(103)))), ((int)(((byte)(192)))));
            this.btnNew.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNew.FlatAppearance.BorderSize = 0;
            this.btnNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNew.ForeColor = System.Drawing.Color.White;
            this.btnNew.Location = new System.Drawing.Point(18, 18);
            this.btnNew.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnNew.MinimumSize = new System.Drawing.Size(150, 51);
            this.btnNew.Name = "btnNew";
            this.btnNew.Padding = new System.Windows.Forms.Padding(12, 3, 12, 3);
            this.btnNew.Size = new System.Drawing.Size(156, 51);
            this.btnNew.TabIndex = 0;
            this.btnNew.Text = "Thêm mới";
            this.btnNew.UseVisualStyleBackColor = false;
            this.btnNew.Click += new System.EventHandler(this.New_Click);
            //
            // btnSave
            //
            this.btnSave.AutoSize = true;
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(103)))), ((int)(((byte)(192)))));
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(186, 18);
            this.btnSave.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnSave.MinimumSize = new System.Drawing.Size(150, 51);
            this.btnSave.Name = "btnSave";
            this.btnSave.Padding = new System.Windows.Forms.Padding(12, 3, 12, 3);
            this.btnSave.Size = new System.Drawing.Size(150, 51);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Lưu";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.Save_Click);
            //
            // btnDelete
            //
            this.btnDelete.AutoSize = true;
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(103)))), ((int)(((byte)(192)))));
            this.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Location = new System.Drawing.Point(348, 18);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnDelete.MinimumSize = new System.Drawing.Size(150, 51);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Padding = new System.Windows.Forms.Padding(12, 3, 12, 3);
            this.btnDelete.Size = new System.Drawing.Size(150, 51);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "Xóa";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.Delete_Click);
            //
            // btnRefresh
            //
            this.btnRefresh.AutoSize = true;
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(103)))), ((int)(((byte)(192)))));
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(510, 18);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnRefresh.MinimumSize = new System.Drawing.Size(150, 51);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Padding = new System.Windows.Forms.Padding(12, 3, 12, 3);
            this.btnRefresh.Size = new System.Drawing.Size(150, 51);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "Làm mới";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.Refresh_Click);
            //
            // code
            //
            this.code.AutoSize = true;
            this.code.Location = new System.Drawing.Point(670, 12);
            this.code.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.code.Name = "code";
            this.code.Padding = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.code.Size = new System.Drawing.Size(358, 52);
            this.code.TabIndex = 4;
            this.code.Text = "Đang thêm mới • Mã tự động khi lưu";
            //
            // fields
            //
            this.fields.AutoSize = true;
            this.fields.ColumnCount = 4;
            this.fields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 278F));
            this.fields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.fields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 278F));
            this.fields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.fields.Controls.Add(this.lblTenSach, 0, 0);
            this.fields.Controls.Add(this.editTenSach, 1, 0);
            this.fields.Controls.Add(this.lblTenTG, 2, 0);
            this.fields.Controls.Add(this.editTenTG, 3, 0);
            this.fields.Controls.Add(this.lblMaNXB, 0, 1);
            this.fields.Controls.Add(this.editMaNXB, 1, 1);
            this.fields.Controls.Add(this.lblMaXB, 2, 1);
            this.fields.Controls.Add(this.editMaXB, 3, 1);
            this.fields.Controls.Add(this.lblKhoSach, 0, 2);
            this.fields.Controls.Add(this.editKhoSach, 1, 2);
            this.fields.Controls.Add(this.lblGiaSach, 2, 2);
            this.fields.Controls.Add(this.editGiaSach, 3, 2);
            this.fields.Controls.Add(this.lblLanTaiBan, 0, 3);
            this.fields.Controls.Add(this.editLanTaiBan, 1, 3);
            this.fields.Controls.Add(this.lblMaTheLoai, 2, 3);
            this.fields.Controls.Add(this.editMaTheLoai, 3, 3);
            this.fields.Controls.Add(this.lblMaNgonNgu, 0, 4);
            this.fields.Controls.Add(this.editMaNgonNgu, 1, 4);
            this.fields.Controls.Add(this.lblSoTrang, 2, 4);
            this.fields.Controls.Add(this.editSoTrang, 3, 4);
            this.fields.Controls.Add(this.lblTomTatND, 0, 5);
            this.fields.Controls.Add(this.editTomTatND, 1, 5);
            this.fields.Controls.Add(this.lblSoLuong, 2, 5);
            this.fields.Controls.Add(this.editSoLuong, 3, 5);
            this.fields.Controls.Add(this.lblDonGiaThue, 0, 6);
            this.fields.Controls.Add(this.editDonGiaThue, 1, 6);
            this.fields.Dock = System.Windows.Forms.DockStyle.Top;
            this.fields.Location = new System.Drawing.Point(0, 0);
            this.fields.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.fields.Name = "fields";
            this.fields.Padding = new System.Windows.Forms.Padding(18, 18, 18, 18);
            this.fields.RowCount = 7;
            this.fields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.fields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.fields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.fields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.fields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.fields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.fields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.fields.Size = new System.Drawing.Size(1746, 456);
            this.fields.TabIndex = 0;
            //
            // lblTenSach
            //
            this.lblTenSach.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTenSach.AutoSize = true;
            this.lblTenSach.Location = new System.Drawing.Point(24, 34);
            this.lblTenSach.Margin = new System.Windows.Forms.Padding(6, 12, 6, 12);
            this.lblTenSach.Name = "lblTenSach";
            this.lblTenSach.Size = new System.Drawing.Size(97, 28);
            this.lblTenSach.TabIndex = 0;
            this.lblTenSach.Text = "Tên sách *";
            //
            // editTenSach
            //
            this.editTenSach.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editTenSach.Location = new System.Drawing.Point(302, 27);
            this.editTenSach.Margin = new System.Windows.Forms.Padding(6, 9, 18, 9);
            this.editTenSach.MaxLength = 200;
            this.editTenSach.Name = "editTenSach";
            this.editTenSach.Size = new System.Drawing.Size(553, 34);
            this.editTenSach.TabIndex = 0;
            //
            // lblTenTG
            //
            this.lblTenTG.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTenTG.AutoSize = true;
            this.lblTenTG.Location = new System.Drawing.Point(879, 34);
            this.lblTenTG.Margin = new System.Windows.Forms.Padding(6, 12, 6, 12);
            this.lblTenTG.Name = "lblTenTG";
            this.lblTenTG.Size = new System.Drawing.Size(71, 28);
            this.lblTenTG.TabIndex = 1;
            this.lblTenTG.Text = "Tác giả";
            //
            // editTenTG
            //
            this.editTenTG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editTenTG.Location = new System.Drawing.Point(1157, 27);
            this.editTenTG.Margin = new System.Windows.Forms.Padding(6, 9, 18, 9);
            this.editTenTG.MaxLength = 100;
            this.editTenTG.Name = "editTenTG";
            this.editTenTG.Size = new System.Drawing.Size(553, 34);
            this.editTenTG.TabIndex = 1;
            //
            // lblMaNXB
            //
            this.lblMaNXB.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblMaNXB.AutoSize = true;
            this.lblMaNXB.Location = new System.Drawing.Point(24, 94);
            this.lblMaNXB.Margin = new System.Windows.Forms.Padding(6, 12, 6, 12);
            this.lblMaNXB.Name = "lblMaNXB";
            this.lblMaNXB.Size = new System.Drawing.Size(128, 28);
            this.lblMaNXB.TabIndex = 2;
            this.lblMaNXB.Text = "Nhà xuất bản";
            //
            // editMaNXB
            //
            this.editMaNXB.DisplayMember = "Name";
            this.editMaNXB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editMaNXB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.editMaNXB.DropDownWidth = 450;
            this.editMaNXB.Location = new System.Drawing.Point(302, 87);
            this.editMaNXB.Margin = new System.Windows.Forms.Padding(6, 9, 18, 9);
            this.editMaNXB.Name = "editMaNXB";
            this.editMaNXB.Size = new System.Drawing.Size(553, 36);
            this.editMaNXB.TabIndex = 2;
            this.editMaNXB.ValueMember = "Id";
            //
            // lblMaXB
            //
            this.lblMaXB.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblMaXB.AutoSize = true;
            this.lblMaXB.Location = new System.Drawing.Point(879, 94);
            this.lblMaXB.Margin = new System.Windows.Forms.Padding(6, 12, 6, 12);
            this.lblMaXB.Name = "lblMaXB";
            this.lblMaXB.Size = new System.Drawing.Size(134, 28);
            this.lblMaXB.TabIndex = 3;
            this.lblMaXB.Text = "Năm xuất bản";
            //
            // editMaXB
            //
            this.editMaXB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editMaXB.Location = new System.Drawing.Point(1157, 87);
            this.editMaXB.Margin = new System.Windows.Forms.Padding(6, 9, 18, 9);
            this.editMaXB.MaxLength = 100;
            this.editMaXB.Name = "editMaXB";
            this.editMaXB.Size = new System.Drawing.Size(553, 34);
            this.editMaXB.TabIndex = 3;
            //
            // lblKhoSach
            //
            this.lblKhoSach.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblKhoSach.AutoSize = true;
            this.lblKhoSach.Location = new System.Drawing.Point(24, 154);
            this.lblKhoSach.Margin = new System.Windows.Forms.Padding(6, 12, 6, 12);
            this.lblKhoSach.Name = "lblKhoSach";
            this.lblKhoSach.Size = new System.Drawing.Size(90, 28);
            this.lblKhoSach.TabIndex = 4;
            this.lblKhoSach.Text = "Khổ sách";
            //
            // editKhoSach
            //
            this.editKhoSach.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editKhoSach.Location = new System.Drawing.Point(302, 147);
            this.editKhoSach.Margin = new System.Windows.Forms.Padding(6, 9, 18, 9);
            this.editKhoSach.MaxLength = 100;
            this.editKhoSach.Name = "editKhoSach";
            this.editKhoSach.Size = new System.Drawing.Size(553, 34);
            this.editKhoSach.TabIndex = 4;
            //
            // lblGiaSach
            //
            this.lblGiaSach.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblGiaSach.AutoSize = true;
            this.lblGiaSach.Location = new System.Drawing.Point(879, 154);
            this.lblGiaSach.Margin = new System.Windows.Forms.Padding(6, 12, 6, 12);
            this.lblGiaSach.Name = "lblGiaSach";
            this.lblGiaSach.Size = new System.Drawing.Size(84, 28);
            this.lblGiaSach.TabIndex = 5;
            this.lblGiaSach.Text = "Giá sách";
            //
            // editGiaSach
            //
            this.editGiaSach.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editGiaSach.Location = new System.Drawing.Point(1157, 147);
            this.editGiaSach.Margin = new System.Windows.Forms.Padding(6, 9, 18, 9);
            this.editGiaSach.MaxLength = 100;
            this.editGiaSach.Name = "editGiaSach";
            this.editGiaSach.Size = new System.Drawing.Size(553, 34);
            this.editGiaSach.TabIndex = 5;
            this.editGiaSach.Text = "0";
            //
            // lblLanTaiBan
            //
            this.lblLanTaiBan.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblLanTaiBan.AutoSize = true;
            this.lblLanTaiBan.Location = new System.Drawing.Point(24, 214);
            this.lblLanTaiBan.Margin = new System.Windows.Forms.Padding(6, 12, 6, 12);
            this.lblLanTaiBan.Name = "lblLanTaiBan";
            this.lblLanTaiBan.Size = new System.Drawing.Size(107, 28);
            this.lblLanTaiBan.TabIndex = 6;
            this.lblLanTaiBan.Text = "Lần tái bản";
            //
            // editLanTaiBan
            //
            this.editLanTaiBan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editLanTaiBan.Location = new System.Drawing.Point(302, 207);
            this.editLanTaiBan.Margin = new System.Windows.Forms.Padding(6, 9, 18, 9);
            this.editLanTaiBan.MaxLength = 100;
            this.editLanTaiBan.Name = "editLanTaiBan";
            this.editLanTaiBan.Size = new System.Drawing.Size(553, 34);
            this.editLanTaiBan.TabIndex = 6;
            //
            // lblMaTheLoai
            //
            this.lblMaTheLoai.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblMaTheLoai.AutoSize = true;
            this.lblMaTheLoai.Location = new System.Drawing.Point(879, 214);
            this.lblMaTheLoai.Margin = new System.Windows.Forms.Padding(6, 12, 6, 12);
            this.lblMaTheLoai.Name = "lblMaTheLoai";
            this.lblMaTheLoai.Size = new System.Drawing.Size(80, 28);
            this.lblMaTheLoai.TabIndex = 7;
            this.lblMaTheLoai.Text = "Thể loại";
            //
            // editMaTheLoai
            //
            this.editMaTheLoai.DisplayMember = "Name";
            this.editMaTheLoai.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editMaTheLoai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.editMaTheLoai.DropDownWidth = 450;
            this.editMaTheLoai.Location = new System.Drawing.Point(1157, 207);
            this.editMaTheLoai.Margin = new System.Windows.Forms.Padding(6, 9, 18, 9);
            this.editMaTheLoai.Name = "editMaTheLoai";
            this.editMaTheLoai.Size = new System.Drawing.Size(553, 36);
            this.editMaTheLoai.TabIndex = 7;
            this.editMaTheLoai.ValueMember = "Id";
            //
            // lblMaNgonNgu
            //
            this.lblMaNgonNgu.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblMaNgonNgu.AutoSize = true;
            this.lblMaNgonNgu.Location = new System.Drawing.Point(24, 274);
            this.lblMaNgonNgu.Margin = new System.Windows.Forms.Padding(6, 12, 6, 12);
            this.lblMaNgonNgu.Name = "lblMaNgonNgu";
            this.lblMaNgonNgu.Size = new System.Drawing.Size(102, 28);
            this.lblMaNgonNgu.TabIndex = 8;
            this.lblMaNgonNgu.Text = "Ngôn ngữ";
            //
            // editMaNgonNgu
            //
            this.editMaNgonNgu.DisplayMember = "Name";
            this.editMaNgonNgu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editMaNgonNgu.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.editMaNgonNgu.DropDownWidth = 450;
            this.editMaNgonNgu.Location = new System.Drawing.Point(302, 267);
            this.editMaNgonNgu.Margin = new System.Windows.Forms.Padding(6, 9, 18, 9);
            this.editMaNgonNgu.Name = "editMaNgonNgu";
            this.editMaNgonNgu.Size = new System.Drawing.Size(553, 36);
            this.editMaNgonNgu.TabIndex = 8;
            this.editMaNgonNgu.ValueMember = "Id";
            //
            // lblSoTrang
            //
            this.lblSoTrang.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblSoTrang.AutoSize = true;
            this.lblSoTrang.Location = new System.Drawing.Point(879, 274);
            this.lblSoTrang.Margin = new System.Windows.Forms.Padding(6, 12, 6, 12);
            this.lblSoTrang.Name = "lblSoTrang";
            this.lblSoTrang.Size = new System.Drawing.Size(87, 28);
            this.lblSoTrang.TabIndex = 9;
            this.lblSoTrang.Text = "Số trang";
            //
            // editSoTrang
            //
            this.editSoTrang.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editSoTrang.Location = new System.Drawing.Point(1157, 267);
            this.editSoTrang.Margin = new System.Windows.Forms.Padding(6, 9, 18, 9);
            this.editSoTrang.MaxLength = 100;
            this.editSoTrang.Name = "editSoTrang";
            this.editSoTrang.Size = new System.Drawing.Size(553, 34);
            this.editSoTrang.TabIndex = 9;
            //
            // lblTomTatND
            //
            this.lblTomTatND.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTomTatND.AutoSize = true;
            this.lblTomTatND.Location = new System.Drawing.Point(24, 334);
            this.lblTomTatND.Margin = new System.Windows.Forms.Padding(6, 12, 6, 12);
            this.lblTomTatND.Name = "lblTomTatND";
            this.lblTomTatND.Size = new System.Drawing.Size(162, 28);
            this.lblTomTatND.TabIndex = 10;
            this.lblTomTatND.Text = "Tóm tắt nội dung";
            //
            // editTomTatND
            //
            this.editTomTatND.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editTomTatND.Location = new System.Drawing.Point(302, 327);
            this.editTomTatND.Margin = new System.Windows.Forms.Padding(6, 9, 18, 9);
            this.editTomTatND.Name = "editTomTatND";
            this.editTomTatND.Size = new System.Drawing.Size(553, 34);
            this.editTomTatND.TabIndex = 10;
            //
            // lblSoLuong
            //
            this.lblSoLuong.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblSoLuong.AutoSize = true;
            this.lblSoLuong.Location = new System.Drawing.Point(879, 334);
            this.lblSoLuong.Margin = new System.Windows.Forms.Padding(6, 12, 6, 12);
            this.lblSoLuong.Name = "lblSoLuong";
            this.lblSoLuong.Size = new System.Drawing.Size(234, 28);
            this.lblSoLuong.TabIndex = 11;
            this.lblSoLuong.Text = "Số lượng còn trong kho *";
            //
            // editSoLuong
            //
            this.editSoLuong.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editSoLuong.Location = new System.Drawing.Point(1157, 327);
            this.editSoLuong.Margin = new System.Windows.Forms.Padding(6, 9, 18, 9);
            this.editSoLuong.MaxLength = 100;
            this.editSoLuong.Name = "editSoLuong";
            this.editSoLuong.Size = new System.Drawing.Size(553, 34);
            this.editSoLuong.TabIndex = 11;
            this.editSoLuong.Text = "0";
            //
            // lblDonGiaThue
            //
            this.lblDonGiaThue.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDonGiaThue.AutoSize = true;
            this.lblDonGiaThue.Location = new System.Drawing.Point(24, 394);
            this.lblDonGiaThue.Margin = new System.Windows.Forms.Padding(6, 12, 6, 12);
            this.lblDonGiaThue.Name = "lblDonGiaThue";
            this.lblDonGiaThue.Size = new System.Drawing.Size(189, 28);
            this.lblDonGiaThue.TabIndex = 12;
            this.lblDonGiaThue.Text = "Đơn giá thuê/ngày *";
            //
            // editDonGiaThue
            //
            this.editDonGiaThue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editDonGiaThue.Location = new System.Drawing.Point(302, 387);
            this.editDonGiaThue.Margin = new System.Windows.Forms.Padding(6, 9, 18, 9);
            this.editDonGiaThue.MaxLength = 100;
            this.editDonGiaThue.Name = "editDonGiaThue";
            this.editDonGiaThue.Size = new System.Drawing.Size(553, 34);
            this.editDonGiaThue.TabIndex = 12;
            this.editDonGiaThue.Text = "0";
            //
            // editorPanel
            //
            this.editorPanel.AutoScroll = true;
            this.editorPanel.Controls.Add(this.fields);
            this.editorPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.editorPanel.Location = new System.Drawing.Point(0, 133);
            this.editorPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.editorPanel.Name = "editorPanel";
            this.editorPanel.Size = new System.Drawing.Size(1746, 502);
            this.editorPanel.TabIndex = 3;
            //
            // note
            //
            this.note.AutoSize = true;
            this.note.Dock = System.Windows.Forms.DockStyle.Top;
            this.note.ForeColor = System.Drawing.Color.DimGray;
            this.note.Location = new System.Drawing.Point(0, 87);
            this.note.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.note.Name = "note";
            this.note.Padding = new System.Windows.Forms.Padding(18, 6, 18, 12);
            this.note.Size = new System.Drawing.Size(1207, 46);
            this.note.TabIndex = 4;
            this.note.Text = "Thêm mới → nhập thông tin → nhấn Lưu để thêm sách. Số lượng là số cuốn hiện còn t" +
    "rong kho; hãy nhập tồn thực tế và đơn giá thuê.";
            //
            // frmCatalog
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1746, 1082);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.status);
            this.Controls.Add(this.buttons);
            this.Controls.Add(this.editorPanel);
            this.Controls.Add(this.note);
            this.Controls.Add(this.searchBar);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MinimumSize = new System.Drawing.Size(1339, 902);
            this.Name = "frmCatalog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Quản lý sách";
            this.Shown += new System.EventHandler(this.Catalog_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.searchBar.ResumeLayout(false);
            this.searchBar.PerformLayout();
            this.buttons.ResumeLayout(false);
            this.buttons.PerformLayout();
            this.fields.ResumeLayout(false);
            this.fields.PerformLayout();
            this.editorPanel.ResumeLayout(false);
            this.editorPanel.PerformLayout();
            this.gridMaSachColumn.HeaderText = "Mã";
            this.gridMaSachColumn.Width = 100;
            this.gridMaSachColumn.MinimumWidth = 65;
            this.gridMaSachColumn.ReadOnly = true;
            this.gridMaSachColumn.Name = "gridMaSachColumn";
            this.gridMaSachColumn.DataPropertyName = "MaSach";
            this.gridTenSachColumn.HeaderText = "Tên sách";
            this.gridTenSachColumn.Width = 100;
            this.gridTenSachColumn.MinimumWidth = 65;
            this.gridTenSachColumn.ReadOnly = true;
            this.gridTenSachColumn.Name = "gridTenSachColumn";
            this.gridTenSachColumn.DataPropertyName = "TenSach";
            this.gridTenTGColumn.HeaderText = "Tác giả";
            this.gridTenTGColumn.Width = 100;
            this.gridTenTGColumn.MinimumWidth = 65;
            this.gridTenTGColumn.ReadOnly = true;
            this.gridTenTGColumn.Name = "gridTenTGColumn";
            this.gridTenTGColumn.DataPropertyName = "TenTG";
            this.gridMaNXBColumn.HeaderText = "Nhà xuất bản";
            this.gridMaNXBColumn.Width = 100;
            this.gridMaNXBColumn.MinimumWidth = 65;
            this.gridMaNXBColumn.ReadOnly = true;
            this.gridMaNXBColumn.Name = "gridMaNXBColumn";
            this.gridMaNXBColumn.DataPropertyName = "MaNXBText";
            this.gridMaXBColumn.HeaderText = "Năm xuất bản";
            this.gridMaXBColumn.Width = 100;
            this.gridMaXBColumn.MinimumWidth = 65;
            this.gridMaXBColumn.ReadOnly = true;
            this.gridMaXBColumn.Name = "gridMaXBColumn";
            this.gridMaXBColumn.DataPropertyName = "MaXB";
            this.gridKhoSachColumn.HeaderText = "Khổ sách";
            this.gridKhoSachColumn.Width = 100;
            this.gridKhoSachColumn.MinimumWidth = 65;
            this.gridKhoSachColumn.ReadOnly = true;
            this.gridKhoSachColumn.Name = "gridKhoSachColumn";
            this.gridKhoSachColumn.DataPropertyName = "KhoSach";
            this.gridGiaSachColumn.HeaderText = "Giá sách";
            this.gridGiaSachColumn.Width = 100;
            this.gridGiaSachColumn.MinimumWidth = 65;
            this.gridGiaSachColumn.ReadOnly = true;
            this.gridGiaSachColumn.Name = "gridGiaSachColumn";
            this.gridGiaSachColumn.DataPropertyName = "GiaSach";
            this.gridLanTaiBanColumn.HeaderText = "Lần tái bản";
            this.gridLanTaiBanColumn.Width = 100;
            this.gridLanTaiBanColumn.MinimumWidth = 65;
            this.gridLanTaiBanColumn.ReadOnly = true;
            this.gridLanTaiBanColumn.Name = "gridLanTaiBanColumn";
            this.gridLanTaiBanColumn.DataPropertyName = "LanTaiBan";
            this.gridMaTheLoaiColumn.HeaderText = "Thể loại";
            this.gridMaTheLoaiColumn.Width = 100;
            this.gridMaTheLoaiColumn.MinimumWidth = 65;
            this.gridMaTheLoaiColumn.ReadOnly = true;
            this.gridMaTheLoaiColumn.Name = "gridMaTheLoaiColumn";
            this.gridMaTheLoaiColumn.DataPropertyName = "MaTheLoaiText";
            this.gridMaNgonNguColumn.HeaderText = "Ngôn ngữ";
            this.gridMaNgonNguColumn.Width = 100;
            this.gridMaNgonNguColumn.MinimumWidth = 65;
            this.gridMaNgonNguColumn.ReadOnly = true;
            this.gridMaNgonNguColumn.Name = "gridMaNgonNguColumn";
            this.gridMaNgonNguColumn.DataPropertyName = "MaNgonNguText";
            this.gridSoTrangColumn.HeaderText = "Số trang";
            this.gridSoTrangColumn.Width = 100;
            this.gridSoTrangColumn.MinimumWidth = 65;
            this.gridSoTrangColumn.ReadOnly = true;
            this.gridSoTrangColumn.Name = "gridSoTrangColumn";
            this.gridSoTrangColumn.DataPropertyName = "SoTrang";
            this.gridTomTatNDColumn.HeaderText = "Tóm tắt nội dung";
            this.gridTomTatNDColumn.Width = 100;
            this.gridTomTatNDColumn.MinimumWidth = 65;
            this.gridTomTatNDColumn.ReadOnly = true;
            this.gridTomTatNDColumn.Name = "gridTomTatNDColumn";
            this.gridTomTatNDColumn.DataPropertyName = "TomTatND";
            this.gridSoLuongColumn.HeaderText = "Số lượng còn trong kho";
            this.gridSoLuongColumn.Width = 100;
            this.gridSoLuongColumn.MinimumWidth = 65;
            this.gridSoLuongColumn.ReadOnly = true;
            this.gridSoLuongColumn.Name = "gridSoLuongColumn";
            this.gridSoLuongColumn.DataPropertyName = "SoLuong";
            this.gridDonGiaThueColumn.HeaderText = "Đơn giá thuê/ngày";
            this.gridDonGiaThueColumn.Width = 100;
            this.gridDonGiaThueColumn.MinimumWidth = 65;
            this.gridDonGiaThueColumn.ReadOnly = true;
            this.gridDonGiaThueColumn.Name = "gridDonGiaThueColumn";
            this.gridDonGiaThueColumn.DataPropertyName = "DonGiaThue";
            this.grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { this.gridMaSachColumn, this.gridTenSachColumn, this.gridTenTGColumn, this.gridMaNXBColumn, this.gridMaXBColumn, this.gridKhoSachColumn, this.gridGiaSachColumn, this.gridLanTaiBanColumn, this.gridMaTheLoaiColumn, this.gridMaNgonNguColumn, this.gridSoTrangColumn, this.gridTomTatNDColumn, this.gridSoLuongColumn, this.gridDonGiaThueColumn });
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        protected System.Windows.Forms.DataGridView grid;
        protected System.Windows.Forms.Label status;
        protected System.Windows.Forms.FlowLayoutPanel searchBar;
        protected System.Windows.Forms.Label lblSearch;
        protected System.Windows.Forms.TextBox search;
        protected System.Windows.Forms.Button btnSearch;
        protected System.Windows.Forms.Button btnExport;
        protected System.Windows.Forms.Button btnStaffLoans;
        protected System.Windows.Forms.FlowLayoutPanel buttons;
        protected System.Windows.Forms.Button btnNew;
        protected System.Windows.Forms.Button btnSave;
        protected System.Windows.Forms.Button btnDelete;
        protected System.Windows.Forms.Button btnRefresh;
        protected System.Windows.Forms.Label code;
        protected System.Windows.Forms.TableLayoutPanel fields;
        protected System.Windows.Forms.Label lblTenSach;
        protected System.Windows.Forms.TextBox editTenSach;
        protected System.Windows.Forms.Label lblTenTG;
        protected System.Windows.Forms.TextBox editTenTG;
        protected System.Windows.Forms.Label lblMaNXB;
        protected System.Windows.Forms.ComboBox editMaNXB;
        protected System.Windows.Forms.Label lblMaXB;
        protected System.Windows.Forms.TextBox editMaXB;
        protected System.Windows.Forms.Label lblKhoSach;
        protected System.Windows.Forms.TextBox editKhoSach;
        protected System.Windows.Forms.Label lblGiaSach;
        protected System.Windows.Forms.TextBox editGiaSach;
        protected System.Windows.Forms.Label lblLanTaiBan;
        protected System.Windows.Forms.TextBox editLanTaiBan;
        protected System.Windows.Forms.Label lblMaTheLoai;
        protected System.Windows.Forms.ComboBox editMaTheLoai;
        protected System.Windows.Forms.Label lblMaNgonNgu;
        protected System.Windows.Forms.ComboBox editMaNgonNgu;
        protected System.Windows.Forms.Label lblSoTrang;
        protected System.Windows.Forms.TextBox editSoTrang;
        protected System.Windows.Forms.Label lblTomTatND;
        protected System.Windows.Forms.TextBox editTomTatND;
        protected System.Windows.Forms.Label lblSoLuong;
        protected System.Windows.Forms.TextBox editSoLuong;
        protected System.Windows.Forms.Label lblDonGiaThue;
        protected System.Windows.Forms.TextBox editDonGiaThue;
        protected System.Windows.Forms.Panel editorPanel;
        protected System.Windows.Forms.Label note;
        protected System.Windows.Forms.DataGridViewTextBoxColumn gridMaSachColumn;
        protected System.Windows.Forms.DataGridViewTextBoxColumn gridTenSachColumn;
        protected System.Windows.Forms.DataGridViewTextBoxColumn gridTenTGColumn;
        protected System.Windows.Forms.DataGridViewTextBoxColumn gridMaNXBColumn;
        protected System.Windows.Forms.DataGridViewTextBoxColumn gridMaXBColumn;
        protected System.Windows.Forms.DataGridViewTextBoxColumn gridKhoSachColumn;
        protected System.Windows.Forms.DataGridViewTextBoxColumn gridGiaSachColumn;
        protected System.Windows.Forms.DataGridViewTextBoxColumn gridLanTaiBanColumn;
        protected System.Windows.Forms.DataGridViewTextBoxColumn gridMaTheLoaiColumn;
        protected System.Windows.Forms.DataGridViewTextBoxColumn gridMaNgonNguColumn;
        protected System.Windows.Forms.DataGridViewTextBoxColumn gridSoTrangColumn;
        protected System.Windows.Forms.DataGridViewTextBoxColumn gridTomTatNDColumn;
        protected System.Windows.Forms.DataGridViewTextBoxColumn gridSoLuongColumn;
        protected System.Windows.Forms.DataGridViewTextBoxColumn gridDonGiaThueColumn;
    }
}
