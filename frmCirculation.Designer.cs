namespace QuanLyThuVienQuocGia
{
    partial class frmCirculation
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        // Component names remain stable in the Designer; data loads only at runtime.
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.grid = new System.Windows.Forms.DataGridView();
            this.gridChooseColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.gridMaSachColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridTenSachColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridTenTheLoaiColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridTenNXBColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridSoLuongColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridNgayMuonColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridDonGiaThueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridViolationColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.gridAmountColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.summary = new System.Windows.Forms.Label();
            this.header = new System.Windows.Forms.FlowLayoutPanel();
            this.lblOwner = new System.Windows.Forms.Label();
            this.cardOrLoan = new System.Windows.Forms.ComboBox();
            this.lblStaff = new System.Windows.Forms.Label();
            this.librarian = new System.Windows.Forms.ComboBox();
            this.lblDate = new System.Windows.Forms.Label();
            this.date = new System.Windows.Forms.DateTimePicker();
            this.actions = new System.Windows.Forms.FlowLayoutPanel();
            this.lblDeposit = new System.Windows.Forms.Label();
            this.deposit = new System.Windows.Forms.NumericUpDown();
            this.lblSearch = new System.Windows.Forms.Label();
            this.search = new System.Windows.Forms.TextBox();
            this.btnReload = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnHistory = new System.Windows.Forms.Button();
            this.note = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.header.SuspendLayout();
            this.actions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.deposit)).BeginInit();
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
            this.grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.gridChooseColumn,
            this.gridMaSachColumn,
            this.gridTenSachColumn,
            this.gridTenTheLoaiColumn,
            this.gridTenNXBColumn,
            this.gridSoLuongColumn,
            this.gridNgayMuonColumn,
            this.gridDonGiaThueColumn,
            this.gridViolationColumn,
            this.gridAmountColumn});
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.Location = new System.Drawing.Point(0, 227);
            this.grid.Margin = new System.Windows.Forms.Padding(4);
            this.grid.MultiSelect = false;
            this.grid.Name = "grid";
            this.grid.RowHeadersVisible = false;
            this.grid.RowHeadersWidth = 62;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.Size = new System.Drawing.Size(1851, 711);
            this.grid.TabIndex = 0;
            //
            // gridChooseColumn
            //
            this.gridChooseColumn.FalseValue = false;
            this.gridChooseColumn.HeaderText = "Chọn";
            this.gridChooseColumn.MinimumWidth = 65;
            this.gridChooseColumn.Name = "gridChooseColumn";
            this.gridChooseColumn.TrueValue = true;
            this.gridChooseColumn.Width = 65;
            //
            // gridMaSachColumn
            //
            this.gridMaSachColumn.HeaderText = "Mã sách";
            this.gridMaSachColumn.MinimumWidth = 65;
            this.gridMaSachColumn.Name = "gridMaSachColumn";
            this.gridMaSachColumn.ReadOnly = true;
            this.gridMaSachColumn.Width = 119;
            //
            // gridTenSachColumn
            //
            this.gridTenSachColumn.HeaderText = "Tên sách";
            this.gridTenSachColumn.MinimumWidth = 65;
            this.gridTenSachColumn.Name = "gridTenSachColumn";
            this.gridTenSachColumn.ReadOnly = true;
            this.gridTenSachColumn.Width = 120;
            //
            // gridTenTheLoaiColumn
            //
            this.gridTenTheLoaiColumn.HeaderText = "Thể loại";
            this.gridTenTheLoaiColumn.MinimumWidth = 65;
            this.gridTenTheLoaiColumn.Name = "gridTenTheLoaiColumn";
            this.gridTenTheLoaiColumn.ReadOnly = true;
            this.gridTenTheLoaiColumn.Width = 116;
            //
            // gridTenNXBColumn
            //
            this.gridTenNXBColumn.HeaderText = "Nhà xuất bản";
            this.gridTenNXBColumn.MinimumWidth = 65;
            this.gridTenNXBColumn.Name = "gridTenNXBColumn";
            this.gridTenNXBColumn.ReadOnly = true;
            this.gridTenNXBColumn.Width = 164;
            //
            // gridSoLuongColumn
            //
            this.gridSoLuongColumn.HeaderText = "Còn trong kho";
            this.gridSoLuongColumn.MinimumWidth = 65;
            this.gridSoLuongColumn.Name = "gridSoLuongColumn";
            this.gridSoLuongColumn.ReadOnly = true;
            this.gridSoLuongColumn.Width = 175;
            //
            // gridNgayMuonColumn
            //
            this.gridNgayMuonColumn.HeaderText = "Ngày mượn";
            this.gridNgayMuonColumn.MinimumWidth = 65;
            this.gridNgayMuonColumn.Name = "gridNgayMuonColumn";
            this.gridNgayMuonColumn.ReadOnly = true;
            this.gridNgayMuonColumn.Visible = false;
            this.gridNgayMuonColumn.Width = 120;
            //
            // gridDonGiaThueColumn
            //
            this.gridDonGiaThueColumn.HeaderText = "Đơn giá/ngày";
            this.gridDonGiaThueColumn.MinimumWidth = 65;
            this.gridDonGiaThueColumn.Name = "gridDonGiaThueColumn";
            this.gridDonGiaThueColumn.ReadOnly = true;
            this.gridDonGiaThueColumn.Width = 168;
            //
            // gridViolationColumn
            //
            dataGridViewCellStyle3.NullValue = "Không vi phạm";
            this.gridViolationColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.gridViolationColumn.HeaderText = "Vi phạm";
            this.gridViolationColumn.MinimumWidth = 65;
            this.gridViolationColumn.Name = "gridViolationColumn";
            this.gridViolationColumn.Visible = false;
            this.gridViolationColumn.Width = 210;
            //
            // gridAmountColumn
            //
            dataGridViewCellStyle4.Format = "N0";
            this.gridAmountColumn.DefaultCellStyle = dataGridViewCellStyle4;
            this.gridAmountColumn.HeaderText = "Thành tiền";
            this.gridAmountColumn.MinimumWidth = 65;
            this.gridAmountColumn.Name = "gridAmountColumn";
            this.gridAmountColumn.ReadOnly = true;
            this.gridAmountColumn.Visible = false;
            this.gridAmountColumn.Width = 130;
            //
            // summary
            //
            this.summary.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.summary.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(32)))), ((int)(((byte)(64)))));
            this.summary.Location = new System.Drawing.Point(0, 938);
            this.summary.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.summary.Name = "summary";
            this.summary.Padding = new System.Windows.Forms.Padding(18);
            this.summary.Size = new System.Drawing.Size(1851, 114);
            this.summary.TabIndex = 1;
            this.summary.Text = "Đã chọn 0 cuốn • Tạm ứng: 0 đ";
            //
            // header
            //
            this.header.AutoSize = true;
            this.header.BackColor = System.Drawing.Color.White;
            this.header.Controls.Add(this.lblOwner);
            this.header.Controls.Add(this.cardOrLoan);
            this.header.Controls.Add(this.lblStaff);
            this.header.Controls.Add(this.librarian);
            this.header.Controls.Add(this.lblDate);
            this.header.Controls.Add(this.date);
            this.header.Dock = System.Windows.Forms.DockStyle.Top;
            this.header.Location = new System.Drawing.Point(0, 0);
            this.header.Margin = new System.Windows.Forms.Padding(4);
            this.header.Name = "header";
            this.header.Padding = new System.Windows.Forms.Padding(12);
            this.header.Size = new System.Drawing.Size(1851, 76);
            this.header.TabIndex = 4;
            //
            // lblOwner
            //
            this.lblOwner.AutoSize = true;
            this.lblOwner.Location = new System.Drawing.Point(20, 28);
            this.lblOwner.Margin = new System.Windows.Forms.Padding(8, 16, 8, 8);
            this.lblOwner.Name = "lblOwner";
            this.lblOwner.Size = new System.Drawing.Size(100, 28);
            this.lblOwner.TabIndex = 0;
            this.lblOwner.Text = "Thẻ mượn";
            //
            // cardOrLoan
            //
            this.cardOrLoan.DisplayMember = "Name";
            this.cardOrLoan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cardOrLoan.DropDownWidth = 450;
            this.cardOrLoan.Location = new System.Drawing.Point(134, 22);
            this.cardOrLoan.Margin = new System.Windows.Forms.Padding(6, 10, 6, 6);
            this.cardOrLoan.Name = "cardOrLoan";
            this.cardOrLoan.Size = new System.Drawing.Size(463, 36);
            this.cardOrLoan.TabIndex = 1;
            this.cardOrLoan.ValueMember = "Id";
            //
            // lblStaff
            //
            this.lblStaff.AutoSize = true;
            this.lblStaff.Location = new System.Drawing.Point(611, 28);
            this.lblStaff.Margin = new System.Windows.Forms.Padding(8, 16, 8, 8);
            this.lblStaff.Name = "lblStaff";
            this.lblStaff.Size = new System.Drawing.Size(79, 28);
            this.lblStaff.TabIndex = 2;
            this.lblStaff.Text = "Thủ thư";
            //
            // librarian
            //
            this.librarian.DisplayMember = "Name";
            this.librarian.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.librarian.DropDownWidth = 450;
            this.librarian.Location = new System.Drawing.Point(704, 22);
            this.librarian.Margin = new System.Windows.Forms.Padding(6, 10, 6, 6);
            this.librarian.Name = "librarian";
            this.librarian.Size = new System.Drawing.Size(366, 36);
            this.librarian.TabIndex = 3;
            this.librarian.ValueMember = "Id";
            //
            // lblDate
            //
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(1084, 28);
            this.lblDate.Margin = new System.Windows.Forms.Padding(8, 16, 8, 8);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(116, 28);
            this.lblDate.TabIndex = 4;
            this.lblDate.Text = "Ngày mượn";
            //
            // date
            //
            this.date.CustomFormat = "dd/MM/yyyy";
            this.date.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.date.Location = new System.Drawing.Point(1214, 22);
            this.date.Margin = new System.Windows.Forms.Padding(6, 10, 6, 6);
            this.date.Name = "date";
            this.date.Size = new System.Drawing.Size(193, 34);
            this.date.TabIndex = 5;
            //
            // actions
            //
            this.actions.AutoSize = true;
            this.actions.BackColor = System.Drawing.Color.White;
            this.actions.Controls.Add(this.lblDeposit);
            this.actions.Controls.Add(this.deposit);
            this.actions.Controls.Add(this.lblSearch);
            this.actions.Controls.Add(this.search);
            this.actions.Controls.Add(this.btnReload);
            this.actions.Controls.Add(this.btnSave);
            this.actions.Controls.Add(this.btnHistory);
            this.actions.Dock = System.Windows.Forms.DockStyle.Top;
            this.actions.Location = new System.Drawing.Point(0, 76);
            this.actions.Margin = new System.Windows.Forms.Padding(4);
            this.actions.Name = "actions";
            this.actions.Padding = new System.Windows.Forms.Padding(12);
            this.actions.Size = new System.Drawing.Size(1851, 87);
            this.actions.TabIndex = 3;
            //
            // lblDeposit
            //
            this.lblDeposit.AutoSize = true;
            this.lblDeposit.Location = new System.Drawing.Point(20, 28);
            this.lblDeposit.Margin = new System.Windows.Forms.Padding(8, 16, 8, 8);
            this.lblDeposit.Name = "lblDeposit";
            this.lblDeposit.Size = new System.Drawing.Size(89, 28);
            this.lblDeposit.TabIndex = 0;
            this.lblDeposit.Text = "Tạm ứng";
            //
            // deposit
            //
            this.deposit.DecimalPlaces = 2;
            this.deposit.Location = new System.Drawing.Point(123, 22);
            this.deposit.Margin = new System.Windows.Forms.Padding(6, 10, 6, 6);
            this.deposit.Maximum = new decimal(new int[] {
            -727379969,
            232,
            0,
            0});
            this.deposit.Name = "deposit";
            this.deposit.Size = new System.Drawing.Size(218, 34);
            this.deposit.TabIndex = 1;
            this.deposit.ThousandsSeparator = true;
            //
            // lblSearch
            //
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(355, 28);
            this.lblSearch.Margin = new System.Windows.Forms.Padding(8, 16, 8, 8);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(87, 28);
            this.lblSearch.TabIndex = 2;
            this.lblSearch.Text = "Tìm sách";
            //
            // search
            //
            this.search.Location = new System.Drawing.Point(456, 22);
            this.search.Margin = new System.Windows.Forms.Padding(6, 10, 6, 6);
            this.search.Name = "search";
            this.search.Size = new System.Drawing.Size(373, 34);
            this.search.TabIndex = 3;
            //
            // btnReload
            //
            this.btnReload.AutoSize = true;
            this.btnReload.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(103)))), ((int)(((byte)(192)))));
            this.btnReload.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReload.FlatAppearance.BorderSize = 0;
            this.btnReload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReload.ForeColor = System.Drawing.Color.White;
            this.btnReload.Location = new System.Drawing.Point(841, 18);
            this.btnReload.Margin = new System.Windows.Forms.Padding(6);
            this.btnReload.MinimumSize = new System.Drawing.Size(150, 51);
            this.btnReload.Name = "btnReload";
            this.btnReload.Padding = new System.Windows.Forms.Padding(12, 3, 12, 3);
            this.btnReload.Size = new System.Drawing.Size(150, 51);
            this.btnReload.TabIndex = 4;
            this.btnReload.Text = "Tải lại";
            this.btnReload.UseVisualStyleBackColor = false;
            this.btnReload.Click += new System.EventHandler(this.Reload_Click);
            //
            // btnSave
            //
            this.btnSave.AutoSize = true;
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(103)))), ((int)(((byte)(192)))));
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(1003, 18);
            this.btnSave.Margin = new System.Windows.Forms.Padding(6);
            this.btnSave.MinimumSize = new System.Drawing.Size(150, 51);
            this.btnSave.Name = "btnSave";
            this.btnSave.Padding = new System.Windows.Forms.Padding(12, 3, 12, 3);
            this.btnSave.Size = new System.Drawing.Size(224, 51);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Lưu phiếu mượn";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.Save_Click);
            //
            // btnHistory
            //
            this.btnHistory.AutoSize = true;
            this.btnHistory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(103)))), ((int)(((byte)(192)))));
            this.btnHistory.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHistory.FlatAppearance.BorderSize = 0;
            this.btnHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHistory.ForeColor = System.Drawing.Color.White;
            this.btnHistory.Location = new System.Drawing.Point(1239, 18);
            this.btnHistory.Margin = new System.Windows.Forms.Padding(6);
            this.btnHistory.MinimumSize = new System.Drawing.Size(150, 51);
            this.btnHistory.Name = "btnHistory";
            this.btnHistory.Padding = new System.Windows.Forms.Padding(12, 3, 12, 3);
            this.btnHistory.Size = new System.Drawing.Size(166, 51);
            this.btnHistory.TabIndex = 6;
            this.btnHistory.Text = "Xem hồ sơ";
            this.btnHistory.UseVisualStyleBackColor = false;
            this.btnHistory.Click += new System.EventHandler(this.History_Click);
            //
            // note
            //
            this.note.AutoSize = true;
            this.note.Dock = System.Windows.Forms.DockStyle.Top;
            this.note.Location = new System.Drawing.Point(0, 163);
            this.note.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.note.Name = "note";
            this.note.Padding = new System.Windows.Forms.Padding(18);
            this.note.Size = new System.Drawing.Size(976, 64);
            this.note.TabIndex = 2;
            this.note.Text = "Đánh dấu các sách cần mượn (tối đa 3 cuốn đang mượn/thẻ). Thẻ phải còn hạn và sác" +
    "h phải còn trong kho.";
            //
            // frmCirculation
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1851, 1052);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.summary);
            this.Controls.Add(this.note);
            this.Controls.Add(this.actions);
            this.Controls.Add(this.header);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(1339, 902);
            this.Name = "frmCirculation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Lập hồ sơ mượn sách";
            this.Shown += new System.EventHandler(this.Circulation_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.header.ResumeLayout(false);
            this.header.PerformLayout();
            this.actions.ResumeLayout(false);
            this.actions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.deposit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        protected System.Windows.Forms.DataGridView grid;
        protected System.Windows.Forms.Label summary;
        protected System.Windows.Forms.FlowLayoutPanel header;
        protected System.Windows.Forms.Label lblOwner;
        protected System.Windows.Forms.ComboBox cardOrLoan;
        protected System.Windows.Forms.Label lblStaff;
        protected System.Windows.Forms.ComboBox librarian;
        protected System.Windows.Forms.Label lblDate;
        protected System.Windows.Forms.DateTimePicker date;
        protected System.Windows.Forms.FlowLayoutPanel actions;
        protected System.Windows.Forms.Label lblDeposit;
        protected System.Windows.Forms.NumericUpDown deposit;
        protected System.Windows.Forms.Label lblSearch;
        protected System.Windows.Forms.TextBox search;
        protected System.Windows.Forms.Button btnReload;
        protected System.Windows.Forms.Button btnSave;
        protected System.Windows.Forms.Button btnHistory;
        protected System.Windows.Forms.Label note;
        protected System.Windows.Forms.DataGridViewCheckBoxColumn gridChooseColumn;
        protected System.Windows.Forms.DataGridViewTextBoxColumn gridMaSachColumn;
        protected System.Windows.Forms.DataGridViewTextBoxColumn gridTenSachColumn;
        protected System.Windows.Forms.DataGridViewTextBoxColumn gridTenTheLoaiColumn;
        protected System.Windows.Forms.DataGridViewTextBoxColumn gridTenNXBColumn;
        protected System.Windows.Forms.DataGridViewTextBoxColumn gridSoLuongColumn;
        protected System.Windows.Forms.DataGridViewTextBoxColumn gridNgayMuonColumn;
        protected System.Windows.Forms.DataGridViewTextBoxColumn gridDonGiaThueColumn;
        protected System.Windows.Forms.DataGridViewComboBoxColumn gridViolationColumn;
        protected System.Windows.Forms.DataGridViewTextBoxColumn gridAmountColumn;
    }
}
