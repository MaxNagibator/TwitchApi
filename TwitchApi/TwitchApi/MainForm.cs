namespace TwitchApi
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private async void updateButton_Click(object sender, EventArgs e)
        {
            var updateResult = await Program.Client.UpdateBroadcast(titleTextBox.Text, gameTextBot.Text);

            if (updateResult)
            {
                MessageBox.Show("Обновлено!", "!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Не удалось обновить!", "!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
