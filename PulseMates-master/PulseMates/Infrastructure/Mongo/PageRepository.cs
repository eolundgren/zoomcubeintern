

namespace PulseMates.Infrastructure.Mongo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    using PulseMates.Models;
    using PulseMates.Models.Storage;

    public class PageRepository : Repository<Page>, IPageRepository
    {
        #region IPageRepository Members

        public Page Find(string id)
        {
            return Collection.FindOne(findById(id));
        }

        public IQueryable<Page> FindAll()
        {
            return Collection.FindAll().AsQueryable();
        }

        public bool Create(Page node)
        {
            node.Id = generateNewId();

            var result = Collection.Insert(node);
            return result.Ok;
        }

        public bool Update(Page node)
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