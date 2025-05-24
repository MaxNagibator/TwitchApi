namespace TwitchApi.Services;
internal class AppInstanceService
{
    private readonly NamedPipeService _pipe;
    private readonly CommandLineHandler _handler;

    public AppInstanceService(NamedPipeService pipe, CommandLineHandler handler)
    {
        _pipe = pipe;
        _handler = handler;
    }

    public void StartPipeServer()
    {
        _pipe.Start(_handler.Handle);
    }

    public void SendToInstance(string[] args)
    {
        _pipe.Send(args);
    }
}
