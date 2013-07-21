using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace PulseMates.Infrastructure.Extensions
{
    public static class DictionaryExtensions
    {
        private static readonly Type _arrayType = typeof(System.Collections.IEnumerable);

        public static IDictionary<string, object> ToSafeDictionary(this IDictionary<string, object> dic) 
        {
            var safeDic = new Dictionary<string, object>();

            if (dic == null)
                return safeDic;

            foreach (var item in dic)
            {
                var t = item.Value.GetType();

                if (IsPrimitive(t))
                    safeDic.Add(item.Key, item.Value);
                else if (IsPrimitiveArray(t))
                    safeDic.Add(item.Key, ToArray(item.Value));
                else
                    safeDic.Add(item.Key, "TODO: convert complex object ToDictionary");
            }

            return safeDic;
        }

        private static string[] ToArray(object value)
        {
            var list = new List<string>();

            foreach(var val in value as IEnumerable)
                list.Add(val.ToString());

            return list.ToArray();
        }

        private static bool IsPrimitive(Type t)
        {
            return t.IsPrimitive || t.IsArray || t.IsEnum || t == typeof(string);
        }

        private static bool IsPrimitiveArray(Type t)
        {
            return _arrayType.IsAssignableFrom(t) && t.GenericTypeArguments.Length == 0;
        }

        private static IDictionary<string, object> ToDictionary(object anonymousObj)
        {
            var dic = new Dictionary<string, object>();

            if (anonymousObj != null)
            {
                foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(anonymousObj))
                    dic.Add(prop.Name, prop.GetValue(anonymousObj));
            }

            return dic;
        }

        //private static object[] ToAr
    }
}