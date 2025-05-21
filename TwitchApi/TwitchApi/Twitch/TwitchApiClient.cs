using System.Collections.Specialized;
using System.IdentityModel.Tokens.Jwt;
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

        private string? _token;
        private string? _refreshToken;
        private string? _broadcasterId;

        public TwitchApiClient(string clientId, string clientSecret)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _token = string.Empty;
            _httpClient = new HttpClient();

            UpdateHeaders();
        }

        public string GetCodeAuthLink(string redirect, TwitchScope scope)
        {
            var query = new NameValueCollection
            {
                {"client_id", _clientId},
                {"redirect_uri", redirect},
                {"response_type", "code"},
                {"scope", scope.ToScopeString()},
            };

            return TwitchConstants.OuathAuthorize + query.ToQueryString();
        }

        public async Task<bool> UpdateBroadcast(string? title = null, string? gameId = null, string[]? tags = null, string? broadcasterId = null)
        {
            var query = new NameValueCollection
            {
                {"broadcaster_id", broadcasterId ?? _broadcasterId},
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

        public async Task UpdateTokenByCode(string code, string redirect)
        {
            var parameters = new Dictionary<string, string>
            {
                { "client_id", _clientId},
                { "client_secret", _clientSecret},
                { "code",code},
                { "grant_type", "authorization_code"},
                {"redirect_uri", redirect},
            };


            var content = new FormUrlEncodedContent(parameters);
            var response = await _httpClient.PostAsync(TwitchConstants.OuathToken, content);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var tokenData = JsonSerializer.Deserialize<TokenResponse>(json);

            if (tokenData == null)
                throw new Exception($"Не удалось распарсить ответ от Twitch: {json}");

            _token = tokenData.AccessToken;
            _refreshToken = tokenData.RefreshToken;

            if (!string.IsNullOrEmpty(tokenData.IdToken))
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(tokenData.IdToken);

                _broadcasterId = token.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            }

            UpdateHeaders();
        }

        private void UpdateHeaders()
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Client-ID", _clientId);
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");
        }
    }
}
