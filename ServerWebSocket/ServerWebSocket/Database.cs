using Newtonsoft.Json;

public static class Database
{
    private static readonly string path = "users.json";

    public static List<User> Carregar()
    {
        if (!File.Exists(path))
            return new List<User>();

        var json = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
    }

    public static void Salvar(List<User> usuarios)
    {
        var json = JsonConvert.SerializeObject(usuarios, Formatting.Indented);
        File.WriteAllText(path, json);
    }
}
