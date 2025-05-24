namespace TwitchApi.Profile;

public record TwitchProfile(string Name, string Title, string CategoryId)
{
    public override string ToString() => Name;
}
