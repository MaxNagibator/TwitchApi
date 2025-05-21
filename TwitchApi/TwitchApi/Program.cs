using TwitchApi.Twitch;

namespace TwitchApi
{
    internal static class Program
    {
        public static readonly TwitchApiClient Client = new(
            Properties.Settings.Default.ClientId,
            Properties.Settings.Default.ClientSecret
        );

        public static ApplicationContext Context { get; set; } = null!;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            ApplicationConfiguration.Initialize();

            Context = new ApplicationContext(new AuthForm());
            Application.Run(Context);
        }
    }
}