using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarquitoUtils.Main.Class.Entities.Sql.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DependentEntityAttribute<TEntity> : Attribute
        where TEntity : Entity, IEntity
    {
        public Type GetEntityType() 
        {
            return typeof(TEntity);
        }
    }
}
