namespace Samples.ToDo.UI;

#region << Using >>

using System.Collections.Concurrent;
using Extensions;
using Newtonsoft.Json;

#endregion

public static class LocalStorage
{
    #region Constants

    private static readonly ConcurrentDictionary<Key, string> values = new();

    #endregion

    #region Nested Classes

    public enum Key
    {
        Language,

        AuthInfo
    }

    #endregion

    public static bool AddOrUpdate(Key key, string value)
    {
        return values.ContainsKey(key) ? values.TryUpdate(key, value, values[key]) : values.TryAdd(key, value);
    }

    public static T GetOrDefault<T>(Key key)
    {
        if (!values.ContainsKey(key))
            return default(T);

        return values[key].IsNullOrWhitespace() ? default(T) : JsonConvert.DeserializeObject<T>(values[key]);
    }
}