using System.Text.Json;
using System.Text;

namespace TwitchApi
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            uiNameTextBox.Text = ".net ������� / ������ ���� ��� ����� / upd1";
        }

        private async void uiChangeNameButton_Click(object sender, EventArgs e)
        {
            var newName = uiNameTextBox.Text;

            var accessFile = "E:\\bobgroup\\projects\\TwitchPomogator\\key.txt";
            var lines = System.IO.File.ReadAllLines(accessFile);


            string clientId = "gvo8mc3xtlkg79k8gdsj1gt8ylb2ht";
            string oauthToken = lines[1];
            string broadcasterId = "177128531";
            await UpdateTwitchStreamTitle(clientId, oauthToken, broadcasterId, newName);
        }

        async Task UpdateTwitchStreamTitle(string clientId, string oauthToken, string broadcasterId, string newTitle)
        {
            using (HttpClient client = new HttpClient())
            {
                // ������������� ���������
                client.DefaultRequestHeaders.Add("Client-ID", clientId);
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {oauthToken.Replace("oauth:", "")}");

                string url = $"https://api.twitch.tv/helix/channels?broadcaster_id={broadcasterId}";

                //var bla = client.GetAsync(url);
                //textBox1.Text = await bla.Result.Content.ReadAsStringAsync();
                //return;
                //{"data":[{"broadcaster_id":"177128531","broadcaster_login":"bobito217","broadcaster_name":"BOBito217","broadcaster_language":"ru","game_id":"491931","game_name":"Escape from Tarkov","title":"Escape From Tarkov / ��������� �������","delay":0,"tags":["�������"],"content_classification_labels":["MatureGame","ProfanityVulgarity"],"is_branded_content":false}]}
                //                { "data":[{ "broadcaster_id":"177128531","broadcaster_login":"bobito217","broadcaster_name":"BOBito217","broadcaster_language":"ru","game_id":"1469308723","game_name":"Software and Game Development","title":".net ������� / ������ ���� ��� ����� / ���� �����","delay":0,"tags":["�������"],"content_classification_labels":["ProfanityVulgarity"],"is_branded_content":false}]}
                // ���� ������� (������ ������ title)

                //!!!���������� � ������!!!
                string jsonBody;
                if (uiTarkRadioButton.Checked)
                {
                    var requestBody = new
                    {
                        title = "Escape From Tarkov / ��������� �������",
                    //    tags = new[] { "�������", "����������������",
                    //    ".net", "���������� ��", "������ ����"
                    //},
                        game_id = "491931" // Software and Game Development
                    };

                    jsonBody = JsonSerializer.Serialize(requestBody);
                }
                else
                {
                    var requestBody = new
                    {
                        title = ".net ������� / ������ ���� ��� ����� #2",
                    //    tags = new[] { "�������", "����������������",
                    //    ".net", "���������� ��", "������ ����"
                    //},
                        game_id = "1469308723" // Software and Game Development
                    };

                    jsonBody = JsonSerializer.Serialize(requestBody);
                }

                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PatchAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("�������� ���������� ������� ��������!", "!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show(responseContent, $"������: {response.StatusCode}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
