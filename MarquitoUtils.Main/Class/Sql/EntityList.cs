﻿using MarquitoUtils.Main.Class.Entities.Sql;
using MarquitoUtils.Main.Class.Service.Sql;
using MarquitoUtils.Main.Class.Tools;
namespace MarquitoUtils.Main.Class.Sql
{
    /// <summary>
    /// Entity list, can be inherited by entity list with specific entity type
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public abstract class EntityList<T> where T : Entity, IEntity
    {
        /// <summary>
        /// The database context
        /// </summary>
        protected DefaultDbContext DbContext { get; set; }
        /// <summary>
        /// Filters
        /// </summary>
        private List<Func<T, bool>> Filters { get; set; } = new List<Func<T, bool>>();
        /// <summary>
        /// Includes
        /// </summary>
        protected ISet<string> Includes { get; private set; } = new HashSet<string>();

        /// <summary>
        /// Entity list, can be inherited by entity list with specific entity type
        /// </summary>
        /// <param name="dbContext">The database context</param>
        public EntityList(DefaultDbContext dbContext)
        {
            this.DbContext = dbContext;

            // Get list properties, and includes data
            typeof(T).GetProperties()
                .Where(prop => Utils.IsGenericCollectionType(prop.PropertyType)).ToList()
                .ForEach(prop =>
                {
                    this.Includes.Add(prop.Name);
                });
        }

        /// <summary>
        /// Get list of entities
        /// </summary>
        /// <returns>Entities</returns>
        public IEnumerable<T> GetEntityList()
        {
            return this.GetEntityService().GetEntityList(this.Filters, this.Includes);
        }

        /// <summary>
        /// Add equal filter
        /// </summary>
        /// <typeparam name="TProperty">Property type</typeparam>
        /// <param name="propertyName">Property name</param>
        /// <param name="propertyValue">Property value</param>
        protected void AddEqualFilter<TProperty>(string propertyName, TProperty propertyValue)
        {
            Func<T, bool> equalFilter = this.GetFilter(propertyName, propertyValue, FilterType.IS_EQUAL);
            this.Filters.Add(equalFilter);
        }

        /// <summary>
        /// Add not equal filter
        /// </summary>
        /// <typeparam name="TProperty">Property type</typeparam>
        /// <param name="propertyName">Property name</param>
        /// <param name="propertyValue">Property value</param>
        protected void AddNotEqualFilter<TProperty>(string propertyName, TProperty propertyValue)
        {
            Func<T, bool> notEqualFilter = this.GetFilter(propertyName, propertyValue, FilterType.IS_NOT_EQUAL);
            this.Filters.Add(notEqualFilter);
        }

        /// <summary>
        /// Add in filter
        /// </summary>
        /// <typeparam name="TProperty">Properties type</typeparam>
        /// <param name="propertyName">Property name</param>
        /// <param name="propertyValues">The values contain the value of property</param>
        protected void AddInFilter<TProperty>(string propertyName, IEnumerable<TProperty> propertyValues)
        {
            Func<T, bool> inFilter = this.GetFilter(propertyName, propertyValues.Select(val => (object) val)
                .ToList(), FilterType.IS_IN);
            this.Filters.Add(inFilter);
        }

        /// <summary>
        /// Add not in filter
        /// </summary>
        /// <typeparam name="TProperty">Properties type</typeparam>
        /// <param name="propertyName">Property name</param>
        /// <param name="propertyValues">The values doesn't contain the value of property</param>
        protected void AddNotInFilter<TProperty>(string propertyName, IEnumerable<TProperty> propertyValues)
        {
            Func<T, bool> notInFilter = this.GetFilter(propertyName, propertyValues.Select(val => (object)val)
                .ToList(), FilterType.IS_NOT_IN);
            this.Filters.Add(notInFilter);
        }

        /// <summary>
        /// Add like filter
        /// </summary>
        /// <param name="propertyName">Property name</param>
        /// <param name="propertyLikeValue">The property like value</param>
        /// <param name="likeType">Start with, like, or end with</param>
        /// <param name="isCaseSensitive">Is case sensitive ?</param>
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
                            /*equalFilter = entity => ((string)entity.GetFieldValue(propertyName)).Trim()
                            .Equals(((string)propertyValue).Trim());*/
                            equalFilter = entity => entity.FieldEquals(propertyName, (string) propertyValue, true);
                        }
                        else
                        {
                            //equalFilter = entity => entity.GetFieldValue(propertyName).Equals(propertyValue);
                            equalFilter = entity => entity.FieldEquals(propertyName, propertyValue);
                        }
                        break;
                    case FilterType.IS_NOT_EQUAL:
                        if (propertyValue is string)
                        {
                           /* equalFilter = entity => !((string)entity.GetFieldValue(propertyName)).Trim()
                            .Equals(((string)propertyValue).Trim());*/
                            equalFilter = entity => !entity.FieldEquals(propertyName, (string)propertyValue, true);
                        }
                        else
                        {
                           // equalFilter = entity => !entity.GetFieldValue(propertyName).Equals(propertyValue);
                            equalFilter = entity => !entity.FieldEquals(propertyName, propertyValue);
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
        private IEntityService GetEntityService()
        {
            IEntityService entityService = new EntityService();

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
            /// <summary>
            /// Contain
            /// </summary>
            CONTAIN,
            /// <summary>
            /// Start with
            /// </summary>
            START_WITH,
            /// <summary>
            /// End with
            /// </summary>
            END_WITH
        }

        /// <summary>
        /// Filter type
        /// </summary>
        public enum FilterType
        {
            /// <summary>
            /// Is equal to
            /// </summary>
            IS_EQUAL,
            /// <summary>
            /// Is not equal to
            /// </summary>
            IS_NOT_EQUAL,
            /// <summary>
            /// Is in list of values
            /// </summary>
            IS_IN,
            /// <summary>
            /// Isn't in list of values
            /// </summary>
            IS_NOT_IN,
            /// <summary>
            /// Is like
            /// </summary>
            IS_LIKE
        }
    }
}
