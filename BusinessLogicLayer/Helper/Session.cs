using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

public static class SessionHelper
{
    public static void SetObject<T>(this ISession session, string key, T value)
    {
        session.Set(key, System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)));
    }

    public static T GetObject<T>(this ISession session, string key)
    {
        var value = session.TryGetValue(key, out var bytes) ? System.Text.Encoding.UTF8.GetString(bytes) : null;
        return value == null ? default : JsonConvert.DeserializeObject<T>(value);
    }
}
