namespace TwitchAPi.Client.Data;

public record UserTwitchAuth(
    string? AccessToken,
    string? RefreshToken
);
