using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;

namespace SSTOverview
{
    public class ExcelWriter : XSSFWorkbook, IWorkbook
    {
        private string _directory;
        private string _fileName;
        
        public ExcelWriter(string directory, string fileName, string extension)
        {
            _fileName = fileName + "." + extension;
            _directory = directory + "\\" + _fileName;
        }

        public void CreateCellValue(string sheetName, int row, int column, string value)
        {
            ISheet sheet = this.GetSheet(sheetName);
            if (sheet == null) sheet = CreateSheet(sheetName);

            IRow cellRow = sheet.GetRow(row);
            if (cellRow == null) cellRow = sheet.CreateRow(row);

            ICell cell = cellRow.GetCell(column);
            if (cell == null) cell = cellRow.CreateCell(column);

            cell.SetCellValue(value);
        }

        public void SaveExcelFile()
        {
            using (var stream = new FileStream(_directory, FileMode.Create, FileAccess.Write))
            {
                this.Write(stream);
            }
        }
    }
}
