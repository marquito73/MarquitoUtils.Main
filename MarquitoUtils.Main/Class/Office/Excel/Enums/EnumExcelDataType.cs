using MarquitoUtils.Main.Class.Office.Excel.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarquitoUtils.Main.Class.Office.Excel.Enums
{
    public enum EnumExcelDataType
    {
        [ExcelDataType<string>]
        String,
    }
}
