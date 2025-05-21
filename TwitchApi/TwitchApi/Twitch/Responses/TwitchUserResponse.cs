using System.Text.Json.Serialization;

namespace TwitchApi.Twitch.Responses;

public record TwitchUser(
    [property: JsonPropertyName("id")]
    string Id,
    [property: JsonPropertyName("login")]
    string Login,
    [property: JsonPropertyName("display_name")]
    string DisplayName,
    [property: JsonPropertyName("type")]
    string Type,
    [property: JsonPropertyName("broadcaster_type")]
    string BroadcasterType,
    [property: JsonPropertyName("description")]
    string Description,
    [property: JsonPropertyName("profile_image_url")]
    string ProfileImageUrl,
    [property: JsonPropertyName("offline_image_url")]
    string OfflineImageUrl,
    [property: JsonPropertyName("view_count")]
    int ViewCount,
    [property: JsonPropertyName("created_at")]
    DateTime CreatedAt
);

public record TwitchUserResponse(
    [property: JsonPropertyName("data")]
    List<TwitchUser> Data
);
