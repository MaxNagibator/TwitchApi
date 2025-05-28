using Microsoft.Extensions.Hosting;
using Microsoft.Win32;

namespace TwitchApi.Services;

internal class AppInstanceService : BackgroundService
{
    private const string UriScheme = "twitchapi";
    private const string UriKey = "URL:twitchapi Protocol";
    
    private readonly NamedPipeService _pipe;
    private readonly CommandLineHandler _handler;

    public AppInstanceService(NamedPipeService pipe, CommandLineHandler handler)
    {
        _pipe = pipe;
        _handler = handler;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        RegisterUriScheme();
        await _pipe.Start(_handler.Handle, cancellationToken);
    }

    public void SendToInstance(string[] args)
    {
        _pipe.Send(args);
    }
    
    private static void RegisterUriScheme()
    {
        string? appPath = Environment.ProcessPath;

        if (appPath == null)
            throw new InvalidOperationException("Failed to get application path");

        using var userClasses = Registry.CurrentUser.CreateSubKey(@$"Software\Classes\{UriScheme}");
        userClasses.SetValue(null, UriKey);
        userClasses.SetValue("URL Protocol", String.Empty, RegistryValueKind.String);

        using RegistryKey defaultIcon = userClasses.CreateSubKey("DefaultIcon");
        string iconValue = $"\"{appPath}\",0";
        defaultIcon.SetValue(null, iconValue);

        using RegistryKey shell = userClasses.CreateSubKey("shell");
        using RegistryKey open = shell.CreateSubKey("open");
        using RegistryKey command = open.CreateSubKey("command");
        
        string cmdValue = $"\"{appPath}\" \"%1\"";
        command.SetValue(null, cmdValue);
    }
}
