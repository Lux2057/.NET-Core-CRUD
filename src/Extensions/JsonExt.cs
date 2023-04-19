namespace CRUD.Extensions;

#region << Using >>

using Newtonsoft.Json;

#endregion

public static class JsonExt
{
    /// <summary>
    ///     A wrapper for Newtonsoft JsonConvert.SerializeObject function
    /// </summary>
    public static string ToJsonString(this object? value, JsonSerializerSettings? settings = null)
    {
        return value == null ? string.Empty : JsonConvert.SerializeObject(value, settings);
    }
}