using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace PulseMates.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string Slugify(this string phrase)
        {
            string str = RemoveAccent(phrase).ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();

            str = Regex.Replace(str, @"\s", "-"); // hyphens  
            str = Regex.Replace(str, @"-+", "-").Trim();
            return str;
        }

        public static string RemoveAccent(this string txt)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }
    }
}