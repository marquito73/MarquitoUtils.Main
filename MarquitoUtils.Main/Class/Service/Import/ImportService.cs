using MarquitoUtils.Main.Class.Entities.Sql;
using MarquitoUtils.Main.Class.Service.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MarquitoUtils.Main.Class.Service.Import
{
    /// <summary>
    /// Import service, with inheritance of the Entity service, for manage import of entities
    /// </summary>
    public class ImportService : EntityService, IImportService
    {
        public Entity? FindEntityById(int id, Type entityType)
        {
            // Get the generic type definition
            MethodInfo method = base.GetType().GetMethods()
                .Where(method => method.Name.Equals(nameof(base.FindEntityById)))
                .Where(method => method.IsGenericMethod).First();

            // Build a method with the specific type argument you're interested in
            method = method.MakeGenericMethod(entityType);

            return (method.Invoke(this, new object[] { id }) as Entity);
        }

        public Entity FindEntityByUniqueConstraint(List<PropertyConstraint> constraints, Type entityType)
        {
            // Get the generic type definition
            MethodInfo method = base.GetType().GetMethods()
                .Where(method => method.Name.Equals(nameof(base.FindEntityByUniqueConstraint)))
                .Where(method => method.IsGenericMethod).First();

            // Build a method with the specific type argument you're interested in
            method = method.MakeGenericMethod(entityType);

            return (method.Invoke(this, new object[] { constraints }) as Entity);
        }

        public void PersistEntity(Entity entity)
        {
            // Get the generic type definition
            MethodInfo method = base.GetType().GetMethods()
                .Where(method => method.Name.Equals(nameof(base.PersistEntity)))
                .Where(method => method.IsGenericMethod).First();

            // Build a method with the specific type argument you're interested in
            method = method.MakeGenericMethod(entity.GetType());

            method.Invoke(this, new object[] { entity });
        }

        public IQueryable GetEntityList(Type entityType)
        {
            // Get the generic type definition
            MethodInfo method = this.DbContext.GetType().GetMethods()
                .Where(method => method.Name.Equals(nameof(this.DbContext.Set))).First();

            // Build a method with the specific type argument you're interested in
            method = method.MakeGenericMethod(entityType);

            return (method.Invoke(this.DbContext, null) as IQueryable);
        }
    }
}
