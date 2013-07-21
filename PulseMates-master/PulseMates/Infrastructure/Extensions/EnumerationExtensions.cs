namespace PulseMates.Infrastructure.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public static class EnumerationExtensions
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (var i in list)
                action(i);

            return list;
        }
    }
}