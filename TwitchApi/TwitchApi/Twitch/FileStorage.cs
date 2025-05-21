using System.Text.Json;

namespace TwitchApi.Twitch;

public class FileStorage : IAuthStorage
{
    private readonly string _filename;

    public FileStorage(string filename)
    {
        _filename = filename;
    }

    public TwitchAuth Load()
    {
        if (!File.Exists(_filename))
            return new TwitchAuth(null, null, null);

        var json = File.ReadAllText(_filename);
        return JsonSerializer.Deserialize<TwitchAuth>(json) ?? new TwitchAuth(null, null, null);
    }

    public void Save(TwitchAuth auth)
    {
        var json = JsonSerializer.Serialize(auth, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(_filename, json);
    }
}
