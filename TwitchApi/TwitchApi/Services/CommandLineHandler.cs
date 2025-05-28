using System.Collections.Specialized;
using TwitchApi.Common;

namespace TwitchApi.Services;

internal class CommandLineHandler
{
    private readonly MainForm _form;
    private readonly Dictionary<string, Action<NameValueCollection>> _routes;

    public CommandLineHandler(MainForm form)
    {
        _form = form;

        _routes = new Dictionary<string, Action<NameValueCollection>>()
        {
            { "auth", HandleAuth }
        };
    }

    public void Handle(string[] args)
    {
        if (args == null || args.Length == 0 || string.IsNullOrEmpty(args[0]))
        {
            return;
        }

        try
        {
            var uri = new Uri(args[0]);
            if (uri.Scheme != "twitchapi")
                return;

            string key = uri.Host.ToLower();
            if (!_routes.TryGetValue(key, out var handler))
            {
                return;
            }

            handler(uri.ParseQueryParams());
        }
        catch (UriFormatException)
        {
        }
    }

    private void HandleAuth(NameValueCollection queryParams)
    {
        string? code = queryParams["code"];
        if (string.IsNullOrEmpty(code))
        {
            return;
        }

        _form.OnCodeRecevied(code);
    }
}
