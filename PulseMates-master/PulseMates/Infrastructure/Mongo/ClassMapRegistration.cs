namespace PulseMates.Infrastructure.Mongo
{
    using MongoDB.Bson.Serialization;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Reflection;

    using Models.Storage;
    using MongoDB.Bson.Serialization.Conventions;

    using Extensions;

    /// <summary>
    /// manual.
    /// url: http://www.mongodb.org/display/DOCS/CSharp+Driver+Serialization+Tutorial
    /// </summary>
    static class ClassMapRegistration
    {
        private static bool _isInitialized = false;

        public static void Register()
        {
            if (!_isInitialized)
            {
                //BsonClassMap.RegisterClassMap<DataSourceValue>(m =>
                //{
                //    m.MapIdMember(x => x.Id);
                //    m.MapMember(x => x.Values).SetDefaultValue(new IDataSourceParameter[0]).SetIgnoreIfDefault(true);
                //});

                //BsonClassMap.RegisterClassMap<DataSource>(m =>
                //{
                //    m.MapIdMember(x => x.Id);
                //    m.MapMember(x => x.Name).SetDefaultValue("");
                //    m.MapMember(x => x.Description).SetDefaultValue("");
                //    m.MapMember(x => x.Parameters).SetDefaultValue(new IDataSourceParameter[0]);
                //});

                //ReflectionHelper.GetTypes()
                //    .ForEach(x => BsonClassMap.LookupClassMap(x.Value));

                //ReflectionHelper.GetGenericTypes()
                //    .ForEach(x => BsonClassMap.LookupClassMap(x));
                
                //_isInitialized = true;
            }
        }
    }
}