using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace QuanLyThuVienQuocGia
{
    // Tuần 7, slides 4–10: Application -> Workbook -> Worksheet -> Range,
    // định dạng, Activate, SaveAs, đóng workbook và Quit ứng dụng Excel.
    internal static class ExcelExport
    {
        internal static void Write(DataTable data, string path, string title)
        {
            if (data == null || data.Rows.Count == 0)
                throw new InvalidOperationException("Không có dữ liệu để xuất.");
            if (data.Columns.Count + 1 > 16384 || data.Rows.Count > 1048569)
                throw new InvalidOperationException("Dữ liệu vượt quá số dòng hoặc cột Excel hỗ trợ.");
            if (!string.Equals(Path.GetExtension(path), ".xlsx", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Vui lòng lưu file với phần mở rộng .xlsx.");

            Excel.Application exApp = null;
            Excel.Workbooks books = null;
            Excel.Workbook exBook = null;
            Excel.Sheets sheets = null;
            Excel.Worksheet exSheet = null;
            Excel.Range range = null, columns = null, rows = null;
            Excel.Font font = null;
            Excel.Borders borders = null;
            string lastColumn = ColumnName(data.Columns.Count + 1);
            int lastRow = data.Rows.Count + 7;
            try
            {
                // Chỉ tạo và đóng instance riêng của chức năng xuất, không dùng
                // GetActiveObject để tránh đóng workbook Excel người dùng đang mở.
                exApp = new Excel.Application { Visible = false, DisplayAlerts = false, ScreenUpdating = false };
                books = exApp.Workbooks;
                exBook = books.Add(Excel.XlWBATemplate.xlWBATWorksheet);
                sheets = exBook.Worksheets;
                exSheet = (Excel.Worksheet)sheets[1];
                exSheet.Name = "Bao cao thu vien";

                range = exSheet.get_Range("A1", lastColumn + lastRow);
                font = range.Font;
                font.Name = "Times New Roman";
                font.Size = 10;
                Release(ref font);
                range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                Release(ref range);

                range = exSheet.get_Range("A1", lastColumn + "3");
                font = range.Font;
                font.Bold = true;
                font.Color = ColorTranslator.ToOle(Color.Blue);
                Release(ref font);
                Release(ref range);
                WriteMerged(exSheet, "A1", lastColumn + "1", "QUẢN LÝ THƯ VIỆN QUỐC GIA");
                WriteMerged(exSheet, "A2", lastColumn + "2", "Nội dung: " + title);
                WriteMerged(exSheet, "A3", lastColumn + "3", "Ngày xuất: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"));

                range = exSheet.get_Range("A5", lastColumn + "5");
                range.Merge(false);
                range.Value2 = title.ToUpper(CultureInfo.GetCultureInfo("vi-VN"));
                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                range.RowHeight = 30;
                font = range.Font;
                font.Size = 14;
                font.Bold = true;
                font.Color = ColorTranslator.ToOle(Color.Red);
                Release(ref font);
                Release(ref range);

                var headers = new object[1, data.Columns.Count + 1];
                headers[0, 0] = "STT";
                for (int c = 0; c < data.Columns.Count; c++) headers[0, c + 1] = data.Columns[c].ColumnName;
                range = exSheet.get_Range("A7", lastColumn + "7");
                range.NumberFormat = "@";
                range.Value2 = headers;
                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                range.RowHeight = 26;
                font = range.Font;
                font.Bold = true;
                font.Size = 12;
                Release(ref font);
                Release(ref range);

                // Đặt kiểu trước khi ghi để tên bắt đầu bằng '=' vẫn là văn bản,
                // còn ngày và tiền giữ giá trị số, có thể tính toán trong Excel.
                for (int c = 0; c < data.Columns.Count; c++)
                {
                    string column = ColumnName(c + 2);
                    range = exSheet.get_Range(column + "8", column + lastRow);
                    Type type = data.Columns[c].DataType;
                    if (type == typeof(string)) range.NumberFormat = "@";
                    else if (type == typeof(DateTime)) range.NumberFormat = "dd/mm/yyyy";
                    else if (type == typeof(decimal) || type == typeof(double) || type == typeof(float)) range.NumberFormat = "#,##0.00";
                    Release(ref range);
                }
                var values = new object[data.Rows.Count, data.Columns.Count + 1];
                for (int r = 0; r < data.Rows.Count; r++)
                {
                    values[r, 0] = r + 1;
                    for (int c = 0; c < data.Columns.Count; c++)
                    {
                        object value = data.Rows[r][c];
                        values[r, c + 1] = value == DBNull.Value ? null : value is DateTime
                            ? (object)((DateTime)value).ToOADate() : value is decimal ? Convert.ToDouble(value) : value;
                    }
                }
                range = exSheet.get_Range("A8", lastColumn + lastRow);
                range.Value2 = values;
                Release(ref range);

                range = exSheet.get_Range("A7", lastColumn + lastRow);
                borders = range.Borders;
                borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                borders.Weight = Excel.XlBorderWeight.xlThin;
                Release(ref borders);
                columns = range.Columns;
                columns.AutoFit();
                Release(ref columns);
                Release(ref range);
                range = exSheet.get_Range("A:A", Type.Missing);
                range.ColumnWidth = 7;
                Release(ref range);
                for (int c = 0; c < data.Columns.Count; c++)
                {
                    string column = ColumnName(c + 2);
                    range = exSheet.get_Range(column + ":" + column, Type.Missing);
                    double width = Convert.ToDouble(range.ColumnWidth);
                    range.ColumnWidth = Math.Max(12, Math.Min(50, width));
                    Release(ref range);
                }
                range = exSheet.get_Range("A7", lastColumn + lastRow);
                range.WrapText = true;
                rows = range.Rows;
                rows.AutoFit();
                Release(ref rows);
                Release(ref range);

                exBook.Activate();
                exBook.SaveAs(Path.GetFullPath(path), Excel.XlFileFormat.xlOpenXMLWorkbook);
            }
            catch (COMException ex) when ((uint)ex.ErrorCode == 0x80040154)
            {
                throw new InvalidOperationException("Chức năng xuất Excel theo bài giảng cần Microsoft Excel cài trên máy. Vui lòng cài hoặc sửa cài đặt Excel rồi thử lại.", ex);
            }
            finally
            {
                Release(ref font); Release(ref borders); Release(ref columns); Release(ref rows); Release(ref range);
                try { if (exBook != null) exBook.Close(false); }
                finally
                {
                    try { if (exApp != null) exApp.Quit(); }
                    finally { Release(ref exSheet); Release(ref sheets); Release(ref exBook); Release(ref books); Release(ref exApp); }
                }
            }
        }
        private static void WriteMerged(Excel.Worksheet sheet, string first, string last, string text)
        {
            Excel.Range range = null;
            try { range = sheet.get_Range(first, last); range.Merge(false); range.NumberFormat = "@"; range.Value2 = text; }
            finally { Release(ref range); }
        }
        private static string ColumnName(int index)
        {
            string name = "";
            while (index > 0) { index--; name = (char)('A' + index % 26) + name; index /= 26; }
            return name;
        }
        private static void Release<T>(ref T value) where T : class
        {
            if (value != null && Marshal.IsComObject(value)) Marshal.FinalReleaseComObject(value);
            value = null;
        }
    }
}
