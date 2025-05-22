using MarquitoUtils.Main.Class.Entities.Image;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarquitoUtils.Main.Class.Attributes.Sql
{
    /// <summary>
    /// Column for Entity Framework usage
    /// </summary>
    /// <typeparam name="T">The generic type to define the SQL type</typeparam>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class GenericColumnAttribute<T> : ColumnAttribute
    {
        /// <summary>
        /// Column for Entity Framework usage
        /// </summary>
        /// <param name="name">The SQL column name</param>
        /// <param name="isKey">This column is an ID for a table ? (a bigint)</param>
        public GenericColumnAttribute(string name, bool isKey) : base(name)
        {
            if (isKey)
            {
                this.TypeName = "bigint";
            }
            else if (typeof(T).IsEnum)
            {
                this.TypeName = "tinyint";
            }
            else
            {
                switch (typeof(T).Name)
                {
                    case nameof(String):
                        this.TypeName = "nvarchar";
                        break;
                    case nameof(Double):
                    case nameof(Single):
                        this.TypeName = "float";
                        break;
                    case nameof(Int32):
                        this.TypeName = "int";
                        break;
                    case nameof(DateTime):
                        this.TypeName = typeof(T).Name.ToLower();
                        break;
                    case nameof(ImageData):
                        this.TypeName = "varbinary";
                        break;
                    default:
                        this.TypeName = typeof(T).Name.ToLower();
                        break;
                }
            }
        }

        /// <summary>
        /// Column for Entity Framework usage
        /// </summary>
        /// <param name="name">The SQL column name</param>
        public GenericColumnAttribute(string name) : this(name, false)
        {
        }
        public GenericColumnAttribute(string name, string typeName) : base(name)
        {
            this.TypeName = typeName;
        }
    }
}
