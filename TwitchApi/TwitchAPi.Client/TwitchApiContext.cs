using TwitchAPi.Client.Data;
using TwitchAPi.Client.Responses;

namespace TwitchAPi.Client;

public class TwitchApiContext
{
    public ClientTwitchAuth ClientAuth { get; }
    public UserAuthStorage AuthStorage { get; }
    public UserTwitchAuth UserAuth { get; private set; }
    public TwitchUser? CurrentUser { get; internal set; }

    public HttpClient HttpClient { get; }

    internal TwitchApiContext(ClientTwitchAuth clientAuth, UserAuthStorage authStorage)
    {
        ClientAuth = clientAuth;
        AuthStorage = authStorage;
        UserAuth = authStorage.Load();
        HttpClient = new HttpClient();

        UpdateHeaders();
    }

    internal void SetAuth(UserTwitchAuth userAuth)
    {
        UserAuth = userAuth;
        AuthStorage.Save(userAuth);

        UpdateHeaders();
    }

    private void UpdateHeaders()
    {
        HttpClient.DefaultRequestHeaders.Clear();
        HttpClient.DefaultRequestHeaders.Add("Client-ID", ClientAuth.ClientId);
        HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {UserAuth.AccessToken}");
    }
}
