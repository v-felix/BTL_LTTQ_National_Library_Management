using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace QuanLyThuVienQuocGia
{
    /// <summary>
    /// Giao diện dùng chung cho toàn bộ chương trình (nền bg.png, font Segoe UI,
    /// nút bo góc, bảng dữ liệu, thanh trạng thái, menu).
    /// Gọi UiTheme.Apply(this) ngay sau InitializeComponent() của mỗi form.
    /// </summary>
    internal static class UiTheme
    {
        public static readonly Color Primary = Color.FromArgb(0, 103, 192);
        public static readonly Color PrimaryHover = Color.FromArgb(0, 120, 215);
        public static readonly Color PrimaryDown = Color.FromArgb(0, 84, 160);
        public static readonly Color Navy = Color.FromArgb(18, 32, 64);
        public static readonly Color Border = Color.FromArgb(196, 202, 214);
        public static readonly Color Surface = Color.FromArgb(250, 251, 254);

        private const string FontName = "Segoe UI";
        private static Image _background;

        // Biểu tượng (mã glyph font Segoe MDL2 Assets) cho từng nút theo tên control
        private static readonly Dictionary<string, string> Icons = new Dictionary<string, string>
        {
            { "btnThem", "\uE710" },
            { "btnLuu", "\uE74E" },
            { "btnSua", "\uE70F" },
            { "btnXoa", "\uE74D" },
            { "btnLamMoi", "\uE72C" },
            { "btnThoat", "\uE7E8" },
            { "btnXuatExcel", "\uE896" },
            { "btnIn", "\uE8E5" },
            { "btnMau", "\uE7C3" },
            { "btnInAn", "\uE749" },
            { "btnMoPdf", "\uEA90" },
            { "btnDoiCheDo", "\uE8AB" },
        };

        /// <summary>Ảnh nền bg.png (nhúng trong exe, có dự phòng đọc file cạnh exe).</summary>
        public static Image Background
        {
            get
            {
                if (_background != null) return _background;
                try
                {
                    using (Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream("bg.png"))
                    {
                        if (s != null)
                        {
                            _background = Scale(Image.FromStream(s));
                            return _background;
                        }
                    }
                    string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bg.png");
                    if (File.Exists(path))
                        _background = Scale(Image.FromFile(path));
                }
                catch { /* không có ảnh nền thì dùng nền màu */ }
                return _background;
            }
        }

        /// <summary>Thu nhỏ ảnh một lần (tối đa 1600px) để việc vẽ nền khi đổi kích thước nhanh hơn.</summary>
        private static Image Scale(Image src)
        {
            const int maxW = 1600;
            int w = Math.Min(maxW, src.Width);
            int h = (int)((long)src.Height * w / src.Width);
            Bitmap bmp = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(src, 0, 0, w, h);
            }
            src.Dispose();
            return bmp;
        }

        public static Font F(float size, FontStyle style = FontStyle.Regular)
        {
            return new Font(FontName, size, style, GraphicsUnit.Point);
        }

        /// <summary>Áp dụng giao diện cho một form và toàn bộ control con.</summary>
        public static void Apply(Form form)
        {
            form.Font = F(11F);
            form.BackColor = Color.FromArgb(232, 240, 250);
            form.BackgroundImage = Background;
            form.BackgroundImageLayout = ImageLayout.Stretch;
            form.MinimumSize = new Size(form.Width, form.Height);
            EnableDoubleBuffer(form);
            Style(form);
        }

        private static void Style(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is Label) StyleLabel((Label)c);
                else if (c is TextBox) StyleTextBox((TextBox)c);
                else if (c is Button) StyleButton((Button)c);
                else if (c is DataGridView) StyleGrid((DataGridView)c);
                else if (c is MenuStrip) StyleMenu((MenuStrip)c);
                else if (c is StatusStrip) StyleStatus((StatusStrip)c);

                if (c.HasChildren && !(c is DataGridView) && !IsThirdParty(c)) Style(c);
            }
        }

        /// <summary>Control của thư viện ngoài (Crystal Reports...) tự quản lý giao diện, không được đụng vào.</summary>
        private static bool IsThirdParty(Control c)
        {
            string ns = c.GetType().Namespace ?? "";
            return ns.StartsWith("CrystalDecisions", StringComparison.Ordinal);
        }

        // ---------------------------------------------------------------- Label
        private static void StyleLabel(Label l)
        {
            l.AutoSize = true;
            l.BackColor = Color.Transparent;
            l.ForeColor = Navy;
            l.Font = F(13F, FontStyle.Bold);
        }

        // ------------------------------------------------------------- TextBox
        private static void StyleTextBox(TextBox t)
        {
            t.BorderStyle = BorderStyle.FixedSingle;
            t.BackColor = Surface;
            t.ForeColor = Navy;
            t.Font = F(12F);
            t.Enter += delegate { t.BackColor = Color.White; };
            t.Leave += delegate { t.BackColor = Surface; };
        }

        // -------------------------------------------------------------- Button
        private static void StyleButton(Button b)
        {
            bool primary = b.Name == "btnThem" || b.Name == "btnIn" || b.Name == "btnDocDuLieu";

            b.UseVisualStyleBackColor = false;
            b.FlatStyle = FlatStyle.Flat;
            b.Cursor = Cursors.Hand;
            b.Font = F(12F);
            b.TextAlign = ContentAlignment.MiddleCenter;
            b.Text = ToTitle(b.Text);

            if (b.Name == "btnThoat" || (b.Text != null && b.Text.IndexOf("thoát", StringComparison.OrdinalIgnoreCase) >= 0))
            {
                b.CausesValidation = false;
            }

            string icon;
            if (Icons.TryGetValue(b.Name, out icon))
            {
                // Chừa chỗ bên trái cho biểu tượng rồi vẽ glyph ngay trước chữ
                b.Padding = new Padding(30, 0, 0, 0);
                Font iconFont = new Font("Segoe MDL2 Assets", 14F, GraphicsUnit.Point);
                b.Paint += delegate(object s, PaintEventArgs e)
                {
                    Size t = TextRenderer.MeasureText(e.Graphics, b.Text, b.Font);
                    int x = (b.Width + b.Padding.Left - t.Width) / 2 - 30;
                    Rectangle r = new Rectangle(Math.Max(6, x), 0, 28, b.Height);
                    TextRenderer.DrawText(e.Graphics, icon, iconFont, r, b.ForeColor,
                        TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.NoPadding);
                };
            }

            if (primary)
            {
                b.BackColor = Primary;
                b.ForeColor = Color.White;
                b.FlatAppearance.BorderColor = PrimaryDown;
                b.FlatAppearance.MouseOverBackColor = PrimaryHover;
                b.FlatAppearance.MouseDownBackColor = PrimaryDown;
            }
            else
            {
                b.BackColor = Surface;
                b.ForeColor = Navy;
                b.FlatAppearance.BorderColor = Border;
                b.FlatAppearance.MouseOverBackColor = Color.FromArgb(226, 238, 252);
                b.FlatAppearance.MouseDownBackColor = Color.FromArgb(204, 224, 247);
            }

            b.SizeChanged += delegate { Round(b, 10); };
            Round(b, 10);
        }

        /// <summary>"ĐỌC DỮ LIỆU" -> "Đọc dữ liệu" cho đồng nhất, dễ đọc.</summary>
        private static string ToTitle(string s)
        {
            if (string.IsNullOrEmpty(s) || s != s.ToUpperInvariant()) return s;
            s = s.ToLowerInvariant();
            return char.ToUpperInvariant(s[0]) + s.Substring(1);
        }

        private static void Round(Control c, int radius)
        {
            if (c.Width <= 0 || c.Height <= 0) return;
            using (GraphicsPath p = new GraphicsPath())
            {
                int d = radius * 2;
                Rectangle r = new Rectangle(0, 0, c.Width, c.Height);
                p.AddArc(r.X, r.Y, d, d, 180, 90);
                p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
                p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
                p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
                p.CloseFigure();
                c.Region = new Region(p);
            }
        }

        // ------------------------------------------------------- DataGridView
        public static void StyleGrid(DataGridView g)
        {
            g.BorderStyle = BorderStyle.None;
            g.BackgroundColor = Color.White;
            g.GridColor = Color.FromArgb(226, 230, 238);
            g.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            g.RowHeadersVisible = false;
            g.EnableHeadersVisualStyles = false;
            g.AllowUserToResizeRows = false;
            g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            g.MultiSelect = false;
            g.AllowUserToAddRows = false;
            g.AllowUserToDeleteRows = false;
            g.ReadOnly = true;
            g.EditMode = DataGridViewEditMode.EditProgrammatically;
            g.DataError += (s, ev) => { ev.ThrowException = false; ev.Cancel = false; };
            g.ScrollBars = ScrollBars.Both;
            g.Font = F(11.5F);

            g.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            g.ColumnHeadersHeight = 42;
            g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            g.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(240, 244, 251);
            g.ColumnHeadersDefaultCellStyle.ForeColor = Navy;
            g.ColumnHeadersDefaultCellStyle.Font = F(12F, FontStyle.Bold);
            g.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(240, 244, 251);
            g.ColumnHeadersDefaultCellStyle.SelectionForeColor = Navy;
            g.ColumnHeadersDefaultCellStyle.Padding = new Padding(6, 0, 0, 0);

            g.RowTemplate.Height = 36;
            g.DefaultCellStyle.BackColor = Color.White;
            g.DefaultCellStyle.ForeColor = Navy;
            g.DefaultCellStyle.SelectionBackColor = Color.FromArgb(196, 224, 248);
            g.DefaultCellStyle.SelectionForeColor = Navy;
            g.DefaultCellStyle.Padding = new Padding(6, 0, 0, 0);
            g.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(246, 249, 254);
            g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        /// <summary>Đặt tiêu đề tiếng Việt cho các cột (gọi lại sau mỗi lần nạp dữ liệu).</summary>
        public static void SetBookHeaders(DataGridView g)
        {
            Dictionary<string, string> h = new Dictionary<string, string>
            {
                { "MaSach", "Mã sách" }, { "TenSach", "Tên sách" }, { "TenTG", "Tác giả" },
                { "NhaXuatBan", "Nhà xuất bản" }, { "TenTheLoai", "Thể loại" }, { "TenNgonNgu", "Ngôn ngữ" }
            };
            foreach (KeyValuePair<string, string> kv in h)
                if (g.Columns.Contains(kv.Key)) g.Columns[kv.Key].HeaderText = kv.Value;

            if (g.Columns.Contains("MaSach")) g.Columns["MaSach"].FillWeight = 60;
            if (g.Columns.Contains("TenSach")) g.Columns["TenSach"].FillWeight = 160;
            g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // --------------------------------------------------------- StatusStrip
        private static void StyleStatus(StatusStrip s)
        {
            s.SizingGrip = true;
            s.BackColor = Color.FromArgb(240, 244, 251);
            s.ForeColor = Navy;
            s.Font = F(10.5F);
            foreach (ToolStripItem i in s.Items) i.ForeColor = Navy;
        }

        /// <summary>Thêm thanh trạng thái "n bản ghi" gắn với một DataGridView.</summary>
        public static StatusStrip AddRecordCounter(Form form, DataGridView grid)
        {
            StatusStrip strip = new StatusStrip();
            ToolStripStatusLabel lbl = new ToolStripStatusLabel();
            strip.Items.Add(lbl);
            StyleStatus(strip);
            form.Controls.Add(strip);

            Action update = delegate { lbl.Text = grid.Rows.Count + " bản ghi"; };
            grid.DataSourceChanged += delegate { update(); };
            grid.RowsAdded += delegate { update(); };
            grid.RowsRemoved += delegate { update(); };
            update();
            return strip;
        }

        // ------------------------------------------------------------ MenuStrip
        private static void StyleMenu(MenuStrip m)
        {
            m.Renderer = new ToolStripProfessionalRenderer(new MenuColors());
            m.BackColor = Color.FromArgb(0, 82, 155);
            m.ForeColor = Color.White;
            m.Font = F(11F, FontStyle.Bold);
            m.Padding = new Padding(10, 6, 10, 6);
            foreach (ToolStripItem item in m.Items) StyleMenuItem(item, true);
        }

        private static void StyleMenuItem(ToolStripItem item, bool top)
        {
            item.Font = top ? F(11F, FontStyle.Bold) : F(11F);
            item.ForeColor = top ? Color.White : Navy;
            item.Padding = top ? new Padding(10, 4, 10, 4) : new Padding(4);

            ToolStripMenuItem mi = item as ToolStripMenuItem;
            if (mi == null) return;
            mi.DropDown.BackColor = Color.White;
            foreach (ToolStripItem child in mi.DropDownItems) StyleMenuItem(child, false);
        }

        private sealed class MenuColors : ProfessionalColorTable
        {
            public MenuColors() { UseSystemColors = false; }
            public override Color MenuItemSelected { get { return Color.FromArgb(226, 238, 252); } }
            public override Color MenuItemSelectedGradientBegin { get { return Color.FromArgb(226, 238, 252); } }
            public override Color MenuItemSelectedGradientEnd { get { return Color.FromArgb(226, 238, 252); } }
            public override Color MenuItemPressedGradientBegin { get { return Color.FromArgb(0, 103, 192); } }
            public override Color MenuItemPressedGradientEnd { get { return Color.FromArgb(0, 103, 192); } }
            public override Color MenuItemBorder { get { return Color.FromArgb(196, 224, 248); } }
            public override Color MenuBorder { get { return Color.FromArgb(196, 202, 214); } }
            public override Color ToolStripDropDownBackground { get { return Color.White; } }
            public override Color ImageMarginGradientBegin { get { return Color.White; } }
            public override Color ImageMarginGradientMiddle { get { return Color.White; } }
            public override Color ImageMarginGradientEnd { get { return Color.White; } }
            public override Color MenuStripGradientBegin { get { return Color.FromArgb(0, 82, 155); } }
            public override Color MenuStripGradientEnd { get { return Color.FromArgb(0, 82, 155); } }
        }

        // --------------------------------------------------------------- Khác
        private static void EnableDoubleBuffer(Control c)
        {
            typeof(Control).GetProperty("DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic).SetValue(c, true, null);
        }
    }
}
