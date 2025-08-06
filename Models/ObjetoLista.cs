using Newtonsoft.Json;
public static class Objetos
{
    public static string ListToString<T>(List<T> lista)
    {
        return JsonConvert.SerializeObject(lista);
    }

    public static List<T>? StringToList<T>(string json)
    {
        if (string.IsNullOrEmpty(json))
            return default;

        else
            return JsonConvert.DeserializeObject<List<T>>(json);
    }
}