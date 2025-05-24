using System.Collections.Specialized;
using System.Text.Json;
using System.Text;
using TwitchAPi.Client.Common;

namespace TwitchAPi.Client.Services;

public class ChannelService
{
    private readonly TwitchApiContext _context;

    internal ChannelService(TwitchApiContext context)
    {
        _context = context;
    }

    public async Task<bool> UpdateBroadcast(string? title = null, string? gameId = null, string[]? tags = null)
    {
        if (_context.CurrentUser?.Id == null)
        {
            throw new InvalidOperationException("Broadcaster id is empty");
        }

        var query = new NameValueCollection
        {
            { "broadcaster_id", _context.CurrentUser.Id },
        };

        var requestBody = new
        {
            title,
            game_id = gameId,
            tags,
        };

        var jsonBody = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
        var response = await _context.HttpClient.PatchAsync(TwitchConstants.Channels + query.ToQueryString(), content);

        return response.IsSuccessStatusCode;
    }
}
