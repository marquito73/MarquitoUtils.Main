using MarquitoUtils.Main.Class.Tools;
using MarquitoUtils.Main.Class.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace MarquitoUtils.Main.Class.Entities.Sql
{
    [Serializable]
    public class Entity : IEntity, IDisposable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }

        public Entity()
        {

        }

        public PropertyInfo GetPropertyInfo(string fieldName)
        {
            return this.GetType().GetProperty(fieldName);
        }

        public Type GetFieldType(string fieldName)
        {
            return this.GetPropertyInfo(fieldName).PropertyType;
        }

        public object GetFieldValue(string fieldName)
        {
            return this.GetPropertyInfo(fieldName).GetValue(this, null);
        }

        public DataType GetDataType(string fieldName)
        {
            DataType dataType = DataType.Custom;

            DataTypeAttribute attribute = this.GetPropertyInfo(fieldName).GetCustomAttribute<DataTypeAttribute>();

            if (Utils.IsNotNull(attribute))
            {
                dataType = attribute.DataType;
            }

            return dataType;
        }

        public PropertyAttributes GetFieldAttributes(string fieldName)
        {
            return this.GetPropertyInfo(fieldName).Attributes;
        }

        public string GetTableName()
        {
            return Utils.GetAsString(this.GetType().GetCustomAttributesData()
                .Where(attr => attr.AttributeType.IsAssignableFrom(typeof(TableAttribute)))
                .ToList().First().ConstructorArguments.First().Value);
        }

        public Guid GetEntityIdentityCode()
        {
            return Utils.GetIdentityCode(this);
        }

        public EnumContentType GetContentType(string columnName)
        {
            EnumContentType contentType;

            object value = this.GetFieldValue(columnName);

            switch (this.GetDataType(columnName))
            {
                case DataType.Password:
                    contentType = EnumContentType.Password;
                    break;
                case DataType.Currency:
                    contentType = EnumContentType.Currency;
                    break;
                case DataType.EmailAddress:
                    contentType = EnumContentType.EmailAddress;
                    break;
                case DataType.PhoneNumber:
                    contentType = EnumContentType.PhoneNumber;
                    break;
                case DataType.Upload:
                    contentType = EnumContentType.Binary;
                    break;
                case DataType.Custom:
                default:
                    switch (value)
                    {
                        case bool b:
                            contentType = EnumContentType.Boolean;
                            break;
                        case long l:
                            contentType = EnumContentType.Number;
                            break;
                        case int i:
                            contentType = EnumContentType.Number;
                            break;
                        case short s:
                            contentType = EnumContentType.Number;
                            break;
                        case string s:
                            contentType = EnumContentType.Text;
                            break;
                        default:
                            contentType = EnumContentType.Text;
                            break;
                    }
                    break;
            }

            return contentType;
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
