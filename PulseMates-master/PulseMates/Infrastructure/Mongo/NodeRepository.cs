

namespace PulseMates.Infrastructure.Mongo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    using Models;
    using Models.Storage;

    using MongoDB.Driver;
    using MongoDB.Driver.Linq;
    using MongoDB.Driver.Builders;
    using MongoDB.Bson;

    public class NodeRepository : Repository<Node>, INodeRepository
    {
        const double _earthRadius = 6378.0; // km

        #region INodeRepository Members

        public Node Find(string id)
        {
            return Collection.FindOne(findById(id));
        }

        public IQueryable<Node> FindAll()
        {
            return Collection.AsQueryable()
                .OrderByDescending(x => x.Time);
        }

        public IQueryable<Node> FindAll(string[] tags)
        {
            return FindAll()
                .Where(x => LinqToMongo.Inject(Query.In("Tags", new BsonArray(tags))));
        }

        public IQueryable<Node> FindAll(double longitude, double latitude, double radius)
        {
            // ensure the Location index exist.
            Collection.EnsureIndex(IndexKeys.GeoSpatial("Location"));

            return FindAll()
                .Where(x => LinqToMongo.Inject(Query.WithinCircle("Location", longitude, latitude, radius / _earthRadius, true)));
        }

        public IQueryable<Node> FindAll(string[] tags, double longitude, double latitude, double radius)
        {
            // ensure the Location index exist.
            Collection.EnsureIndex(IndexKeys.GeoSpatial("Location"));

            return FindAll()
                .Where(x =>
                    LinqToMongo.Inject(Query.In("Tags", new BsonArray(tags))) &&
                    LinqToMongo.Inject(Query.WithinCircle("Location", longitude, latitude, radius / _earthRadius, true))
                );
        }

        public bool Create(Node node)
        {
            node.Id = generateNewId();
            var result = Collection.Insert(node);

            return result.Ok;
        }

        public bool Update(Node node)
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

        #region INodeRepository Members

        public Tag[] GetUniqueTags()
        {
            var unwindTagsOperation = new BsonDocument { { "$unwind", "$Tags" } };
            var group = new BsonDocument { { "$group", 
                new BsonDocument { { "_id", "$Tags" }, 
                { "Count", new BsonDocument { { "$sum", 1 } } } 
            } } };

            var pipeline = new[] { unwindTagsOperation, group };
            var result = Collection.Aggregate(pipeline);

            if (result.Ok)
                return result.ResultDocuments
                    .Select(x => new Tag { Name = x["_id"].AsString, Sum = x["Count"].AsInt32 })
                    .OrderByDescending(x => x.Sum)
                    .ThenBy(x => x.Name)
                    .ToArray();

            return new Tag[0];
        }

        #endregion
    }
}