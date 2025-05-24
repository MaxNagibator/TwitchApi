using System.Collections.Specialized;
using System.Text.Json;
using TwitchAPi.Client.Common;
using TwitchAPi.Client.Responses;

namespace TwitchAPi.Client.Services;

public class AuthService
{
    private readonly TwitchApiContext _context;

    internal AuthService(TwitchApiContext context)
    {
        _context = context;
    }

    public async ValueTask<bool> IsTokenValid()
    {
        if (string.IsNullOrEmpty(_context.UserAuth.AccessToken))
        {
            return false;
        }

        var response = await _context.HttpClient.GetAsync(TwitchConstants.OauthValidate);
        return response.IsSuccessStatusCode;
    }

    public string GetCodeAuthLink(TwitchScope scope)
    {
        var query = new NameValueCollection
        {
            { "client_id", _context.ClientAuth.ClientId },
            { "redirect_uri", _context.ClientAuth.RedirectUrl },
            { "response_type", "code" },
            { "scope", scope.ToScopeString() },
        };

        return TwitchConstants.OuathAuthorize + query.ToQueryString();
    }

    public async Task RefreshTokenByCode(string code)
    {
        var parameters = new Dictionary<string, string>
        {
            { "client_id", _context.ClientAuth.ClientId },
            { "client_secret", _context.ClientAuth.ClientSecret },
            { "code", code },
            { "grant_type", "authorization_code" },
            { "redirect_uri", _context.ClientAuth.RedirectUrl },
        };

        await RefreshTokenInternal(parameters);
    }

    public async Task RefreshToken()
    {
        if (_context.UserAuth.RefreshToken == null)
        {
            throw new InvalidOperationException("Refresh token is empty");
        }

        var parameters = new Dictionary<string, string>
        {
            { "client_id", _context.ClientAuth.ClientId },
            { "client_secret", _context.ClientAuth.ClientSecret },
            { "refresh_token", _context.UserAuth.RefreshToken },
            { "grant_type", "refresh_token" },
        };

        await RefreshTokenInternal(parameters);
    }

    private async Task RefreshTokenInternal(Dictionary<string, string> parameters)
    {
        var content = new FormUrlEncodedContent(parameters);
        var response = await _context.HttpClient.PostAsync(TwitchConstants.OuathToken, content);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var tokenData = JsonSerializer.Deserialize<TokenResponse>(json);

        if (tokenData == null)
        {
            throw new($"Failed parse response from twitch: {json}");
        }

        _context.SetAuth(new(tokenData.AccessToken, tokenData.RefreshToken));
    }
}
