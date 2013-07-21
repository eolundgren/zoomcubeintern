namespace PulseMates.Infrastructure.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public static class UriExtensions
    {
        public static IEnumerable<string> GetQuerystringParameterValues(this Uri url, string parameterName)
        {
            if (string.IsNullOrEmpty(parameterName) || url == null)
                yield break;

            var regex = new Regex(@"[?&]" + parameterName + "=(.[^&]*)", RegexOptions.IgnoreCase);

            foreach (Match m in regex.Matches(url.Query))
                yield return m.Groups[1].Value.ToLower();
        }
    }
}