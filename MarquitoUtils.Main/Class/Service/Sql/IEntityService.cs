﻿using MarquitoUtils.Main.Class.Cache;
using MarquitoUtils.Main.Class.Entities.Sql;
using MarquitoUtils.Main.Class.Service.General;
using MarquitoUtils.Main.Class.Sql;

namespace MarquitoUtils.Main.Class.Service.Sql
{
    /// <summary>
    /// Entity service, for get, update, insert or delete entities
    /// </summary>
    public interface IEntityService : DefaultService
    {
        /// <summary>
        /// The database context, contains DbSets
        /// </summary>
        public DefaultDbContext DbContext { get; set; }
        /// <summary>
        /// Entities stored in cache
        /// </summary>
        public EntityCache EntityCache { get; set; }

        /// <summary>
        /// Find an entity by his id
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <param name="id">The id</param>
        /// <returns>An entity found by his id</returns>
        public T? FindEntityById<T>(int id, bool ignoreCache = false) 
            where T : Entity, IEntity;

        /// <summary>
        /// Find entities by ids
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <param name="ids">Ids</param>
        /// <returns>Entities found by ids</returns>
        public List<T> FindEntitiesByIds<T>(List<int> ids, bool ignoreCache = false)
            where T : Entity, IEntity;

        /// <summary>
        /// Find entity by unique constraint
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <param name="constraints">Constraints</param>
        /// <returns>Entity found by unique constraint</returns>
        public T? FindEntityByUniqueConstraint<T>(List<PropertyConstraint<T>> constraints, bool ignoreCache = false)
            where T : Entity, IEntity;

        /// <summary>
        /// Find entity by unique constraint
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <param name="constraints">Constraints</param>
        /// <returns>Entity found by unique constraint</returns>
        public T? FindEntityByUniqueConstraint<T>(bool ignoreCache = false, params PropertyConstraint<T>[] constraints)
            where T : Entity, IEntity;

        /// <summary>
        /// Check if an entity match with constraints
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <param name="entity">The entity</param>
        /// <param name="constraints">Constraints</param>
        /// <returns>The entity match with constraints ?</returns>
        public bool MatchUniqueConstraint<T>(T entity, params PropertyConstraint<T>[] constraints)
            where T : Entity, IEntity;

        /// <summary>
        /// Persist an entity (find new id and add entity to DbContext)
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <param name="entity">The entity to persist</param>
        public void PersistEntity<T>(T entity) 
            where T : Entity, IEntity;

        /// <summary>
        /// Get entities of specific entity type
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <returns>Entities of specific entity type</returns>
        public List<T> GetEntityList<T>(bool ignoreCache = false) 
            where T : Entity, IEntity;

        /// <summary>
        /// Get entities of specific entity type
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <param name="filters">Filters</param>
        /// <param name="includes">Includes</param>
        /// <returns>Entities of specific entity type</returns>
        public List<T> GetEntityList<T>(List<Func<T, bool>> filters, ISet<string> includes, bool ignoreCache = false) 
            where T : Entity, IEntity;
        /// <summary>
        /// Flush data to the database, if any error happen, transaction will be rollback
        /// </summary>
        /// <param name="exception"></param>
        /// <returns>Return true if it's works without any error</returns>
        public bool FlushData(out Exception exception);
    }
}
