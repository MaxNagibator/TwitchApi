using System.ComponentModel;
using System.Diagnostics;
using TwitchApi.Profile;
using TwitchApi.Client;
using TwitchApi.Client.Common;
using TwitchLib.Client.Models;
using TwitchLib.Client;
using TwitchLib.Communication.Models;


using TwitchLib.Communication.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Client.Models;
using TwitchLib.Client;

namespace TwitchApi
{
    public partial class MainForm : Form
    {
        private readonly TwitchApiClient _twitchApiClient;
        private readonly TwitchProfileStorage _profileStorage;

        private BindingList<TwitchProfile> _profiles;
        private TwitchClient client;

        public MainForm(TwitchApiClient TwitchApiClient, TwitchProfileStorage profileStorage)
        {
            InitializeComponent();

            _twitchApiClient = TwitchApiClient;
            _profileStorage = profileStorage;

            _profiles = new BindingList<TwitchProfile>(profileStorage.LoadProfiles());
            uiProfileComboBox.DataSource = _profiles;
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {

            updateButton.Enabled = false;
            var tokenIsValid = await _twitchApiClient.Auth.IsTokenValid();

            if (!tokenIsValid)
            {
                var authorizeUrl = _twitchApiClient.Auth.GetCodeAuthLink(
                      TwitchScope.UserReadBroadcast
                    | TwitchScope.ChannelManageBroadcast
                    | TwitchScope.ChannelBot
                    | TwitchScope.UserReadChat
                    | TwitchScope.UserWriteChat
                    | TwitchScope.UserReadWhispers
                    | TwitchScope.UserManageWhispers
                    | TwitchScope.ModeratorManageChatMessages
                );

                Process.Start(new ProcessStartInfo(authorizeUrl) { UseShellExecute = true });

                return;
            }

            InitializeUser();


            // 1. Настройка подключения
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30),

            };

            var customClient = new WebSocketClient(clientOptions);
            client = new TwitchClient(customClient);


            // 2. Авторизация бота
            var credentials = new ConnectionCredentials(
                "bobito217",//"логин_бота",  // Например, "MyCoolBot"
                _twitchApiClient.Context.UserAuth.AccessToken //"oauth:токен"  // Получить тут: https://twitchtokengenerator.com
            );

            client.Initialize(credentials, "bobito217"); // Например, "ваш_ник"

            // 3. Подписка на события
            client.OnConnected += (sender, e) =>
            {
                Text += ($"Бот подключён к каналу!");

                //Console.WriteLine("Бот подключён к Twitch!");
                client.JoinChannel("ваш_канал");
            };

            client.OnMessageReceived += (sender, e) =>
            {
                // Ответ на сообщение в чате
                if (e.ChatMessage.Message.ToLower() == "!привет")
                {
                    client.SendMessage(e.ChatMessage.Channel,
                        $"Привет, {e.ChatMessage.Username}!");
                }

                Text += ($"{e.ChatMessage.Username}: {e.ChatMessage.Message}");
            };

            client.OnConnectionError += (sender, e) =>
                  Console.WriteLine($"Ошибка подключения: {e.Error.Message}");

            client.OnError += (sender, e) =>
                Console.WriteLine($"Ошибка: {e.Exception.Message}");

            // 4. Запуск
            client.Connect();
            client.JoinChannel("bobito217");
            client.SendMessage(new JoinedChannel("bobito217"), "дорова ребят");
            //Text += ("Бот запущен. Нажмите Enter для выхода.");

            return;
        }

        public async void OnCodeRecevied(string code)
        {
            await _twitchApiClient.Auth.RefreshTokenByCode(code);

            Invoke(Activate);
            InitializeUser();
        }

        private async void InitializeUser()
        {
            await _twitchApiClient.InitializeUser();

            Invoke(() =>
            {
                updateButton.Enabled = true;
                uiUserLabel.Text = _twitchApiClient.UserName;
            });
        }

        private void uiProfileComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (uiProfileComboBox.SelectedItem is TwitchProfile selectedProfile)
            {
                uiTitleTextBox.Text = selectedProfile.Title;
                uiCategoryIdTextBox.Text = selectedProfile.CategoryId;
            }
        }

        private void uiCreateProfileButton_Click(object sender, EventArgs e)
        {
            var profileName = InputDialogHelper.Show("Введите название профиля:");
            if (profileName == null)
            {
                return;
            }

            var profile = new TwitchProfile(profileName, uiTitleTextBox.Text, uiCategoryIdTextBox.Text);

            _profiles.Add(profile);
            uiProfileComboBox.SelectedIndex = _profiles.Count - 1;
        }

        private void uiDeleteProfileButton_Click(object sender, EventArgs e)
        {
            _profiles.RemoveAt(uiProfileComboBox.SelectedIndex);
        }

        private async void updateButton_Click(object sender, EventArgs e)
        {
            var updateResult = await _twitchApiClient.Channel.UpdateBroadcast(uiTitleTextBox.Text, uiCategoryIdTextBox.Text);

            Invoke(() =>
            {
                if (!updateResult)
                {
                    MessageBox.Show(this, "Не удалось обновить!", "!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show(this, "Обновлено!", "!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            });
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            _profileStorage.SaveProfiles(_profiles.ToList());
        }
    }
}
