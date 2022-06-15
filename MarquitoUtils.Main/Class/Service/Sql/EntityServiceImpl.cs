using MarquitoUtils.Main.Class.Entities.Sql;
using MarquitoUtils.Main.Class.Tools;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarquitoUtils.Main.Class.Service.Sql
{
    public class EntityServiceImpl : EntityService
    {
        public DbContext DbContext { get; set; }
        public T FindEntityById<T>(int id) 
            where T : Entity, IEntity
        {
            T entity = null;

            List<T> entities = this.GetEntityList<T>();

            if (Utility.IsNotEmpty(entities))
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

            if (Utility.IsNotEmpty(entities))
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

            //if (Utility.IsNotEmpty(this.DbContext.Set<T>())) 
            if (Utility.IsNotEmpty(entities))
            {
                entity = entities.Where(entity => this.MatchUniqueConstraint(entity, constraints))
                    .FirstOrDefault();
                /*entity = this.DbContext.Set<T>().ToList()
                    .Where(entity => this.MatchUniqueConstraint(entity, constraints)).FirstOrDefault();*/
            }

            return entity;
        }

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
                    if (!Utility.GetAsString(fieldValue).Trim()
                        .Equals(Utility.GetAsString(constraint.Value).Trim()))
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
            if (Utility.IsNotNull(entity.Id))
            {
                int newId = 1;

                List<T> entities = this.GetEntityList<T>();

                //if (Utility.IsNotEmpty(this.DbContext.Set<T>()))
                if (Utility.IsNotEmpty(entities))
                {

                    newId = entities.Max(entity => entity.Id) + 1;
                    //newId = this.DbContext.Set<T>().Max(entity => entity.Id) + 1;
                }

                this.DbContext.Set<T>().Add(entity);

                entity.Id = newId;
            }
        }

        public List<T> GetEntityList<T>() where T : Entity, IEntity
        {
            List<T> entityList = new List<T>();

            if (Utility.IsNotEmpty(this.DbContext.ChangeTracker.Entries()))
            {
                entityList.AddRange(this.DbContext.ChangeTracker.Entries()
                    .Where(entry => this.MatchEntityType<T>(entry.Entity))
                    .Select(entry => (T)entry.Entity)
                    .ToList());
            }

            if (Utility.IsNotEmpty(this.DbContext.Set<T>().ToList()))
            {
                entityList.AddRange(this.DbContext.Set<T>().ToList());
            }

            return entityList.Distinct().ToList();
        }

        public void RemoveEntity<T>(T entity) where T : Entity, IEntity
        {
            if (Utility.IsNotEmpty(this.DbContext.Set<T>()))
            {
                this.DbContext.Set<T>().Remove(entity);
            }
        }

        public bool FlushData()
        {
            bool dataHasFlushed = false;
            //Entity entity = (T)Activator.CreateInstance(typeof(T));

            //string tableName = entity.GetTableName();

            using (IDbContextTransaction ts = this.DbContext.Database.BeginTransaction())
            {
                try
                {
                    /*this.DbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT " 
                        + entity.GetTableName().ToUpper() + " ON");*/

                    // TODO Ne marche pas a cause du IDENTITY_INSERT, que faire ??
                    //this.DbContext.SaveChanges();




                    /*Dictionary<EntityEntry, EntityState> entities = new Dictionary<EntityEntry, EntityState>();



                    foreach (EntityEntry entity in this.DbContext.ChangeTracker.Entries())
                    {
                        entities.Add(entity, entity.State);
                        entity.State = EntityState.Unchanged;
                    }

                    foreach (KeyValuePair<EntityEntry, EntityState> entity in entities)
                    {
                        Entity ent = (Entity)entity.Key.Entity;
                        entity.Key.State = entity.Value;

                        switch (entity.Key.State)
                        {
                            case EntityState.Added:
                                this.AddEntity(ent);
                                break;
                            case EntityState.Modified:
                                this.UpdateEntity(ent);
                                break;
                            case EntityState.Deleted:
                                this.RemoveEntity(ent);
                                break;
                        }
                    }*/


                    this.DbContext.SaveChanges();



                    /*this.DbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT " 
                        + entity.GetTableName().ToUpper() + " OFF");*/

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

        private void AddEntity(Entity entity)
        {
            this.DbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT "
                        + entity.GetTableName().ToUpper() + " ON");

            this.DbContext.Add(entity);
            this.DbContext.SaveChanges();

            this.DbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT "
            + entity.GetTableName().ToUpper() + " OFF");
        }

        private void UpdateEntity(Entity entity)
        {
            this.DbContext.Update(entity);
            this.DbContext.SaveChanges();
        }

        private void RemoveEntity(Entity entity)
        {
            this.DbContext.Remove(entity);
            this.DbContext.SaveChanges();
        }
    }
}
