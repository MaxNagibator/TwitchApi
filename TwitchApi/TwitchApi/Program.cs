using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using TwitchApi.Profile;
using TwitchAPi.Client.Data;
using TwitchAPi.Client;
using TwitchApi.Services;
using Microsoft.Win32;

namespace TwitchApi
{
    internal static class Program
    {
        private const string UriScheme = "twitchapi";
        private const string UriKey = "URL:twitchapi Protocol";
        private const string MutexName = "TwitchApiInstanceApp";

        public static IServiceProvider ServiceProvider { get; private set; } = null!;

        [STAThread]
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            ApplicationConfiguration.Initialize();
            ServiceProvider = CreateHostBuilder().Build().Services;

            using var mutex = new Mutex(true, MutexName, out bool isNewInstance);
            var instanceService = ServiceProvider.GetRequiredService<AppInstanceService>();

            if (!isNewInstance)
            {
                instanceService.SendToInstance(args);
                return;
            }

            instanceService.StartPipeServer();
            RegisterUriScheme();

            Application.Run(ServiceProvider.GetRequiredService<MainForm>());
        }

        private static IHostBuilder CreateHostBuilder()
        {
            var configuration = new ConfigurationBuilder()
                 .AddJsonFile("appsettings.json", optional: true)
                 .AddEnvironmentVariables()
                 .AddUserSecrets(Assembly.GetExecutingAssembly())
                 .Build();

            var hostBuilder = Host.CreateDefaultBuilder();
            hostBuilder.ConfigureAppConfiguration(builder =>
            {
                builder.Sources.Clear();
                builder.AddConfiguration(configuration);
            });

            hostBuilder.ConfigureServices((context, services) =>
            {
                var clientAuth = context.Configuration.GetSection("Twitch")?.Get<ClientTwitchAuth>();

                if (clientAuth == null)
                    throw new InvalidOperationException("Missing twitch configuration.");

                services.AddSingleton<CommandLineHandler>();
                services.AddSingleton<NamedPipeService>();
                services.AddSingleton<AppInstanceService>();

                services.AddSingleton(clientAuth);
                services.AddSingleton(new UserAuthStorage("credentials.json"));
                services.AddSingleton<TwitchApiClient>();

                services.AddSingleton(new TwitchProfileStorage("profiles.json"));
                services.AddSingleton<MainForm>();
            });

            return hostBuilder;
        }
        private static void RegisterUriScheme()
        {
            string? appPath = Environment.ProcessPath;

            if (appPath == null)
                throw new InvalidOperationException("Failed to get application path");

            using (var userClasses = Registry.CurrentUser.CreateSubKey(@$"Software\Classes\{UriScheme}"))
            {
                userClasses.SetValue(null, UriKey);
                userClasses.SetValue("URL Protocol", String.Empty, RegistryValueKind.String);

                using (RegistryKey defaultIcon = userClasses.CreateSubKey("DefaultIcon"))
                {
                    string iconValue = string.Format("\"{0}\",0", appPath);
                    defaultIcon.SetValue(null, iconValue);
                }

                using (RegistryKey shell = userClasses.CreateSubKey("shell"))
                {
                    using (RegistryKey open = shell.CreateSubKey("open"))
                    {
                        using (RegistryKey command = open.CreateSubKey("command"))
                        {
                            string cmdValue = string.Format("\"{0}\" \"%1\"", appPath);
                            command.SetValue(null, cmdValue);
                        }
                    }
                }
            }
        }
    }
}
