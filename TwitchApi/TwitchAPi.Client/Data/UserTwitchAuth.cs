namespace TwitchApi.Client.Data;

public record UserTwitchAuth(
    string? AccessToken,
    string? RefreshToken
);
