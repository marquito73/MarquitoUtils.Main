using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarquitoUtils.Main.Sql.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CanBeExportedImportedAttribute : Attribute
    {
        public bool CanBeExportedImported { get; private set; }

        public CanBeExportedImportedAttribute(bool canBeExportedImported)
        {
            this.CanBeExportedImported = canBeExportedImported;
        }
    }
}
