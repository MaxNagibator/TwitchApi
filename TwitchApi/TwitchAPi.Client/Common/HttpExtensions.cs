using System.Collections.Specialized;
using System.Web;

namespace TwitchAPi.Client.Common;

internal static class HttpExtensions
{
    public static string ToQueryString(this NameValueCollection nvc)
    {
        if (nvc == null || nvc.Count == 0)
            return string.Empty;

        var array = (
            from key in nvc.AllKeys
            where key != null
            from value in nvc.GetValues(key) ?? Array.Empty<string>()
            select $"{HttpUtility.UrlEncode(key)}={HttpUtility.UrlEncode(value)}"
        ).ToArray();

        return array.Length > 0 ? "?" + string.Join("&", array) : string.Empty;
    }
}
