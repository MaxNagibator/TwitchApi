namespace TwitchApi.Twitch;

public record TwitchAuth(
    string? AccessToken,
    string? RefreshToken,
    string? BroadcasterId
);
