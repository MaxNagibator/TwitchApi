using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TwitchApi;

public partial class MainForm : Form
{
    private readonly TwitchAuth _settings;

    public MainForm()
    {
        InitializeComponent();
        uiNameTextBox.Text = ".net помойка / делаем бота для твича / upd1";

        var json = File.ReadAllText(@"C:\Sources\TheVSAKeeper\TwitchApi\settings.json");
        //var json = File.ReadAllText(@"E:\bobgroup\projects\TwitchPomogator\settings.json");

        var settings = JsonSerializer.Deserialize<Settings>(json) ?? throw new NullReferenceException("Can't read settings");
        _settings = settings.TwitchAuth;
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
    }

    // TODO: Любые исключения, не обработанные методом "async void", могут привести к сбою процесса
    private async void uiChangeNameButton_Click(object sender, EventArgs args)
    {
        // var accessFile = "E:\\bobgroup\\projects\\TwitchPomogator\\key.txt";
        // var lines = File.ReadAllLines(accessFile);
        // var oauthToken = lines[1];

        await UpdateTwitchStreamTitle(_settings.ClientId, _settings.ClientSecret);
    }

    private async void uiSubmitCodeButton_Click(object sender, EventArgs e)
    {
        var authorizationCode = uiCodeTextBox.Text;
        var oauthToken = await ExchangeCodeForToken(_settings.ClientId, _settings.ClientSecret, authorizationCode);

        await File.WriteAllTextAsync("token.txt", oauthToken);
        MessageBox.Show("Токен успешно сохранен!");
    }

    private async void uiAuthButton_Click(object sender, EventArgs e)
    {
        var authorizationCode = await StartOAuthFlow(_settings.ClientId);
        await ExchangeCodeForToken(_settings.ClientId, _settings.ClientSecret, authorizationCode);
    }

    private async void uiGetBroadcasterButton_Click(object sender, EventArgs e)
    {
        var token = await GetValidToken(_settings.ClientId, _settings.ClientSecret);
        var twitchUser = await GetBroadcasterAsync(_settings.ClientId, token);
        uiInfoTextBox.Text += twitchUser;
    }

    private async Task UpdateTwitchStreamTitle(string clientId, string clientSecret)
    {
        var oauthToken = await GetValidToken(clientId, clientSecret);
        var broadcasterId = await File.ReadAllTextAsync("broadcaster_id.txt");

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Client-ID", clientId);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {oauthToken}");

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

    private async Task<bool> IsTokenValid(string token)
    {
        using var client = new HttpClient();
        var validateUrl = "https://id.twitch.tv/oauth2/validate ";
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        var response = await client.GetAsync(validateUrl);
        return response.IsSuccessStatusCode;
    }

    private async Task<string> RefreshToken(string clientId, string clientSecret)
    {
        var refreshToken = await File.ReadAllTextAsync("refresh_token.txt");

        using var client = new HttpClient();
        var tokenUrl = "https://id.twitch.tv/oauth2/token";

        var formData = new Dictionary<string, string>
        {
            { "client_id", clientId },
            { "client_secret", clientSecret },
            { "refresh_token", refreshToken },
            { "grant_type", "refresh_token" },
        };

        var response = await client.PostAsync(tokenUrl, new FormUrlEncodedContent(formData));
        var jsonResponse = await response.Content.ReadAsStringAsync();
        var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(jsonResponse);

        await File.WriteAllTextAsync("token.txt", tokenResponse.AccessToken);
        await File.WriteAllTextAsync("refresh_token.txt", tokenResponse.RefreshToken);

        return tokenResponse.AccessToken;
    }

    private async Task<string> GetValidToken(string clientId, string clientSecret)
    {
        var token = await File.ReadAllTextAsync("token.txt");

        if (await IsTokenValid(token))
        {
            return token;
        }

        return await RefreshToken(clientId, clientSecret);
    }

