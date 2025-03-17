using BusinessLogicLayer.DTOs;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

public static class SessionHelper
{
    private const string GuestCartKey = "GuestCart"; // Add this constant for cart storage

    public static void SetObject<T>(this ISession session, string key, T value)
    {
        session.Set(key, System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)));
    }


    public static T GetObject<T>(this ISession session, string key)
    {
        var value = session.TryGetValue(key, out var bytes) ? System.Text.Encoding.UTF8.GetString(bytes) : null;
        return value == null ? Activator.CreateInstance<T>() : JsonConvert.DeserializeObject<T>(value);
    }

    // Add helper methods for cart management
    public static List<CartDetailsDto> GetGuestCart(this ISession session)
    {
        return session.GetObject<List<CartDetailsDto>>(GuestCartKey);
    }

    public static void SaveGuestCart(this ISession session, List<CartDetailsDto> cart)
    {
        session.SetObject(GuestCartKey, cart);
    }

    public static void ClearGuestCart(this ISession session)
    {
        session.Remove(GuestCartKey);
    }
}
