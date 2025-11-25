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
    /// <typeparam name="T">The type of the generic column</typeparam>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class GenericDependencyColumnAttribute<T> : DependencyColumnAttribute
    {
        /// <summary>
        /// An attribute indicate that column depends of another column's value
        /// </summary>
        /// <param name="columnName">Dependency column's name</param>
        /// <param name="dependentValue">The dependency column's value</param>
        public GenericDependencyColumnAttribute(string columnName, T dependentValue) : base (columnName, dependentValue)
        {
        }
    }
}
