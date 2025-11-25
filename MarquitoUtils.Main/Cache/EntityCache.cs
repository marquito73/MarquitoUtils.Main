using MarquitoUtils.Main.Common.Tools;
using MarquitoUtils.Main.Sql.Entities;

namespace MarquitoUtils.Main.Cache
{
    /// <summary>
    /// A cache for store entities
    /// </summary>
    public class EntityCache : Cache<Entity>
    {
        /// <summary>
        /// Add entities inside the cache
        /// </summary>
        /// <param name="entityType">Entity type</param>
        /// <param name="entities">Entities to store inside the cache</param>
        public void AddEntities(Type entityType, List<Entity> entities)
        {
            this.CacheMap.Add(entityType, Utils.Nvl(entities));
        }

        /// <summary>
        /// Return entities from the cache
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <returns>Entities from the cache</returns>
        public List<TEntity> GetEntities<TEntity>() where TEntity : Entity
        {
            return Utils.Nvl(this.CacheMap[typeof(TEntity)]).Cast<TEntity>().ToList();
        }

        /// <summary>
        /// Return the first entity from the cache
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <returns>The first entity from the cache</returns>
        public TEntity? GetFirstEntity<TEntity>() where TEntity : Entity
        {
            return this.GetEntities<TEntity>().FirstOrDefault();
        }
    }
}
