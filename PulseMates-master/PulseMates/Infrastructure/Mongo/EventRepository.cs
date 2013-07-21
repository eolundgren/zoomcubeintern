

namespace PulseMates.Infrastructure.Mongo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    using Models;
    using Models.Storage;

    public class EventRepository : Repository<Event>, IEventRepository
    {
        #region IEventRepository Members

        public Event Find(string id)
        {
            return Collection.FindOne(findById(id));
        }

        public IQueryable<Event> FindAll()
        {
            return Collection.FindAll().AsQueryable();
        }

        public bool Create(Event node)
        {
            node.Id = generateNewId();

            var result = Collection.Insert(node);
            return result.Ok;
        }

        public bool Update(Event node)
        {
            var result = Collection.Save(node);
            return result.UpdatedExisting;
        }

        public bool Delete(string id)
        {
            var result = Collection.Remove(findById(id));
            return result.DocumentsAffected > 0;
        }

        #endregion
    }
}