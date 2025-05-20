using System.Text;
using System.Text.Json;

namespace TwitchApi;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();
        uiNameTextBox.Text = ".net помойка / делаем бота для твича / upd1";
    }

    // TODO: Любые исключения, не обработанные методом "async void", могут привести к сбою процесса
    private async void uiChangeNameButton_Click(object sender, EventArgs args)
    {
        var newName = uiNameTextBox.Text;

        var accessFile = "E:\\bobgroup\\projects\\TwitchPomogator\\key.txt";
        var lines = File.ReadAllLines(accessFile);

        var clientId = "gvo8mc3xtlkg79k8gdsj1gt8ylb2ht";
        var oauthToken = lines[1];
        var broadcasterId = "177128531";
        await UpdateTwitchStreamTitle(clientId, oauthToken, broadcasterId, newName);
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
    }

    private async Task UpdateTwitchStreamTitle(string clientId, string oauthToken, string broadcasterId, string newTitle)
    {
        using var client = new HttpClient();

        // Устанавливаем заголовки
        client.DefaultRequestHeaders.Add("Client-ID", clientId);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {oauthToken.Replace("oauth:", "")}");

        var url = $"https://api.twitch.tv/helix/channels?broadcaster_id={broadcasterId}";

        //var bla = client.GetAsync(url);
        //textBox1.Text = await bla.Result.Content.ReadAsStringAsync();
        //return;
        //{"data":[{"broadcaster_id":"177128531","broadcaster_login":"bobito217","broadcaster_name":"BOBito217","broadcaster_language":"ru","game_id":"491931","game_name":"Escape from Tarkov","title":"Escape From Tarkov / Страдания новичка","delay":0,"tags":["Русский"],"content_classification_labels":["MatureGame","ProfanityVulgarity"],"is_branded_content":false}]}
        //                { "data":[{ "broadcaster_id":"177128531","broadcaster_login":"bobito217","broadcaster_name":"BOBito217","broadcaster_language":"ru","game_id":"1469308723","game_name":"Software and Game Development","title":".net помойка / делаем бота для твича / всем любви","delay":0,"tags":["Русский"],"content_classification_labels":["ProfanityVulgarity"],"is_branded_content":false}]}
        // Тело запроса (меняем только title)

        //!!!продолжить с тэгами!!!
        string jsonBody;

        if (uiTarkRadioButton.Checked)
        {
            var requestBody = new
            {
                title = "Escape From Tarkov / Страдания новичка",
                //    tags = new[] { "русский", "программирование",
                //    ".net", "разработка ПО", "знание сила"
                //},
                game_id = "491931", // Software and Game Development
            };

            jsonBody = JsonSerializer.Serialize(requestBody);
        }
        else
        {
            var requestBody = new
            {
                title = ".net помойка / делаем бота для твича #2",
                //    tags = new[] { "русский", "программирование",
                //    ".net", "разработка ПО", "знание сила"
                //},
                game_id = "1469308723", // Software and Game Development
            };

            jsonBody = JsonSerializer.Serialize(requestBody);
        }

        using var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        var response = await client.PatchAsync(url, content);

        if (response.IsSuccessStatusCode)
        {
            MessageBox.Show("Название трансляции успешно изменено!", "!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            MessageBox.Show(responseContent, $"Ошибка: {response.StatusCode}", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
