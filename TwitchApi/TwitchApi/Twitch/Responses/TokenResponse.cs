using System.Text.Json.Serialization;

namespace TwitchApi.Twitch.Responses;

public record TokenResponse(
    [property: JsonPropertyName("access_token")] string AccessToken,
    [property: JsonPropertyName("expires_in")] int ExpiresIn,
    [property: JsonPropertyName("id_token")] string? IdToken,
    [property: JsonPropertyName("refresh_token")] string RefreshToken,
    [property: JsonPropertyName("scope")] List<string> Scope,
    [property: JsonPropertyName("token_type")] string TokenType
);

