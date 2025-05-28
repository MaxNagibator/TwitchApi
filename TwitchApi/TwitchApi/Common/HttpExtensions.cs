using System.Collections.Specialized;

namespace TwitchApi.Common;

public static class HttpExtensions
{
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

    public static bool IsLocalhost(this Uri uri, out int port)
    {
        port = uri.Port;
        return uri.Host.Equals("localhost", StringComparison.OrdinalIgnoreCase)
               || uri.Host.Equals("127.0.0.1")
               || uri.Host.Equals("::1");
    }
}
