namespace TwitchApi.Twitch;

public interface IAuthStorage
{
    TwitchAuth Load();
    void Save(TwitchAuth auth);
}
