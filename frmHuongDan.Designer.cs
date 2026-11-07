namespace QuanLyThuVienQuocGia
{
    partial class frmHuongDan
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblPhuDe = new System.Windows.Forms.Label();
            this.lblTieuDe = new System.Windows.Forms.Label();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnIn = new System.Windows.Forms.Button();
            this.btnDong = new System.Windows.Forms.Button();
            this.wbNoiDung = new System.Windows.Forms.WebBrowser();
            this.pnlTop.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(252)))));
            this.pnlTop.Controls.Add(this.lblPhuDe);
            this.pnlTop.Controls.Add(this.lblTieuDe);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1148, 100);
            this.pnlTop.TabIndex = 0;
            // 
            // lblPhuDe
            // 
            this.lblPhuDe.AutoSize = true;
            this.lblPhuDe.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblPhuDe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(105)))), ((int)(((byte)(120)))));
            this.lblPhuDe.Location = new System.Drawing.Point(29, 60);
            this.lblPhuDe.Name = "lblPhuDe";
            this.lblPhuDe.Size = new System.Drawing.Size(638, 28);
            this.lblPhuDe.TabIndex = 1;
            this.lblPhuDe.Text = "Hệ thống Quản lý Thư viện Quốc gia - Hướng dẫn chi tiết các chức năng";
            // 
            // lblTieuDe
            // 
            this.lblTieuDe.AutoSize = true;
            this.lblTieuDe.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTieuDe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
            this.lblTieuDe.Location = new System.Drawing.Point(27, 15);
            this.lblTieuDe.Name = "lblTieuDe";
            this.lblTieuDe.Size = new System.Drawing.Size(497, 45);
            this.lblTieuDe.TabIndex = 0;
            this.lblTieuDe.Text = "SỔ TAY HƯỚNG DẪN SỬ DỤNG";
            this.lblTieuDe.Click += new System.EventHandler(this.lblTieuDe_Click);
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(248)))), ((int)(((byte)(252)))));
            this.pnlBottom.Controls.Add(this.btnIn);
            this.pnlBottom.Controls.Add(this.btnDong);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 813);
            this.pnlBottom.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(1148, 81);
            this.pnlBottom.TabIndex = 2;
            // 
            // btnIn
            // 
            this.btnIn.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnIn.Location = new System.Drawing.Point(27, 15);
            this.btnIn.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnIn.Name = "btnIn";
            this.btnIn.Size = new System.Drawing.Size(180, 52);
            this.btnIn.TabIndex = 0;
            this.btnIn.Text = "IN HƯỚNG DẪN";
            this.btnIn.UseVisualStyleBackColor = true;
            this.btnIn.Click += new System.EventHandler(this.btnIn_Click);
            // 
            // btnDong
            // 
            this.btnDong.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDong.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnDong.Location = new System.Drawing.Point(979, 15);
            this.btnDong.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDong.Name = "btnDong";
            this.btnDong.Size = new System.Drawing.Size(142, 52);
            this.btnDong.TabIndex = 1;
            this.btnDong.Text = "ĐÓNG";
            this.btnDong.UseVisualStyleBackColor = true;
            this.btnDong.Click += new System.EventHandler(this.btnDong_Click);
            // 
            // wbNoiDung
            // 
            this.wbNoiDung.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wbNoiDung.Location = new System.Drawing.Point(0, 100);
            this.wbNoiDung.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.wbNoiDung.MinimumSize = new System.Drawing.Size(22, 25);
            this.wbNoiDung.Name = "wbNoiDung";
            this.wbNoiDung.Size = new System.Drawing.Size(1148, 713);
            this.wbNoiDung.TabIndex = 1;
            // 
            // frmHuongDan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1148, 894);
            this.Controls.Add(this.wbNoiDung);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlTop);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MinimumSize = new System.Drawing.Size(897, 736);
            this.Name = "frmHuongDan";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "HƯỚNG DẪN SỬ DỤNG - QUẢN LÝ THƯ VIỆN QUỐC GIA";
            this.Load += new System.EventHandler(this.frmHuongDan_Load);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblTieuDe;
        private System.Windows.Forms.Label lblPhuDe;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Button btnIn;
        private System.Windows.Forms.Button btnDong;
        private System.Windows.Forms.WebBrowser wbNoiDung;
    }
}
