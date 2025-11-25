using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarquitoUtils.Main.Sql.Attributes
{
    /// <summary>
    /// An attribute indicate that column depends of another column's value
    /// </summary>
    public abstract class DependencyColumnAttribute : Attribute
    {
        /// <summary>
        /// Dependency column's name
        /// </summary>
        public string ColumnName { get; protected set; }
        /// <summary>
        /// The dependency column's value
        /// </summary>
        public object DependentValue { get; }
        /// <summary>
        /// An attribute indicate that column depends of another column's value
        /// </summary>
        /// <param name="columnName">Dependency column's name</param>
        /// <param name="dependentValue">The dependency column's value</param>
        public DependencyColumnAttribute(string columnName, object dependentValue)
        {
            this.ColumnName = columnName;
            this.DependentValue = dependentValue;
        }
    }
}
