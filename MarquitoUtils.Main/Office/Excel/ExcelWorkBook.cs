using MarquitoUtils.Main.Common.Tools;

namespace MarquitoUtils.Main.Office.Excel
{
    public sealed class ExcelWorkBook
    {
        public string Filename { get; set; }
        public ISet<ExcelSheet> Sheets { get; set; } = new HashSet<ExcelSheet>();

        public ExcelWorkBook(string filename)
        {
            Filename = filename;
        }

        public ExcelSheet GetSheet(string sheetName)
        {
            ExcelSheet sheet = this.Sheets.Where(row => row.Name == sheetName).FirstOrDefault();

            if (Utils.IsNull(sheet))
            {
                sheet = new ExcelSheet(this, sheetName);
                this.Sheets.Add(sheet);
            }

            return sheet;
        }
    }
}
