namespace Samples.ToDo.UI;

#region << Using >>

using System.Collections.Concurrent;

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

    public static string GetOrDefault(Key key)
    {
        return values.ContainsKey(key) ? values[key] : default;
    }
}