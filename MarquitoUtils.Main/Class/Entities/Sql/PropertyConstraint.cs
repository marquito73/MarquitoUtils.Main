using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarquitoUtils.Main.Class.Entities.Sql
{
    public class PropertyConstraint
    {
        public string PropertyName { get; private set; }
        public object Value { get; private set; }

        public PropertyConstraint(string propertyName, object value)
        {
            this.PropertyName = propertyName;
            this.Value = value;
        }
    }
}
