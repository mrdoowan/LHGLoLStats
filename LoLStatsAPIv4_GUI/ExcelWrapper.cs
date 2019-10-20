using System;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.Win32.SafeHandles;

namespace LoLStatsAPIv4_GUI {
    public class ExcelWrapper : IDisposable
    {
        protected Excel.Application ExcelApp { get; set; }
        protected Excel.Workbook ExcelWorkBook { get; set; }
        protected Excel.Worksheet ExcelSheet { get; set; }
        private OpenFileDialog OFD_Excel;
        private SaveFileDialog SFD_Excel;
        public string FileName { get; private set; }

        // Ctor
        public ExcelWrapper() {
            ExcelApp = new Excel.Application();
            object misValue = System.Reflection.Missing.Value;
            ExcelWorkBook = ExcelApp.Workbooks.Add(misValue);
            ExcelSheet = null;
            OFD_Excel = new OpenFileDialog();
            SFD_Excel = new SaveFileDialog();
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        protected virtual void Dispose(bool disposing) {
            handle.Dispose();
            ReleaseObject(ExcelApp);
            ReleaseObject(ExcelWorkBook);
            ReleaseObject(ExcelSheet);
            Console.WriteLine("Excel Wrapper finalizer called");
        }

        // Clean up the COM objects
        protected void ReleaseObject(object obj) {
            try {
                Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch {
                obj = null;
            }
            finally {
                GC.Collect();
            }
        }

        public bool OpenExcelFile(string openFileDialogTitle) {
            OFD_Excel.Filter = "Excel Sheet (*.xlsx)|*.xlsx";
            OFD_Excel.Title = openFileDialogTitle;
            OFD_Excel.RestoreDirectory = true;
            if (OFD_Excel.ShowDialog() != DialogResult.OK) { return false; }

            // Initialize App and Workbook
            ReleaseObject(ExcelWorkBook);
            ExcelWorkBook = ExcelApp.Workbooks.Open(OFD_Excel.FileName);
            return true;
        }

        public bool SaveExcelFile(string saveFileDialogTitle) {
            SFD_Excel.Filter = "Excel Sheet (*.xlsx)|*.xlsx";
            SFD_Excel.Title = saveFileDialogTitle;
            SFD_Excel.OverwritePrompt = true;
            if (SFD_Excel.ShowDialog() != DialogResult.OK) { return false; }

            // Save workbook
            ExcelApp.DisplayAlerts = false;
            ExcelWorkBook.SaveAs(SFD_Excel.FileName);
            ExcelApp.Visible = true;
            return true;
        }

        // Initializes sheet by locating its Name
        public bool SelectExcelSheet(string sheetName) {
            ReleaseObject(ExcelSheet); // Release any current sheet
            for (int i = 1; i <= ExcelWorkBook.Worksheets.Count; ++i) {
                ExcelSheet = ExcelWorkBook.Worksheets.Item[i];
                if (sheetName == ExcelSheet.Name) { return true; }
                ReleaseObject(ExcelSheet);
            }
            // Couldn't find sheet
            MessageBox.Show("Opened Excel sheet does not have \"" + sheetName + "\" as its Sheet Name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        // Initializes sheet by Index
        public bool SelectExcelSheet(int index) {
            if (index >= ExcelWorkBook.Worksheets.Count || index <= 0) { return false; }
            ReleaseObject(ExcelSheet);
            ExcelSheet = ExcelWorkBook.Worksheets.Item[index];
            return true;
        }

        // Creating an Excel sheet
        public void MakeExcelSheet(string sheetName, int insertBefore) {
            var xlSheets = ExcelWorkBook.Sheets as Excel.Sheets;
            ExcelSheet = (Excel.Worksheet)xlSheets.Add(xlSheets[insertBefore], Type.Missing, Type.Missing, Type.Missing);
            ExcelSheet.Name = sheetName;
            ReleaseObject(xlSheets);
        }

        public string GetCell(int row, int column) {
            return ExcelSheet.Cells[row, column].Value2.ToString();
        }

        public void SetCellValue(int row, int column, string value) {
            ExcelSheet.Cells[row, column] = value;
        }

        public void SetCellBackgroundColor(int row, int column, string hex) {
            ExcelSheet.Cells[row, column].Interior.Color = ColorTranslator.ToOle(ColorTranslator.FromHtml(hex));
        }

        public void SetCellTextColor(int row, int column, string hex) {
            ExcelSheet.Cells[row, column].Font.Color = ColorTranslator.ToOle(ColorTranslator.FromHtml(hex));
        }

        public void SetCellBold(int row, int column, bool bold) {
            ExcelSheet.Cells[row, column].Font.Bold = bold;
        }

        public void SetCellItalic(int row, int column, bool italic) {
            ExcelSheet.Cells[row, column].Font.Italic = italic;
        }

        public void SetCellUnderline(int row, int column, bool underline) {
            ExcelSheet.Cells[row, column].Font.Underline = underline;
        }

        public void SetCellAlign(int row, int column, Align type) {
            Excel.XlHAlign align = (type == Align.LEFT) ? Excel.XlHAlign.xlHAlignLeft :
                (type == Align.MIDDLE) ? Excel.XlHAlign.xlHAlignCenter :
                (type == Align.RIGHT) ? Excel.XlHAlign.xlHAlignRight : 0;
            ExcelSheet.Cells[row, column].HorizontalAlignment = align;
        }

        public void SetColumnWidth(int column, dynamic width) {
            ExcelSheet.Columns[column].ColumnWidth = width;
        }

        // Return # of non-Empty Rows the sheet has
        // Grabbed from: https://stackoverflow.com/questions/43353073/c-sharp-excel-correct-way-to-get-rows-and-columns-count
        public int getNumRows() {
            return ExcelSheet.Cells.Find("*", System.Reflection.Missing.Value,
                System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlPrevious,
                false, System.Reflection.Missing.Value, System.Reflection.Missing.Value).Row;
        }

    }
}
