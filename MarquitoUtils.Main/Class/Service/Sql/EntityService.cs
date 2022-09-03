using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarquitoUtils.Main.Class.Entities.Sql;
using MarquitoUtils.Main.Class.Service.General;
using Microsoft.EntityFrameworkCore;

namespace MarquitoUtils.Main.Class.Service.Sql
{
    public interface EntityService : DefaultService
    {
        public DbContext DbContext { get; set; }

        public T FindEntityById<T>(int id) 
            where T : Entity, IEntity;

        public List<T> FindEntitiesByIds<T>(List<int> ids)
            where T : Entity, IEntity;

        public T FindEntityByUniqueConstraint<T>(List<PropertyConstraint> constraints)
            where T : Entity, IEntity;

        public T FindEntityByUniqueConstraint<T>(params PropertyConstraint[] constraints)
            where T : Entity, IEntity;

        public bool MatchUniqueConstraint<T>(T entity, params PropertyConstraint[] constraints)
            where T : Entity, IEntity;

        public void PersistEntity<T>(T entity) 
            where T : Entity, IEntity;

        public List<T> GetEntityList<T>() 
            where T : Entity, IEntity;

        public List<T> GetEntityList<T>(List<Func<T, bool>> filters, List<string> includes) 
            where T : Entity, IEntity;

        public bool FlushData();

        // TODO Ajouter des methodes de persist, flush et une variable contenant les éléments non flushés
    }
}
