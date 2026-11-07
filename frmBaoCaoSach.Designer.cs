namespace QuanLyThuVienQuocGia
{
    partial class frmBaoCaoSach
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.rptvDanhSachSach = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.wbBaoCao = new System.Windows.Forms.WebBrowser();
            this.btnMau = new System.Windows.Forms.Button();
            this.btnIn = new System.Windows.Forms.Button();
            this.btnInAn = new System.Windows.Forms.Button();
            this.btnMoPdf = new System.Windows.Forms.Button();
            this.btnDoiCheDo = new System.Windows.Forms.Button();
            this.btnThoat = new System.Windows.Forms.Button();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.pnlTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // rptvDanhSachSach
            // 
            this.rptvDanhSachSach.ActiveViewIndex = -1;
            this.rptvDanhSachSach.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rptvDanhSachSach.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rptvDanhSachSach.Cursor = System.Windows.Forms.Cursors.Default;
            this.rptvDanhSachSach.Location = new System.Drawing.Point(14, 106);
            this.rptvDanhSachSach.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rptvDanhSachSach.Name = "rptvDanhSachSach";
            this.rptvDanhSachSach.ShowCloseButton = false;
            this.rptvDanhSachSach.ShowGroupTreeButton = false;
            this.rptvDanhSachSach.ShowParameterPanelButton = false;
            this.rptvDanhSachSach.Size = new System.Drawing.Size(1210, 731);
            this.rptvDanhSachSach.TabIndex = 8;
            this.rptvDanhSachSach.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            this.rptvDanhSachSach.Visible = false;
            // 
            // wbBaoCao
            // 
            this.wbBaoCao.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wbBaoCao.Location = new System.Drawing.Point(14, 106);
            this.wbBaoCao.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.wbBaoCao.MinimumSize = new System.Drawing.Size(22, 25);
            this.wbBaoCao.Name = "wbBaoCao";
            this.wbBaoCao.Size = new System.Drawing.Size(1210, 731);
            this.wbBaoCao.TabIndex = 7;
            // 
            // btnMau
            // 
            this.btnMau.Location = new System.Drawing.Point(0, 0);
            this.btnMau.Name = "btnMau";
            this.btnMau.Size = new System.Drawing.Size(75, 23);
            this.btnMau.TabIndex = 0;
            // 
            // btnIn
            // 
            this.btnIn.Location = new System.Drawing.Point(14, 18);
            this.btnIn.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnIn.Name = "btnIn";
            this.btnIn.Size = new System.Drawing.Size(214, 62);
            this.btnIn.TabIndex = 1;
            this.btnIn.Text = "TẢI LẠI TỪ CSDL";
            this.btnIn.UseVisualStyleBackColor = true;
            this.btnIn.Click += new System.EventHandler(this.btnIn_Click);
            // 
            // btnInAn
            // 
            this.btnInAn.Location = new System.Drawing.Point(238, 18);
            this.btnInAn.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnInAn.Name = "btnInAn";
            this.btnInAn.Size = new System.Drawing.Size(180, 62);
            this.btnInAn.TabIndex = 2;
            this.btnInAn.Text = "IN BÁO CÁO";
            this.btnInAn.UseVisualStyleBackColor = true;
            this.btnInAn.Click += new System.EventHandler(this.btnInAn_Click);
            // 
            // btnMoPdf
            // 
            this.btnMoPdf.Location = new System.Drawing.Point(430, 18);
            this.btnMoPdf.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnMoPdf.Name = "btnMoPdf";
            this.btnMoPdf.Size = new System.Drawing.Size(191, 62);
            this.btnMoPdf.TabIndex = 3;
            this.btnMoPdf.Text = "XUẤT RA PDF";
            this.btnMoPdf.UseVisualStyleBackColor = true;
            this.btnMoPdf.Click += new System.EventHandler(this.btnMoPdf_Click);
            // 
            // btnDoiCheDo
            // 
            this.btnDoiCheDo.Location = new System.Drawing.Point(632, 18);
            this.btnDoiCheDo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDoiCheDo.Name = "btnDoiCheDo";
            this.btnDoiCheDo.Size = new System.Drawing.Size(225, 62);
            this.btnDoiCheDo.TabIndex = 4;
            this.btnDoiCheDo.Text = "XEM DẠNG WEB A4";
            this.btnDoiCheDo.UseVisualStyleBackColor = true;
            this.btnDoiCheDo.Click += new System.EventHandler(this.btnDoiCheDo_Click);
            // 
            // btnThoat
            // 
            this.btnThoat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnThoat.Location = new System.Drawing.Point(1076, 18);
            this.btnThoat.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnThoat.Name = "btnThoat";
            this.btnThoat.Size = new System.Drawing.Size(146, 62);
            this.btnThoat.TabIndex = 6;
            this.btnThoat.Text = "THOÁT";
            this.btnThoat.UseVisualStyleBackColor = true;
            this.btnThoat.Click += new System.EventHandler(this.btnThoat_Click);
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.Transparent;
            this.pnlTop.Controls.Add(this.btnIn);
            this.pnlTop.Controls.Add(this.btnInAn);
            this.pnlTop.Controls.Add(this.btnMoPdf);
            this.pnlTop.Controls.Add(this.btnDoiCheDo);
            this.pnlTop.Controls.Add(this.btnThoat);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1238, 98);
            this.pnlTop.TabIndex = 0;
            // 
            // frmBaoCaoSach
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1238, 850);
            this.Controls.Add(this.wbBaoCao);
            this.Controls.Add(this.rptvDanhSachSach);
            this.Controls.Add(this.pnlTop);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmBaoCaoSach";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BÁO CÁO DANH SÁCH SÁCH";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmBaoCaoSach_FormClosing);
            this.Load += new System.EventHandler(this.frmBaoCaoSach_Load);
            this.pnlTop.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Button btnMau;
        private System.Windows.Forms.Button btnIn;
        private System.Windows.Forms.Button btnInAn;
        private System.Windows.Forms.Button btnMoPdf;
        private System.Windows.Forms.Button btnDoiCheDo;
        private System.Windows.Forms.Button btnThoat;
        private System.Windows.Forms.WebBrowser wbBaoCao;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer rptvDanhSachSach;
    }
}