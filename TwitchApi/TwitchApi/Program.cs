﻿using TwitchApi.Twitch;

namespace TwitchApi
{
    internal static class Program
    {
        public static readonly TwitchApiClient Client = new(
            new AuthStorage("credentials.json")
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

            Context = new(new AuthForm());
            Application.Run(Context);
        }
    }
}
