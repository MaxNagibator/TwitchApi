using System.Text.Json;
using TwitchAPi.Client.Responses;

namespace TwitchAPi.Client.Services;

public class UserService
{
    private readonly TwitchApiContext _context;

    internal UserService(TwitchApiContext context)
    {
        _context = context;
    }

    public async Task<List<TwitchUser>> GetUsers(string? id = null, string? login = null)
    {
        var response = await _context.HttpClient.GetAsync("https://api.twitch.tv/helix/users");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var userResponse = JsonSerializer.Deserialize<TwitchUserResponse>(json);

        if (userResponse == null)
        {
            throw new($"Failed parse response from twitch: {json}");
        }

        return userResponse.Data;
    }
}
