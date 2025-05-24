using System.Text;
using System.Text.Json;

namespace TwitchApi.Profile;

public class TwitchProfileStorage
{
    private readonly string _filePath;

    private static List<TwitchProfile> Default = new List<TwitchProfile>
    {
       new TwitchProfile("Tarkov", "Escape From Tarkov / Страдания новичка", "491931"),
       new TwitchProfile(".Net помойка", ".net помойка / делаем бота для твича #2", "1469308723"),
    };

    public TwitchProfileStorage(string filePath)
    {
        _filePath = filePath;
    }

    public List<TwitchProfile> LoadProfiles()
    {
        if (!File.Exists(_filePath))
            return new List<TwitchProfile>(Default);

        var json = File.ReadAllText(_filePath, Encoding.UTF8);
        return JsonSerializer.Deserialize<List<TwitchProfile>>(json) ?? new List<TwitchProfile>(Default);
    }

    public void SaveProfiles(List<TwitchProfile> profiles)
    {
        var json = JsonSerializer.Serialize(profiles, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json, Encoding.UTF8);
    }
}