    private async Task<string> ExchangeCodeForToken(string clientId, string clientSecret, string authorizationCode)
    {
        string accessToken;

        if (string.IsNullOrWhiteSpace(authorizationCode))
        {
            accessToken = await GetValidToken(clientId, clientSecret);
        }
        else
        {
            using var client = new HttpClient();
            var tokenUrl = "https://id.twitch.tv/oauth2/token";

            var formData = new Dictionary<string, string>
            {
                { "client_id", clientId },
                { "client_secret", clientSecret },
                { "code", authorizationCode },
                { "grant_type", "authorization_code" },
                { "redirect_uri", "http://localhost:8080" },
            };

            var response = await client.PostAsync(tokenUrl, new FormUrlEncodedContent(formData));
            var jsonResponse = await response.Content.ReadAsStringAsync();

            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(jsonResponse);
            var refreshToken = tokenResponse.RefreshToken;
            accessToken = tokenResponse.AccessToken;

            await File.WriteAllTextAsync("token.txt", accessToken);
            await File.WriteAllTextAsync("refresh_token.txt", refreshToken);
        }

        var broadcasterId = await GetBroadcasterIdAsync(clientId, accessToken);
        await File.WriteAllTextAsync("broadcaster_id.txt", broadcasterId);

        MessageBox.Show($"Токен и ID стримера ({broadcasterId}) сохранены!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return accessToken;
    }

    private async Task<string> StartOAuthFlow(string clientId)
    {
        var scope = "channel:manage:broadcast user:read:broadcast";

        var authUrl = $"https://id.twitch.tv/oauth2/authorize"
                      + $"?response_type=code"
                      + $"&client_id={clientId}"
                      + $"&redirect_uri=http://localhost:8080"
                      + $"&scope={scope}";

        var codeTask = StartLocalHttpServerAsync();

        Process.Start(new ProcessStartInfo { FileName = authUrl, UseShellExecute = true });

        var authorizationCode = await codeTask;
        return authorizationCode;
    }

    private async Task<string> GetBroadcasterIdAsync(string clientId, string accessToken)
    {
        var broadcaster = await GetBroadcasterAsync(clientId, accessToken);
        return broadcaster.Id;
    }

    private async Task<TwitchUser> GetBroadcasterAsync(string clientId, string accessToken)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Client-ID", clientId);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

        var response = await client.GetAsync("https://api.twitch.tv/helix/users");
        var content = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode == false)
        {
            throw new($"Ошибка получения данных: {content}");
        }

        var userResponse = JsonSerializer.Deserialize<TwitchUserResponse>(content);

        if (userResponse?.Data == null || userResponse.Data.Count == 0)
        {
            throw new("Пользователь не найден.");
        }

        return userResponse.Data[0];
    }

    private async Task<string> StartLocalHttpServerAsync()
    {
        using var listener = new HttpListener();
        listener.Prefixes.Add("http://localhost:8080/");
        listener.Start();

        try
        {
            var context = await listener.GetContextAsync();
            var request = context.Request;
            var response = context.Response;

            var code = request.QueryString["code"];
            var error = request.QueryString["error"];

            if (string.IsNullOrEmpty(error) == false)
            {
                MessageBox.Show($"Ошибка авторизации: {error}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }

            if (string.IsNullOrEmpty(code))
            {
                MessageBox.Show("Не удалось получить authorization_code.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }

            var responseString =
                """
                <!DOCTYPE html>
                <html lang="ru">
                <head><meta charset="UTF-8"</head>
                ><body>Вы можете закрыть это окно.</body>
                </html>
                """;

            var buffer = Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            response.Close();

            return code;
        }
        finally
        {
            listener.Stop();
        }
    }
}

public record TwitchUser(
    [property: JsonPropertyName("id")]
    string Id,
    [property: JsonPropertyName("login")]
    string Login,
    [property: JsonPropertyName("display_name")]
    string DisplayName,
    [property: JsonPropertyName("type")]
    string Type,
    [property: JsonPropertyName("broadcaster_type")]
    string BroadcasterType,
    [property: JsonPropertyName("description")]
    string Description,
    [property: JsonPropertyName("profile_image_url")]
    string ProfileImageUrl,
    [property: JsonPropertyName("offline_image_url")]
    string OfflineImageUrl,
    [property: JsonPropertyName("view_count")]
    int ViewCount,
    [property: JsonPropertyName("created_at")]
    DateTime CreatedAt
);

public record TwitchUserResponse(
    [property: JsonPropertyName("data")]
    List<TwitchUser> Data
);

public record TokenResponse(
    [property: JsonPropertyName("access_token")]
    string AccessToken,
    [property: JsonPropertyName("expires_in")]
    int ExpiresIn,
    [property: JsonPropertyName("refresh_token")]
    string RefreshToken,
    [property: JsonPropertyName("scope")]
    List<string> Scope,
    [property: JsonPropertyName("token_type")]
    string TokenType
);

public record Settings(
    [property: JsonPropertyName("TwitchAuth")]
    TwitchAuth TwitchAuth
);

public record TwitchAuth(
    [property: JsonPropertyName("ClientId")]
    string ClientId,
    [property: JsonPropertyName("ClientSecret")]
    string ClientSecret
);
