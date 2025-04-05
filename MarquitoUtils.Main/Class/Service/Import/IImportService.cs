using MarquitoUtils.Main.Class.Entities.Sql;
using MarquitoUtils.Main.Class.Service.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarquitoUtils.Main.Class.Service.Import
{
    /// <summary>
    /// Import service, with inheritance of the Entity service, for manage import of entities
    /// </summary>
    public interface IImportService : IEntityService
    {
        /// <summary>
        /// Find an entity by his id
        /// </summary>
        /// <param name="id">The id</param>
        /// <param name="entityType">The entity's type</param>
        /// <returns>An entity found by his id</returns>
        public Entity? FindEntityById(int id, Type entityType);

        /// <summary>
        /// Find entity by unique constraint
        /// </summary>
        /// <param name="constraints">Constraints</param>
        /// <param name="entityType">The entity's type</param>
        /// <returns>Entity found by unique constraint</returns>
        public Entity FindEntityByUniqueConstraint(List<PropertyConstraint<Entity>> constraints, Type entityType);

        /// <summary>
        /// Persist an entity (find new id and add entity to DbContext)
        /// </summary>
        /// <param name="entity">The entity to persist</param>
        public void PersistEntity(Entity entity);

        /// <summary>
        /// Get entities of specific entity type
        /// </summary>
        /// <param name="entityType">The entity's type</param>
        /// <returns>Entities of specific entity type</returns>
        public IQueryable GetEntityList(Type entityType);
    }
}
