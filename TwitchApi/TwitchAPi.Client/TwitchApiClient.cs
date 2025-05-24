using TwitchAPi.Client.Data;
using TwitchAPi.Client.Services;

namespace TwitchAPi.Client;

public class TwitchApiClient
{
    private readonly TwitchApiContext _context;

    public AuthService Auth { get; }
    public UserService User { get; }
    public ChannelService Channel { get; }

    public string? UserName => _context.CurrentUser?.DisplayName;

    public TwitchApiClient(ClientTwitchAuth clientAuth, UserAuthStorage authStorage)
    {
        _context = new TwitchApiContext(clientAuth, authStorage);

        Auth = new AuthService(_context);
        User = new UserService(_context);
        Channel = new ChannelService(_context);
    }

    public async Task InitializeUser()
    {
        var users = await User.GetUsers();
        _context.CurrentUser = users[0];
    }
}

