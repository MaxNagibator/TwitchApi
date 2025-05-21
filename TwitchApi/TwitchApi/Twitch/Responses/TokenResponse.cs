using System.Text.Json.Serialization;

namespace TwitchApi.Twitch.Responses
{
    public class TokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("id_token")]
        public string? IdToken { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonPropertyName("scope")]
        public List<string> Scope { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }
    }

}
