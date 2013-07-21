namespace PulseMates.Infrastructure.Mongo
{
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;

    using Extensions;

    using System;
    using System.Configuration;
    using MongoDB.Bson;

    public abstract class Repository
    {
        private bool _disposed = false;

        protected Repository() : this("PulseMatesDb") { }
        protected Repository(string connectionStringName) 
        {
            var connString = ConfigurationManager.AppSettings[connectionStringName];

            if (string.IsNullOrWhiteSpace(connString))
                throw new InvalidOperationException("Couldn't find any AppSettings with the specific key=" + connectionStringName);

            var settings = MongoServerSettings.FromUrl(new MongoUrl(connString));

            Server = new MongoServer(settings);
            Database = Server.GetDatabase(connectionStringName, WriteConcern.Acknowledged);

            // workaround for WindowsAzure and Mongolab connection timeout.
            MongoDefaults.ConnectTimeout = TimeSpan.FromMinutes(3);
            MongoDefaults.MaxConnectionIdleTime = TimeSpan.FromMinutes(3);
            MongoDefaults.SocketTimeout = TimeSpan.FromMinutes(3);

            ClassMapRegistration.Register();
        }

        protected MongoServer Server { get; private set; }
        protected MongoDatabase Database { get; private set; }

        protected readonly Func<string, IMongoQuery> findById = (id) => Query.EQ("_id", id);
        protected readonly Func<string> generateNewId = () => ObjectId.GenerateNewId().ToString();


        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                if (Server != null)
                    Server.Disconnect();
            }

            _disposed = true;
        }
    }

    public abstract class Repository<TCollectionItem> : Repository
    {
        protected Repository() : base("PulseMatesDb") 
        {
            var collectionName = this.GetType().GetNameWithoutBase();
            Collection = Database.GetCollection<TCollectionItem>(collectionName);
        }
        protected Repository(string customCollectionName, string connectionStringName) : base(connectionStringName)
        {
            Collection = Database.GetCollection<TCollectionItem>(customCollectionName);
        }

        protected MongoCollection<TCollectionItem> Collection { get; private set; }
    }
}