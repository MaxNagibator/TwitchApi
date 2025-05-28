using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace TwitchApi.Services;

public class LocalOAuthReceiver : BackgroundService
{
    private const string HtmlResponse = @"
    <html>
      <head>
        <script>
          window.close();
        </script>
      </head>
      <body>
        <p>Вы можете закрыть это окно</p>
      </body>
    </html>";

    private readonly int _port;
    private readonly MainForm _form;

    public LocalOAuthReceiver(int port, MainForm form)
    {
        _port = port;
        _form = form;
    }


    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            var listener = new HttpListener();
            listener.Prefixes.Add($"http://localhost:{_port}/");
            listener.Start();

            while (!cancellationToken.IsCancellationRequested)
            {
                var context = await listener.GetContextAsync().WaitAsync(cancellationToken);
                var request = context.Request;
                var code = request.QueryString["code"];

                if (string.IsNullOrEmpty(code))
                    continue;

                byte[] buffer = Encoding.UTF8.GetBytes(HtmlResponse);
                context.Response.ContentLength64 = buffer.Length;
                context.Response.ContentType = "text/html";

                await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length, cancellationToken);
                context.Response.OutputStream.Close();

                _form.OnCodeRecevied(code);
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Listener error: {e.Message}");
        }
    }
}
