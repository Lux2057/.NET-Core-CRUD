namespace Samples.ToDo.UI;

#region << Using >>

using System.Collections.Concurrent;
using Extensions;
using Microsoft.JSInterop;

#endregion

public static class LocalStorage
{
    #region Constants

    private static readonly ConcurrentDictionary<string, string> values = new();

    #endregion

    #region Nested Classes

    public enum Key
    {
        Language,

        AuthInfo
    }

    #endregion

    static bool addOrUpdate(string key, string value)
    {
        return values.ContainsKey(key) ? values.TryUpdate(key, value, values[key]) : values.TryAdd(key, value);
    }

    public static async Task SetAsync<T>(IJSRuntime js, Key key, T value)
    {
        var stringKey = key.ToString();

        var jsonValue = value?.ToJsonString() ?? string.Empty;

        await js.SetLocalStorageValue(stringKey, jsonValue);

        addOrUpdate(stringKey, jsonValue);
    }

    public static string GetBuiltInValueOrDefault(Key key)
    {
        return values.ContainsKey(key.ToString()) ? values[key.ToString()] : default;
    }

    public static async Task FetchBuiltInValuesAsync(IJSRuntime js)
    {
        var keys = Enum.GetValues<Key>().ToArray();

        foreach (var key in keys)
        {
            var stringKey = key.ToString();

            var value = await js.GetLocalStorageValueAsync(stringKey);

            addOrUpdate(stringKey, value);
        }
    }
}