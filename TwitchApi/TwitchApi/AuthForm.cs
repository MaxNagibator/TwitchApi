using System.Diagnostics;
using System.Net;
using System.Text;
using TwitchApi.Twitch;

namespace TwitchApi
{
    public partial class AuthForm : Form
    {
        private const  int ListenPort = 3012;
        private string RedirectUrl => $"http://localhost:{ListenPort}";

        public AuthForm()
        {
            InitializeComponent();
        }

        private async void authButton_Click(object sender, EventArgs e)
        {
            var code = await GetAuthCode();
            await Program.Client.UpdateTokenByCode(code, RedirectUrl);

            Program.Context.MainForm = new MainForm();
            Program.Context.MainForm.Show();
            Close();
        }

        private async Task<string> GetAuthCode()
        {
            // Openid нужен чтобы сразу получить id текущего юзера из id_token (это jwt)
            // https://dev.twitch.tv/docs/authentication/getting-tokens-oidc/#validating-an-id-token

            var authorizeUrl = Program.Client.GetCodeAuthLink(
                    RedirectUrl,
                   TwitchScope.Openid | TwitchScope.ChannelManageBroadcast
            );

            Process.Start(new ProcessStartInfo(authorizeUrl) { UseShellExecute = true });
            return await ListenCode();
        }

        private async Task<string> ListenCode()
        {
            var listener = new HttpListener();
            listener.Prefixes.Add($"http://localhost:{ListenPort}/");
            listener.Start();

            while (true)
            {

                var context = await listener.GetContextAsync();
                var request = context.Request;
                var code = request.QueryString["code"];

                if (string.IsNullOrEmpty(code))
                    continue;

                string responseString = @"
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

                byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                context.Response.ContentLength64 = buffer.Length;
                context.Response.ContentType = "text/html";
                await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                context.Response.OutputStream.Close();


                return code;
            }
        }
    }
}
