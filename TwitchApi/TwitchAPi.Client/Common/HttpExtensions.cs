using System.Collections.Specialized;
using System.Web;

namespace TwitchAPi.Client.Common;

public static class HttpExtensions
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

    public static NameValueCollection ParseQueryParams(this Uri uri)
    {
        var result = new NameValueCollection();

        if (string.IsNullOrEmpty(uri.Query))
            return result;

        string query = uri.Query.TrimStart('?');
        var pairs = query.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var pair in pairs)
        {
            var kv = pair.Split(new[] { '=' }, 2);
            if (kv.Length == 2)
            {
                var key = Uri.UnescapeDataString(kv[0]);
                var value = Uri.UnescapeDataString(kv[1]);
                result.Add(key, value);
            }
            else if (kv.Length == 1)
            {
                var key = Uri.UnescapeDataString(kv[0]);
                result.Add(key, string.Empty);
            }
        }

        return result;
    }
}
