using System.Diagnostics;
using System.IO.Pipes;

namespace TwitchApi.Services;

internal class NamedPipeService
{
    private const string PipeName = "TwitchApiInstancePipe";

    public async Task Start(Action<string[]> onMessage, CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await using var server = new NamedPipeServerStream(PipeName, PipeDirection.In);
                await server.WaitForConnectionAsync(cancellationToken);
                
                using var reader = new StreamReader(server);
                var line = await reader.ReadLineAsync(cancellationToken);

                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                var args = line.Split('|');
                onMessage?.Invoke(args);
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Pipe error: {e.Message}");
        }
    }

    public async void Send(string[] args)
    {
        try
        {
            await using var client = new NamedPipeClientStream(".", PipeName, PipeDirection.Out);
            await client.ConnectAsync(500);

            await using var writer = new StreamWriter(client);
            writer.AutoFlush = true;

            await writer.WriteLineAsync(string.Join("|", args));
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Pipe error: {e.Message}");
        }
    }
}
