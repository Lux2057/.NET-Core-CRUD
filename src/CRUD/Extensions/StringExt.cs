namespace CRUD.Extensions;

public static class StringExt
{
    /// <summary>
    ///     A wrapper for String.IsNullOrEmpty function
    /// </summary>
    public static bool IsNullOrEmpty(this string? value)
    {
        return string.IsNullOrEmpty(value);
    }

    /// <summary>
    ///     A wrapper for String.IsNullOrWhiteSpace function
    /// </summary>
    public static bool IsNullOrWhitespace(this string value)
    {
        return string.IsNullOrWhiteSpace(value);
    }

    /// <summary>
    ///     A wrapper for Enum.Parse function
    /// </summary>
    public static T ToEnum<T>(this string value) where T : struct, Enum
    {
        return (T)Enum.Parse(typeof(T), value);
    }

    /// <summary>
    ///     A wrapper for String.Join function
    /// </summary>
    public static string ToJoinedString(this IEnumerable<string> enumerable, string? separator = default)
    {
        var enumerableArray = enumerable.ToArrayOrEmpty();
        var currentSeparator = separator.IsNullOrEmpty() ? string.Empty : separator;

        return !enumerableArray.Any() ? string.Empty : string.Join(currentSeparator, enumerableArray);
    }
}