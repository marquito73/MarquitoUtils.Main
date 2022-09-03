using MarquitoUtils.Main.Class.Entities.Sql;
using MarquitoUtils.Main.Class.Service.Sql;
using MarquitoUtils.Main.Class.Tools;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarquitoUtils.Main.Class.Sql
{
    public abstract class EntityList<T> where T : Entity, IEntity
    {
        protected DbContext DbContext { get; set; }

        private List<Func<T, bool>> Filters { get; set; } = new List<Func<T, bool>>();

        protected List<string> Includes { get; private set; } = new List<string>();

        public EntityList(DbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        public List<T> GetEntityList()
        {
            return this.GetEntityService().GetEntityList(this.Filters, this.Includes);
        }

        protected void AddEqualFilter(string propertyName, object propertyValue)
        {
            Func<T, bool> equalFilter = this.GetFilter(propertyName, propertyValue, FilterType.IS_EQUAL);
            this.Filters.Add(equalFilter);
        }

        protected void AddNotEqualFilter(string propertyName, object propertyValue)
        {
            Func<T, bool> notEqualFilter = this.GetFilter(propertyName, propertyValue, FilterType.IS_NOT_EQUAL);
            this.Filters.Add(notEqualFilter);
        }

        protected void AddInFilter(string propertyName, List<object> propertyValues)
        {
            Func<T, bool> inFilter = this.GetFilter(propertyName, propertyValues, FilterType.IS_IN);
            this.Filters.Add(inFilter);
        }

        protected void AddNotInFilter(string propertyName, List<object> propertyValues)
        {
            Func<T, bool> notInFilter = this.GetFilter(propertyName, propertyValues, FilterType.IS_NOT_IN);
            this.Filters.Add(notInFilter);
        }

        protected void AddLikeFilter(string propertyName, string propertyLikeValue, LikeType likeType, 
            bool isCaseSensitive)
        {
            Func<T, bool> likeFilter = this.GetFilter(propertyName, propertyLikeValue, FilterType.IS_LIKE, 
                likeType, isCaseSensitive);

            this.Filters.Add(likeFilter);
        }

        /// <summary>
        /// Return a filter function
        /// </summary>
        /// <param name="propertyName">The property's name</param>
        /// <param name="propertyValue">The property's value</param>
        /// <param name="filterType">The filter's type</param>
        /// <param name="likeType">The like's type</param>
        /// <param name="isCaseSensitive">Need to be case sensitive ?</param>
        /// <returns>A filter function</returns>
        private Func<T, bool> GetFilter(string propertyName, object propertyValue, FilterType filterType,
            LikeType likeType = LikeType.CONTAIN, bool isCaseSensitive = false)
        {
            Func<T, bool> equalFilter = entity => false;

            List<string> propertiesNames = propertyName.Split(".").ToList();

            if (propertiesNames.Count == 1)
            {
                equalFilter = entity => entity.GetFieldValue(propertyName).Equals(propertyValue);
                switch (filterType)
                {
                    case FilterType.IS_EQUAL:
                        if (propertyValue is string)
                        {
                            equalFilter = entity => ((string)entity.GetFieldValue(propertyName)).Trim()
                            .Equals(((string)propertyValue).Trim());
                        }
                        else
                        {
                            equalFilter = entity => entity.GetFieldValue(propertyName).Equals(propertyValue);
                        }
                        break;
                    case FilterType.IS_NOT_EQUAL:
                        if (propertyValue is string)
                        {
                            equalFilter = entity => !((string)entity.GetFieldValue(propertyName)).Trim()
                            .Equals(((string)propertyValue).Trim());
                        }
                        else
                        {
                            equalFilter = entity => !entity.GetFieldValue(propertyName).Equals(propertyValue);
                        }
                        break;
                    case FilterType.IS_IN:
                        equalFilter = entity => ((List<object>)propertyValue)
                        .Contains(entity.GetFieldValue(propertyName));
                        break;
                    case FilterType.IS_NOT_IN:
                        equalFilter = entity => !((List<object>)propertyValue)
                        .Contains(entity.GetFieldValue(propertyName));
                        break;
                    case FilterType.IS_LIKE:
                        equalFilter = entity => this.IsLikeValue(entity, propertyName,
                            Utils.GetAsString(propertyValue), likeType, isCaseSensitive);
                        break;
                }
            }
            else if (propertiesNames.Count > 1)
            {
                equalFilter = entity =>
                {
                    bool result = false;

                    Entity includeEntity = entity;

                    int cpt = 1;
                    foreach (string property in propertiesNames)
                    {
                        if (!cpt.Equals(propertiesNames.Count))
                        {
                            includeEntity = (Entity)includeEntity.GetFieldValue(property);
                        }
                        else
                        {
                            switch (filterType)
                            {
                                case FilterType.IS_EQUAL:
                                    if (propertyValue is string)
                                    {
                                        result = ((string)includeEntity.GetFieldValue(property)).Trim().Equals(((string)propertyValue).Trim());
                                    }
                                    else
                                    {
                                        result = includeEntity.GetFieldValue(property).Equals(propertyValue);
                                    }
                                    break;
                                case FilterType.IS_NOT_EQUAL:
                                    if (propertyValue is string)
                                    {
                                        result = ((string)includeEntity.GetFieldValue(property)).Trim().Equals(((string)propertyValue).Trim());
                                    }
                                    else
                                    {
                                        result = includeEntity.GetFieldValue(property).Equals(propertyValue);
                                    }
                                    break;
                                case FilterType.IS_IN:
                                    result = ((List<object>)propertyValue)
                                    .Contains(includeEntity.GetFieldValue(property));
                                    break;
                                case FilterType.IS_NOT_IN:
                                    result = !((List<object>)propertyValue)
                                    .Contains(includeEntity.GetFieldValue(property));
                                    break;
                                case FilterType.IS_LIKE:
                                    result = this.IsLikeValue(includeEntity, property, Utils.GetAsString(propertyValue),
                                        likeType, isCaseSensitive);
                                    break;
                            }
                        }

                        cpt++;
                    }

                    return result;
                };
            }

            return equalFilter;
        }

        /// <summary>
        /// Return if the property of an entity respect the like property value
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <param name="propertyName">The property's name</param>
        /// <param name="propertyLikeValue">The property's like valye</param>
        /// <param name="likeType">The like's type</param>
        /// <param name="isCaseSensitive">Need to be case sensitive ?</param>
        /// <returns></returns>
        private bool IsLikeValue(Entity entity, string propertyName, string propertyLikeValue, LikeType likeType,
            bool isCaseSensitive)
        {
            bool result = false;

            string strEntityValue = Utils.GetAsString(entity.GetFieldValue(propertyName)).Trim();

            string valueWithoutSpaces = propertyLikeValue.Trim();

            if (!isCaseSensitive)
            {
                strEntityValue = strEntityValue.ToLower();
                valueWithoutSpaces = valueWithoutSpaces.ToLower();
            }

            string filterValue;
            if (likeType.Equals(LikeType.CONTAIN))
            {
                result = strEntityValue.Contains(valueWithoutSpaces);
            }
            else if (likeType.Equals(LikeType.END_WITH))
            {
                result = strEntityValue.EndsWith(valueWithoutSpaces);
            }
            else if (likeType.Equals(LikeType.START_WITH))
            {
                result = strEntityValue.StartsWith(valueWithoutSpaces);
            }

            return result;
        }

        /// <summary>
        /// Return the entity service
        /// </summary>
        /// <returns>Entity service</returns>
        /// <exception cref="Exception">Exception if the DbContext of the entitylist is null</exception>
        private EntityService GetEntityService()
        {
            EntityService entityService = new EntityServiceImpl();

            if (Utils.IsNotNull(this.DbContext))
            {
                entityService.DbContext = this.DbContext;
            }
            else
            {
                throw new Exception("Can't get entity service without DbContext specified to the WebClass");
            }

            return entityService;
        }

        /// <summary>
        /// Like type
        /// </summary>
        public enum LikeType
        {
            CONTAIN,
            START_WITH,
            END_WITH
        }

        /// <summary>
        /// Filter type
        /// </summary>
        public enum FilterType
        {
            IS_EQUAL,
            IS_NOT_EQUAL,
            IS_IN,
            IS_NOT_IN,
            IS_LIKE
        }
    }
}
