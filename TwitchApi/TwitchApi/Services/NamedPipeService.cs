using System.IO.Pipes;

namespace TwitchApi.Services;

internal class NamedPipeService
{
    private const string PipeName = "TwitchApiInstancePipe";

    public void Start(Action<string[]> onMessage)
    {
        new Thread(() =>
        {
            while (true)
            {
                using var server = new NamedPipeServerStream(PipeName, PipeDirection.In);
                server.WaitForConnection();
                using var reader = new StreamReader(server);
                var line = reader.ReadLine();

                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                var args = line.Split('|');
                onMessage?.Invoke(args);
            }
        })
        {
            IsBackground = true
        }.Start();
    }

    public void Send(string[] args)
    {
        try
        {
            using var client = new NamedPipeClientStream(".", PipeName, PipeDirection.Out);
            client.Connect(500);
            using var writer = new StreamWriter(client) { AutoFlush = true };
            writer.WriteLine(string.Join("|", args));
        }
        catch
        {
        }
    }
}
