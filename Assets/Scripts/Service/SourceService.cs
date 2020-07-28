
using System.IO;
using UnityEngine;

public static class SourceService
{
    public static void Serialize(string path, object value)
    {
        string json = JsonUtility.ToJson(value);

        CreateDirectory(Path.GetDirectoryName(path));

        File.WriteAllText(path, json);
    }

    public static T Deserialize<T>(string path)
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            return JsonUtility.FromJson<T>(json);
        }

        return default;
    }

    public static void CreateDirectory(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
}
