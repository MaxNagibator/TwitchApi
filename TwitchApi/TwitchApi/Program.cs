using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using TwitchApi.Profile;
using TwitchAPi.Client.Data;
using TwitchAPi.Client;
using TwitchApi.Common;
using TwitchApi.Services;

namespace TwitchApi
{
    internal static class Program
    {
        private const string MutexName = "TwitchApiInstanceApp";


        [STAThread]
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            ApplicationConfiguration.Initialize();

            var host = CreateHostBuilder().Build();
            using var mutex = new Mutex(true, MutexName, out bool isNewInstance);

            if (!isNewInstance)
            {
                var instanceService = host.Services.GetService<AppInstanceService>();
                instanceService?.SendToInstance(args);
                return;
            }

            host.Start();
            Application.Run(host.Services.GetRequiredService<MainForm>());
        }

        private static IHostBuilder CreateHostBuilder()
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets(Assembly.GetExecutingAssembly())
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var hostBuilder = Host.CreateDefaultBuilder();
            hostBuilder.ConfigureAppConfiguration(builder =>
            {
                builder.Sources.Clear();
                builder.AddConfiguration(configuration);
            });

            hostBuilder.ConfigureServices((context, services) =>
            {
                var clientAuth = context.Configuration.GetSection("Twitch").Get<ClientTwitchAuth>() ??
                                 throw new InvalidOperationException("Missing twitch configuration.");

                if (new Uri(clientAuth.RedirectUrl).IsLocalhost(out int port))
                {
                    services.AddHostedService<LocalOAuthReceiver>(provider =>
                        new(port, provider.GetRequiredService<MainForm>())
                    );
                }
                else
                {
                    services.AddSingleton<CommandLineHandler>();
                    services.AddSingleton<NamedPipeService>();
                    services.AddHostedService<AppInstanceService>();
                }

                services.AddSingleton(clientAuth);
                services.AddSingleton(new UserAuthStorage("credentials.json"));
                services.AddSingleton<TwitchApiClient>();

                services.AddSingleton(new TwitchProfileStorage("profiles.json"));
                services.AddSingleton<MainForm>();
            });

            return hostBuilder;
        }
    }
}
