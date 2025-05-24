namespace TwitchAPi.Client.Data;
public record ClientTwitchAuth(
    string ClientId,
    string ClientSecret,
    string RedirectUrl
);
