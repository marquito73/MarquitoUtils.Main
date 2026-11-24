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
        /// Add entity inside the cache
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity</typeparam>
        /// <param name="entity">The entity to add</param>
        public void AddEntity<TEntity>(TEntity entity) where TEntity : Entity
        {
            if (this.CacheMap.ContainsKey(typeof(TEntity)))
            {
                this.CacheMap[typeof(TEntity)].Add(entity);
            }
            else
            {
                this.CacheMap.Add(typeof(TEntity), new List<Entity>() { entity });
            }
        }

        /// <summary>
        /// Add entities inside the cache
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity</typeparam>
        /// <param name="entities">Entities to store inside the cache</param>
        public void AddEntities<TEntity>(List<TEntity> entities) where TEntity : Entity
        {
            if (this.CacheMap.ContainsKey(typeof(TEntity)))
            {
                this.CacheMap[typeof(TEntity)].AddRange(Utils.Nvl(entities).Cast<Entity>().ToList());
            }
            else
            {
                this.CacheMap.Add(typeof(TEntity), Utils.Nvl(entities).Cast<Entity>().ToList());
            }
        }

        /// <summary>
        /// Add entities inside the cache
        /// </summary>
        /// <param name="entityType">Entity type</param>
        /// <param name="entities">Entities to store inside the cache</param>
        public void AddEntities(Type entityType, List<Entity> entities)
        {
            this.CacheMap[entityType].AddRange(Utils.Nvl(entities));
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
