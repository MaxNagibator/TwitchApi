using System.ComponentModel;
using System.Diagnostics;
using TwitchApi.Profile;
using TwitchAPi.Client;
using TwitchAPi.Client.Common;

namespace TwitchApi
{
    public partial class MainForm : Form
    {
        private readonly TwitchApiClient _twitchApiClient;
        private readonly TwitchProfileStorage _profileStorage;

        private BindingList<TwitchProfile> _profiles;

        public MainForm(TwitchApiClient twitchApiClient, TwitchProfileStorage profileStorage)
        {
            InitializeComponent();

            _twitchApiClient = twitchApiClient;
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
                    TwitchScope.UserReadBroadcast | TwitchScope.ChannelManageBroadcast
                );

                Process.Start(new ProcessStartInfo(authorizeUrl) { UseShellExecute = true });

                return;
            }

            InitializeUser();
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
