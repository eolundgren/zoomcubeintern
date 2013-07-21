using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PulseMates.Infrastructure.Extensions
{
    public static class TypeExtensions
    {
        /// <summary>
        /// For instance return DataSoruce from DataSourceRepository since the DataSourceRepository is a base class of Repository
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetNameWithoutBase(this Type type)
        {
            var removal = type.BaseType.IsGenericType ?
                type.BaseType.Name.Substring(0, type.BaseType.Name.Length - 2) // dirty hack, will not work with multiple generic parameters
                : type.BaseType.Name;

            return type.Name.Replace(removal, "");
        }
    }
}