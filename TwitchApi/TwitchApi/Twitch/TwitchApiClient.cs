using System.Collections.Specialized;
using System.Text;
using System.Text.Json;
using TwitchApi.Twitch.Responses;

namespace TwitchApi.Twitch
{
    public class TwitchApiClient
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly HttpClient _httpClient;
        private readonly IAuthStorage _authStorage;

        private TwitchAuth _auth;

        public TwitchApiClient(string clientId, string clientSecret, IAuthStorage authStorage)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _httpClient = new();
            _authStorage = authStorage;
            _auth = _authStorage.Load();

            UpdateHeaders();
        }

        public async ValueTask<bool> IsTokenValid()
        {
            if (string.IsNullOrEmpty(_auth.AccessToken))
                return false;

            var response = await _httpClient.GetAsync(TwitchConstants.OauthValidate);
            return response.IsSuccessStatusCode;
        }

        public string GetCodeAuthLink(string redirect, TwitchScope scope)
        {
            var query = new NameValueCollection
            {
                { "client_id", _clientId },
                { "redirect_uri", redirect },
                { "response_type", "code" },
                { "scope", scope.ToScopeString() },
            };

            return TwitchConstants.OuathAuthorize + query.ToQueryString();
        }

        public async Task RefreshTokenByCode(string code, string redirect)
        {
            var parameters = new Dictionary<string, string>
            {
                { "client_id", _clientId },
                { "client_secret", _clientSecret },
                { "code", code },
                { "grant_type", "authorization_code" },
                { "redirect_uri", redirect },
            };

            await RefreshToken(parameters);
        }

        public async Task RefreshToken()
        {
            if (_auth.RefreshToken == null)
                throw new InvalidOperationException("Refresh token is empty");

            var parameters = new Dictionary<string, string>
            {
                { "client_id", _clientId },
                { "client_secret", _clientSecret },
                { "refresh_token", _auth.RefreshToken },
                { "grant_type", "refresh_token" },
            };

            await RefreshToken(parameters);
        }

        public async Task<bool> UpdateBroadcast(string? title = null, string? gameId = null, string[]? tags = null)
        {
            if (_auth.BroadcasterId == null)
                throw new InvalidOperationException("Broadcaster id is empty");

            var query = new NameValueCollection
            {
                { "broadcaster_id", _auth.BroadcasterId },
            };

            var requestBody = new
            {
                title,
                game_id = gameId,
                tags,
            };

            var jsonBody = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = await _httpClient.PatchAsync(TwitchConstants.Channels + query.ToQueryString(), content);

            return response.IsSuccessStatusCode;
        }

        public async Task<List<TwitchUser>> GetUsers(string? id = null, string? login = null)
        {
            var response = await _httpClient.GetAsync("https://api.twitch.tv/helix/users");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var userResponse = JsonSerializer.Deserialize<TwitchUserResponse>(json);

            if (userResponse == null)
                throw new($"Failed parse response from twitch: {json}");

            return userResponse.Data;
        }

        private async Task RefreshToken(Dictionary<string, string> parameters)
        {
            var content = new FormUrlEncodedContent(parameters);
            var response = await _httpClient.PostAsync(TwitchConstants.OuathToken, content);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var tokenData = JsonSerializer.Deserialize<TokenResponse>(json);

            if (tokenData == null)
                throw new($"Failed parse response from twitch: {json}");

            UpdateHeaders(tokenData.AccessToken);
            var users = await GetUsers();

            _auth = new(tokenData.AccessToken, tokenData.RefreshToken, users[0].Id);
            _authStorage.Save(_auth);
        }

        private void UpdateHeaders(string? accessToken = null)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Client-ID", _clientId);
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken ?? _auth.AccessToken}");
        }
    }
}
