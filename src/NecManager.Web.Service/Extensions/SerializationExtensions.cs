namespace NecManager.Web.Service.Extensions;
using System.Net.Http;
using System.Text;
using System.Text.Json;

internal static class SerializationExtensions
{
    internal static StringContent ToStringContent<T>(this T obj)
    {
        return new StringContent(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");
    }
}
