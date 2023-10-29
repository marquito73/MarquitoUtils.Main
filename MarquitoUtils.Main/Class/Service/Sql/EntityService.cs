﻿using MarquitoUtils.Main.Class.Entities.Sql;
using MarquitoUtils.Main.Class.Sql;
using MarquitoUtils.Main.Class.Tools;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Reflection;

namespace MarquitoUtils.Main.Class.Service.Sql
{
    /// <summary>
    /// Entity service, for get, update, insert or delete entities
    /// </summary>
    public class EntityService : IEntityService
    {
        /// <summary>
        /// Db context of the Entity service
        /// </summary>
        public DefaultDbContext DbContext { get; set; }

        public T FindEntityById<T>(int id) 
            where T : Entity, IEntity
        {
            T entity = null;

            List<T> entities = this.GetEntityList<T>();

            if (Utils.IsNotEmpty(entities))
            //if (Utility.IsNotEmpty(this.DbContext.ChangeTracker.Entries()))
            {
                entity = entities.SingleOrDefault<T>(entity => entity.Id == id);
                //entity = this.DbContext.Set<T>().SingleOrDefault<T>(entity => entity.Id == id);
            }

            return entity;
        }

        public List<T> FindEntitiesByIds<T>(List<int> ids)
            where T : Entity, IEntity
        {
            List<T> entitiesFound = default(List<T>);

            List<T> entities = this.GetEntityList<T>();

            if (Utils.IsNotEmpty(entities))
            {
                entitiesFound = entities.Where(entity => ids.Contains(entity.Id)).ToList();
            }

            return entitiesFound;
        }

        public T FindEntityByUniqueConstraint<T>(List<PropertyConstraint> constraints) 
            where T : Entity, IEntity
        {
            return this.FindEntityByUniqueConstraint<T>(constraints.ToArray());
        }

        public T FindEntityByUniqueConstraint<T>(params PropertyConstraint[] constraints) 
            where T : Entity, IEntity
        {
            T entity = null;

            List<T> entities = this.GetEntityList<T>();

            if (Utils.IsNotEmpty(entities))
            {
                entity = entities.Where(entity => this.MatchUniqueConstraint(entity, constraints))
                    .FirstOrDefault();
            }

            return entity;
        }

        /// <summary>
        /// Entity match the type ?
        /// </summary>
        /// <typeparam name="T">The type to match</typeparam>
        /// <param name="entity">The entity</param>
        /// <returns>Entity match the type ?</returns>
        private bool MatchEntityType<T>(object entity) where T : Entity, IEntity
        {
            return entity is T;
        }

        public bool MatchUniqueConstraint<T>(T entity, params PropertyConstraint[] constraints) 
            where T : Entity, IEntity
        {
            bool matchConstraints = true;

            foreach (PropertyConstraint constraint in constraints)
            {
                object fieldValue = entity.GetFieldValue(constraint.PropertyName);
                if (fieldValue is string)
                {
                    string strFieldValueAsString = Utils.GetAsString(fieldValue);
                    string strConstraintValue = Utils.GetAsString(constraint.Value);
                    if (!constraint.CaseSensitive)
                    {
                        strFieldValueAsString = strFieldValueAsString.ToLower();
                        strConstraintValue = strConstraintValue.ToLower();
                    }

                    if (!strFieldValueAsString.Trim()
                        .Equals(strConstraintValue.Trim()))
                    {
                        matchConstraints = false;
                        break;
                    }
                }
                else if (!fieldValue.Equals(constraint.Value))
                {
                    matchConstraints = false;
                    break;
                }
            }

            return matchConstraints;
        }

        public void PersistEntity<T>(T entity) where T : Entity, IEntity
        {
            if (Utils.IsNotNull(entity.Id))
            {
                int newId = 1;

                List<T> entities = this.GetEntityList<T>();

                if (Utils.IsNotEmpty(entities))
                {

                    newId = entities.Max(entity => entity.Id) + 1;
                }

                this.DbContext.Set<T>().Add(entity);

                entity.Id = newId;
            }
        }

        public IQueryable GetEntityList(Type T)
        {
            // Get the generic type definition
            /*MethodInfo method = this.DbContext.GetType().GetMethod(nameof(this.DbContext.Set), 
                BindingFlags.Public | BindingFlags.Instance);*/

            MethodInfo method = this.DbContext.GetType().GetMethods()
                .Where(method => method.Name.Equals(nameof(this.DbContext.Set))).First();

            // Build a method with the specific type argument you're interested in
            method = method.MakeGenericMethod(T);

            return (method.Invoke(this.DbContext, null) as IQueryable);
        }

        public List<T> GetEntityList<T>() where T : Entity, IEntity
        {
            return this.GetEntityList<T>(new List<Func<T, bool>>(), new HashSet<string>());
        }

        public List<T> GetEntityList<T>(List<Func<T, bool>> filters, ISet<string> includes) where T : Entity, IEntity
        {
            List<T> entityList = new List<T>();

            if (Utils.IsNotEmpty(this.DbContext.ChangeTracker.Entries()))
            {
                entityList.AddRange(this.DbContext.ChangeTracker.Entries()
                    .Where(entry => this.MatchEntityType<T>(entry.Entity))
                    .Select(entry => (T)entry.Entity)
                    .Where(this.ApplyFilters(filters))
                    .ToList());
            }

            if (Utils.IsNotEmpty(this.DbContext.Set<T>().ToList()))
            {
                entityList.AddRange(this.ApplyIncludes(this.DbContext.Set<T>(), includes)
                    .Where(this.ApplyFilters(filters))
                    .ToList());
            }

            return entityList.Distinct().ToList();
        }

        /// <summary>
        /// Apply filters
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <param name="filters">Filters</param>
        /// <returns>Function for apply filters</returns>
        private Func<T, bool> ApplyFilters<T>(List<Func<T, bool>> filters) where T : Entity, IEntity
        {
            return entity =>
            {
                bool result = true;

                foreach (Func<T, bool> filter in filters)
                {
                    result = filter(entity);
                    if (!result)
                    {
                        break;
                    }
                }

                return result;
            };
        }

        /// <summary>
        /// Apply includes
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <param name="dbSet">The DB set</param>
        /// <param name="includes">Includes</param>
        /// <returns>Function for apply includes</returns>
        private IQueryable<T> ApplyIncludes<T>(IQueryable<T> dbSet, ISet<string> includes) where T : Entity, IEntity
        {
            foreach (string include in includes)
            {
                dbSet = dbSet.Include(include);
            }

            return dbSet;
        }

        // TODO
        public void RemoveEntity<T>(T entity) where T : Entity, IEntity
        {
            if (Utils.IsNotEmpty(this.DbContext.Set<T>()))
            {
                this.DbContext.Set<T>().Remove(entity);
            }
        }

        public bool FlushData()
        {
            bool dataHasFlushed = false;

            using (IDbContextTransaction ts = this.DbContext.Database.BeginTransaction())
            {
                try
                {
                    this.DbContext.SaveChanges();

                    ts.Commit();
                    dataHasFlushed = true;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                }
            }

            return dataHasFlushed;
        }
    }
}
